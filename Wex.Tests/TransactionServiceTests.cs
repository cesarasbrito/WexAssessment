using System;
using System.Threading.Tasks;
using Moq;
using Wex.Application.Services;
using Wex.Domain.Entities;
using Wex.Domain.Repositories;
using Wex.Application.Dtos;
using Wex.Infrastructure.External;
using Xunit;

namespace Wex.Tests;

public class TransactionServiceTests
{


    /// <summary>
    /// test GetTransaction with country ex Brazil-Real.
    /// Expect that ExchangeRate and ConvertedAmount return values.
    /// </summary>
    /// <returns>assincronous Task</returns>
    [Fact]
    public async Task Test_GetTransaction_WithCountry_ReturnsConvertedAmount()
    {
        var transaction = new Transaction("Test", DateTime.Today, 100.00m);

        Console.WriteLine($"Created transaction with ID: {transaction.Id}, AmountUsd: {transaction.AmountUsd}");

        var repoMock = new Mock<ITransactionRepository>();
        repoMock.Setup(r => r.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        var treasuryApiMock = new Mock<ITreasuryApiClient>();
        treasuryApiMock.Setup(api => api.GetExchangeRateAsync(transaction.TransactionDate, "Brazil-Real"))
            .ReturnsAsync(5.0m); // simulate exchange rate

        var service = new TransactionService(repoMock.Object, treasuryApiMock.Object);

        var result = await service.GetTransaction(transaction.Id, "Brazil-Real");
        Console.WriteLine($"Service returned: {result}");
        // Assert
        Assert.NotNull(result);
        Assert.Equal(transaction.Id, result.Id);
        Assert.Equal(500m, result.ConvertedAmount); // 100 * 5.0
        Assert.Equal(5.0m, result.ExchangeRate);
    }

    /// <summary>
    /// test GetTransaction without inform the country ex Brazil-Real.
    /// Expect that ExchangeRate and ConvertedAmount return null.
    /// </summary>
    /// <returns>assincronous Task</returns>
    [Fact]
    public async Task Test_GetTransaction_WithoutCountry_ReturnsNullExchangeRate()
    {
        var transactionId = Guid.NewGuid();
        var transaction = new Transaction("Test", DateTime.Today, 100m);

        var repoMock = new Mock<ITransactionRepository>();
        repoMock.Setup(r => r.GetByIdAsync(transactionId)).ReturnsAsync(transaction);

        var treasuryApiMock = new Mock<ITreasuryApiClient>();

        var service = new TransactionService(repoMock.Object, treasuryApiMock.Object);

        var result = await service.GetTransaction(transactionId);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ExchangeRate);
        Assert.Null(result.ConvertedAmount);
    }

    /// <summary>
    /// tests CreateTransaction method
    /// </summary>
    [Fact]
    public async Task Test_CreateTransaction_ReturnsTransactionId()
    {
        var repositoryMock = new Moq.Mock<ITransactionRepository>();
        var treasuryApiMock = new Moq.Mock<ITreasuryApiClient>();
        Transaction? savedTransaction = null;

        repositoryMock.Setup(r => r.SaveAsync(Moq.It.IsAny<Transaction>()))
            .Callback<Transaction>(t => savedTransaction = t)
            .Returns(Task.CompletedTask);

        var service = new TransactionService(repositoryMock.Object, treasuryApiMock.Object);

        var description = "Random Transaction for test purposes";
        var date = DateTime.Today;
        var amountUsd = 150.00m;

        var resultId = await service.CreateTransaction(description, date, amountUsd);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        Assert.NotNull(savedTransaction);
        Assert.Equal(description, savedTransaction.Description);
        Assert.Equal(date, savedTransaction.TransactionDate);
        Assert.Equal(amountUsd, savedTransaction.AmountUsd);
        Assert.Equal(resultId, savedTransaction.Id);
    }
    
    [Fact]
    public async Task Test_GetTransaction_NotFound_ReturnsNull()
    {
        var repositoryMock = new Mock<ITransactionRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Transaction?)null);

        var treasuryApiMock = new Mock<ITreasuryApiClient>();
        var service = new TransactionService(repositoryMock.Object, treasuryApiMock.Object);

        var result = await service.GetTransaction(Guid.NewGuid());

        Assert.Null(result);
    }
}
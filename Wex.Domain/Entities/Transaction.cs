namespace Wex.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime TransactionDate { get; private set; }
    public decimal AmountUsd { get; private set; }

    public Transaction(string description, DateTime date, decimal amountUsd)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > 50)
            throw new ArgumentException("Description must be 1-50 chars");

        if (amountUsd <= 0)
            throw new ArgumentException("Amount must be positive");

        Id = Guid.NewGuid();
        Description = description;
        TransactionDate = date;
        AmountUsd = Math.Round(amountUsd, 2);
    }

    private Transaction() { }
}

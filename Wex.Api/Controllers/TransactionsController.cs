using Microsoft.AspNetCore.Mvc;
using Wex.Application.Services;
using Wex.Application.Dtos;

namespace Wex.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _service;

    public TransactionsController(TransactionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
    {
        var id = await _service.CreateTransaction(request.Description, request.TransactionDate, request.AmountUsd);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> Get(Guid id, [FromQuery] string? country = null)
    {
        var transaction = await _service.GetTransaction(id, country);
        if (transaction == null) return NotFound();
        return Ok(transaction);
    }
}

public record CreateTransactionRequest(string Description, DateTime TransactionDate, decimal AmountUsd);

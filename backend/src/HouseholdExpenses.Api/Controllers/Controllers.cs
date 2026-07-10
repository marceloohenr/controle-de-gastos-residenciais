using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenses.Api.Controllers;

/// <summary>Expõe os casos de uso de criação, consulta e exclusão de pessoas.</summary>
[ApiController, Route("api/people")]
public sealed class PeopleController(IPersonService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PersonResponse>>> Get(
        [FromQuery] string? search,
        CancellationToken ct)
    {
        return Ok(await service.ListAsync(search, ct));
    }

    [HttpPost]
    public async Task<ActionResult<PersonResponse>> Post(
        CreatePersonRequest request,
        CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return Created($"/api/people/{result.Id}", result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}

/// <summary>Expõe o cadastro e a consulta das movimentações financeiras.</summary>
[ApiController, Route("api/transactions")]
public sealed class TransactionsController(ITransactionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionResponse>>> Get(
        [FromQuery] int? personId,
        [FromQuery] TransactionType? type,
        CancellationToken ct)
    {
        return Ok(await service.ListAsync(personId, type, ct));
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse>> Post(
        CreateTransactionRequest request,
        CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return Created($"/api/transactions/{result.Id}", result);
    }
}

/// <summary>Disponibiliza os totais financeiros individuais e consolidados.</summary>
[ApiController, Route("api/summary")]
public sealed class SummaryController(ISummaryService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SummaryResponse>> Get(CancellationToken ct)
    {
        return Ok(await service.GetAsync(ct));
    }
}

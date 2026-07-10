using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenses.Api.Controllers;

[ApiController, Route("api/people")]
public sealed class PeopleController(IPersonService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IReadOnlyList<PersonResponse>>> Get([FromQuery] string? search, CancellationToken ct) => Ok(await service.ListAsync(search, ct));
    [HttpPost] public async Task<ActionResult<PersonResponse>> Post(CreatePersonRequest request, CancellationToken ct) { var result = await service.CreateAsync(request, ct); return Created($"/api/people/{result.Id}", result); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id, CancellationToken ct) { await service.DeleteAsync(id, ct); return NoContent(); }
}

[ApiController, Route("api/transactions")]
public sealed class TransactionsController(ITransactionService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<IReadOnlyList<TransactionResponse>>> Get([FromQuery] int? personId, [FromQuery] TransactionType? type, CancellationToken ct) => Ok(await service.ListAsync(personId, type, ct));
    [HttpPost] public async Task<ActionResult<TransactionResponse>> Post(CreateTransactionRequest request, CancellationToken ct) { var result = await service.CreateAsync(request, ct); return Created($"/api/transactions/{result.Id}", result); }
}

[ApiController, Route("api/summary")]
public sealed class SummaryController(ISummaryService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<SummaryResponse>> Get(CancellationToken ct) => Ok(await service.GetAsync(ct));
}

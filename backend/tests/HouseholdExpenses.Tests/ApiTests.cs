using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace HouseholdExpenses.Tests;

public sealed class ApiTests : IClassFixture<ApiFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
    private readonly HttpClient client;

    public ApiTests(ApiFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task People_endpoints_validate_create_list_and_delete()
    {
        var missingAge = await client.PostAsJsonAsync("/api/people", new { name = "Sem idade" });
        Assert.Equal(HttpStatusCode.BadRequest, missingAge.StatusCode);

        var created = await CreatePersonAsync("Helena", 32);
        var people = await client.GetFromJsonAsync<List<PersonResponse>>("/api/people");
        Assert.Contains(people!, person => person.Id == created.Id && person.Name == "Helena");

        var deleted = await client.DeleteAsync($"/api/people/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);

        var missing = await client.DeleteAsync($"/api/people/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task Transaction_endpoints_enforce_business_rules_and_cascade()
    {
        var adult = await CreatePersonAsync("Rafael", 28);
        var minor = await CreatePersonAsync("Clara", 16);

        var income = await client.PostAsJsonAsync("/api/transactions", new
        {
            description = "Salário",
            amount = 2500.75m,
            type = TransactionType.Income,
            personId = adult.Id
        });
        Assert.Equal(HttpStatusCode.Created, income.StatusCode);

        var minorExpense = await client.PostAsJsonAsync("/api/transactions", new
        {
            description = "Material escolar",
            amount = 120.50m,
            type = TransactionType.Expense,
            personId = minor.Id
        });
        Assert.Equal(HttpStatusCode.Created, minorExpense.StatusCode);

        var minorIncome = await client.PostAsJsonAsync("/api/transactions", new
        {
            description = "Receita bloqueada",
            amount = 20m,
            type = TransactionType.Income,
            personId = minor.Id
        });
        Assert.Equal(HttpStatusCode.Conflict, minorIncome.StatusCode);

        var unknownPerson = await client.PostAsJsonAsync("/api/transactions", new
        {
            description = "Pessoa ausente",
            amount = 20m,
            type = TransactionType.Expense,
            personId = 999999
        });
        Assert.Equal(HttpStatusCode.NotFound, unknownPerson.StatusCode);

        var deleted = await client.DeleteAsync($"/api/people/{minor.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);
        var transactions = await client.GetFromJsonAsync<List<TransactionResponse>>("/api/transactions", JsonOptions);
        Assert.DoesNotContain(transactions!, transaction => transaction.PersonId == minor.Id);
    }

    [Fact]
    public async Task Summary_returns_individual_and_general_totals()
    {
        var person = await CreatePersonAsync("Resumo HTTP", 40);
        var income = await client.PostAsJsonAsync("/api/transactions", new { description = "Receita", amount = 1000.50m, type = TransactionType.Income, personId = person.Id });
        var expense = await client.PostAsJsonAsync("/api/transactions", new { description = "Despesa", amount = 300.25m, type = TransactionType.Expense, personId = person.Id });
        Assert.Equal(HttpStatusCode.Created, income.StatusCode);
        Assert.Equal(HttpStatusCode.Created, expense.StatusCode);

        var response = await client.GetAsync("/api/summary");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var summary = await response.Content.ReadFromJsonAsync<SummaryResponse>();
        var personSummary = Assert.Single(summary!.People, item => item.PersonId == person.Id);
        Assert.Equal(700.25m, personSummary.Balance);
        Assert.Equal(summary.People.Sum(item => item.TotalIncome), summary.Totals.TotalIncome);
        Assert.Equal(summary.People.Sum(item => item.TotalExpenses), summary.Totals.TotalExpenses);
    }

    private async Task<PersonResponse> CreatePersonAsync(string name, int age)
    {
        var response = await client.PostAsJsonAsync("/api/people", new { name, age });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<PersonResponse>())!;
    }
}

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string databasePath = Path.Combine(Path.GetTempPath(), $"household-expenses-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
            new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = $"Data Source={databasePath}" }));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        SqliteConnection.ClearAllPools();
        if (File.Exists(databasePath)) File.Delete(databasePath);
    }
}

using System.Text.Json.Serialization;
using HouseholdExpenses.Api.Interfaces;
using HouseholdExpenses.Api.Persistence;
using HouseholdExpenses.Api.Repositories;
using HouseholdExpenses.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IPersonRepository, PersonRepository>(); services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IPersonService, PersonService>(); services.AddScoped<ITransactionService, TransactionService>(); services.AddScoped<ISummaryService, SummaryService>();
        services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer(); services.AddSwaggerGen();
        services.AddCors(options => options.AddPolicy("Frontend", policy => policy.WithOrigins(configuration["FrontendUrl"] ?? "http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));
        return services;
    }
}

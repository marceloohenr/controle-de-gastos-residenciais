using HouseholdExpenses.Api.Exceptions;
using HouseholdExpenses.Api.Extensions;
using HouseholdExpenses.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication(builder.Configuration);
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseCors("Frontend"); app.MapControllers();
using (var scope = app.Services.CreateScope()) await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreatedAsync();
app.Run();
public partial class Program;

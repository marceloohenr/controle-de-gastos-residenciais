using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenses.Api.Exceptions;

/// <summary>
/// Converte exceções conhecidas em respostas Problem Details e evita expor detalhes internos ao cliente.
/// </summary>
public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception exception)
        {
            var status = exception switch { NotFoundException => 404, BusinessRuleException => 409, _ => 500 };
            if (status == 500) logger.LogError(exception, "Erro não tratado ao processar a requisição");
            var detail = status == 500 ? "Não foi possível concluir a operação." : exception.Message;
            context.Response.StatusCode = status; context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(new ProblemDetails { Status = status, Title = status == 409 ? "Regra de negócio violada" : status == 404 ? "Recurso não encontrado" : "Erro interno", Detail = detail });
        }
    }
}

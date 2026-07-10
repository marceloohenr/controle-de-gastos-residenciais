namespace HouseholdExpenses.Api.Exceptions;

/// <summary>Indica que um recurso necessário ao caso de uso não foi encontrado.</summary>
public sealed class NotFoundException(string message) : Exception(message);

/// <summary>Indica que uma operação válida sintaticamente viola uma regra do domínio.</summary>
public sealed class BusinessRuleException(string message) : Exception(message);

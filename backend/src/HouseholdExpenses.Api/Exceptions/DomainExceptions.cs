namespace HouseholdExpenses.Api.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);
public sealed class BusinessRuleException(string message) : Exception(message);

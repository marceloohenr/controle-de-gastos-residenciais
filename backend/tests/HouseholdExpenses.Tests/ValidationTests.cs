using System.ComponentModel.DataAnnotations;
using HouseholdExpenses.Api.DTOs;

namespace HouseholdExpenses.Tests;

public sealed class ValidationTests
{
    [Fact]
    public void Age_is_required()
    {
        var request = new CreatePersonRequest("Júlia", null);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);

        Assert.False(valid);
        Assert.Contains(results, result => result.MemberNames.Contains(nameof(CreatePersonRequest.Age)));
    }
}

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BankRUs.WebApi.Attributes;

public class MaxDecimalsAttribute(int numberOfDecimals) : ValidationAttribute
{
    private readonly int _numberOfDecimals = numberOfDecimals;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) {
            return Error();
        }

        if (value is not decimal number) {
            return Error();
        }
        try {
            string[] numbers = number.ToString(CultureInfo.InvariantCulture).Split('.');
            if (numbers[1].Length > _numberOfDecimals) {
                return Error();
            }
        // If there's no decimals, numbers[1] will throw an exception.
        // That also means that the number of decimals == 0 which is valid,
        // therefore just carry on
        } catch (IndexOutOfRangeException) { }

        return ValidationResult.Success;
    }

    private static ValidationResult Error() =>
        new ValidationResult("Invalid amount");
}

namespace CombineSample.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
public class MyCustomValidation{
    public class TwoDigitAgeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dateOfBirth = (DateTime)value;
        var age = DateTime.Today.Year - dateOfBirth.Year;
        
        if (dateOfBirth > DateTime.Today.AddYears(-age))
        {
            age--;
        }
        
        if (age < 20 || age > 99)
        {
            return new ValidationResult("Please enter a valid age.");
        }
        
        return ValidationResult.Success;
    }
}


public class MinimumAgeAttribute : ValidationAttribute
{
    private readonly int _minimumAge;

    public MinimumAgeAttribute(int minimumAge)
    {
        _minimumAge = minimumAge;
    }

    
}
public class JoiningDateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = (AddEmployee)validationContext.ObjectInstance;
        
        // Perform the validation
        if (model.employmentStartDate < model.employeeDOB)
        {
            return new ValidationResult("employment start date cannot be earlier than the date of birth.");
        }

        return ValidationResult.Success;
    }
}
public class DateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var inputDate = (DateTime?)value;

        if (inputDate.HasValue)
        {
            var referenceDate = DateTime.Now.AddYears(-21);

            if (inputDate <= referenceDate)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage);
    }
}
public string ? verifycode { get; set; }
    
    public bool EmailAddress(string email){
        Regex regex = new Regex(@"^([a-z\d\-]+)@([a-z\d-]+)\.([a-z]{2,8})(\.[a-z]{2,8})?$");
        return regex.IsMatch(email);
    }
}
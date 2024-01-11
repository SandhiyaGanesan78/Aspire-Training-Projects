namespace CombineSample.Models;
using System.ComponentModel.DataAnnotations;


public class Manager{
    public int managerId { get; set; }

    [StringLength(100,MinimumLength =3)]
    [Display( Prompt ="enter your name")]
    public string? managerName { get; set; }

    [EmailAddress (ErrorMessage ="Please enter a valid email address")]
    [RegularExpression(@"^([a-z\d\-]+)@([a-z\d-]+)\.([a-z]{2,8})(\.[a-z]{2,8})?$", ErrorMessage = "Please enter a valid email")]
    [Display(Prompt ="enter your email address")]
    public string? managerEmail { get; set; }

    [RegularExpression(@"^[1-9]\d{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
    [Display(Prompt ="enter your mobile number")]
    public string? managerNumber { get; set; }

    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    [Display(Prompt ="enter your password ")]
    public string? managerPassword { get; set; } 
    public byte[]? managerImage{get; set;}
}
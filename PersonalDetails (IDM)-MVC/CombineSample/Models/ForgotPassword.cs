using System.ComponentModel.DataAnnotations;

namespace CombineSample.Models;

public class ForgotPassword
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}

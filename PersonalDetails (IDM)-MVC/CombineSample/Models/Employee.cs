namespace CombineSample.Models;
using System.ComponentModel.DataAnnotations;

public class Employee{
    public int employeeId { get; set; }

    [StringLength(100,MinimumLength =3)]
    [Display( Prompt ="enter employee name")]
    public string? employeeName { get; set; }

    [EmailAddress (ErrorMessage ="Please enter a valid email address")]
    [RegularExpression(@"^([a-z\d\-]+)@([a-z\d-]+)\.([a-z]{2,8})(\.[a-z]{2,8})?$", ErrorMessage = "Please enter a valid email")]
    [Display( Prompt ="enter employee email")]
    public string? employeeEmail { get; set; }

    [RegularExpression(@"^[7-9]\d{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
    [Display(Prompt ="enter employee mobile number")]
    public string? employeeMobile { get; set; }

    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    [Display( Prompt ="enter your password ")]
    public string? employeePassword { get; set; }
    
    
    [Display( Prompt ="set employement start date ")]
    [DataType(DataType.Date)]
    public DateTime? employmentStartDate{ get; set;}
    
    [Display( Prompt ="set employee leave start date ")]
    [DataType(DataType.Date)]
    public string? employeeLeaveStartDate{ get; set;}
    
    [Display( Prompt ="set employee leave end date ")]
    [DataType(DataType.Date)]
    public string? employeeLeaveEndDate{ get; set;}
    
    [Display( Prompt ="enter employee working status")]
    public string? employmentStatus { get; set; }

    [Display( Prompt ="enter employee location ")]
    public string? employeeLocation{ get; set;}
    
    [Display( Prompt ="set employee DoB")]
    [DataType(DataType.Date)]
    public DateTime? employeeDOB{get;set;}
    
    [RegularExpression(@"^2[1-9]|[3-5]\d|60$", ErrorMessage = "Please enter a valid age between 21 and 60.")]
    [Display( Prompt ="enter employee Age")]
    public string? employeeAge{get;set;}
    
    
    [Display( Prompt ="enter employee marital status")]
    public string? EmployeeMartialStatus{get;set;}
    public string? PasswordResetToken{get;set;}
    public DateTime PasswordResetTokenExpiry { get; set; }
    public DateTime? PasswordResetTokenExpiration { get; set; }

    
    public string? employee_vacation_req{ get; set;}

    [Display( Prompt ="select your leave reason ")]
    public string? leaveReason{ get; set;}

    
    public byte[]? employeeImage{get; set;}

    public List<string> myList = new List<string>();
}
namespace CombineSample.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using static CombineSample.Models.MyCustomValidation;
public class AddEmployee
{
    public int employeeId { get; set; }

    [StringLength(100,MinimumLength =3)]
    [Required (ErrorMessage ="Please enter employee name")]
    [Display( Prompt ="enter employee name")]
    public string? employeeName { get; set; }

    [Required (ErrorMessage ="Please enter valid email address")]
    [EmailAddress]
    [Display(Prompt ="enter employee Email address")]
    public string? employeeEmail { get; set; }

    [Required (ErrorMessage ="Please enter employee employment status")]
    [Display( Prompt ="enter employee working status")]
    public string? employmentStatus { get; set; }

    [Required(ErrorMessage ="Please enter employee contact number")]
    [RegularExpression(@"^[7-9]\d{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
    [Display( Prompt ="enter employee mobile number")]
    public string? employeeMobile{ get; set;}
    
    [Required (ErrorMessage ="Please set employee date of birth")]
    [Display( Prompt ="set employee DoB")]
    [DateValidation(ErrorMessage = "employee age must be 21 years and above")]
    [DataType(DataType.Date)]
    public DateTime? employeeDOB{get;set;}

    [Required (ErrorMessage ="Please set employee start working date")]
    [Display( Prompt ="set employement start date ")]
    [DataType(DataType.Date)]
    [JoiningDateValidation ]
    public DateTime? employmentStartDate{ get; set;}

    [Required (ErrorMessage ="Please enter employee currently working location")]
    [Display( Prompt ="enter employee currently working location")]
    public string? employeeLocation{ get; set;}

    [Required(ErrorMessage ="Please enter employee age")]
    [DataType(DataType.Date)]
    [RegularExpression(@"^2[1-9]|[3-5]\d|60$", ErrorMessage = "Please enter a valid age between 21 and 60.")]
    [Display(Prompt ="enter employee Age")]
    public string? employeeAge{get;set;}

    [Required(ErrorMessage ="Please enter employee marital status")]
    [Display( Prompt ="enter employee marital status")]
    public string? EmployeeMartialStatus{get;set;}
    public List<string> myList = new List<string>();
}
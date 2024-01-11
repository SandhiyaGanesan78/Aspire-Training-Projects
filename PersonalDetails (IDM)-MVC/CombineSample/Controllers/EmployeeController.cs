using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CombineSample.Models;
using CombineSample.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
namespace CombineSample.Controllers;
[TypeFilter(typeof(LoggingActionFilter))] 
//  action filter at the controller level
public class EmployeeController:Controller{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ILogger<HomeController> _logger;
    ApplicationDbContext dbContext1=new ApplicationDbContext();
    public EmployeeController(ApplicationDbContext dbContext,IEmailSender emailSender,UserManager<ApplicationUser> userManager,IWebHostEnvironment hostEnvironment, ILogger<HomeController> logger)
    {
        _dbContext = dbContext;
        _hostEnvironment = hostEnvironment;
        _emailSender = emailSender;
        _userManager = userManager;
        _logger = logger;
        
    } 
    [HttpGet]
    public IActionResult EmployeeRegister()
    {
        return View();
    }
    
    [HttpPost]
    
        public async Task<IActionResult> EmployeeRegister(Employee employee)
    {
        if(ModelState.IsValid){
           try
    {
        int  emailCount = 0;
        int mobileCount = 0;
        
        if (_dbContext.Employee_Details != null)
        {
            emailCount = _dbContext.Employee_Details.Count(emp => emp.employeeEmail == employee.employeeEmail);
            mobileCount = _dbContext.Employee_Details.Count(emp => emp.employeeMobile == employee.employeeMobile);
            
            if (employee.employeeEmail != null && employee.employeeMobile != null && employee.employeeDOB != null && employee.employeePassword != null)
            {
                if (emailCount > 0)
                {
                    throw new EmployeeAlreadyExistsException("Employee with the same email already exists.");
                }
                else if (mobileCount > 0)
                {
                    throw new EmployeeAlreadyExistsException("Employee with the same mobile number already exists.");
                }
                else
                {
                    employee.employmentStatus = "Trainee";
                    employee.employeeLeaveStartDate = "Not Assigned";
                    employee.employeeLeaveEndDate = "Not Assigned";
                    employee.employee_vacation_req = "Not Assigned";
                    employee.employeeImage = null;
                    ViewData["Message"] = "You are successfully registered with us.";
                    _dbContext.Add(employee);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
    catch (EmployeeAlreadyExistsException ex)
    {
        ViewData["Message"] = "Error: " + ex.Message;
    }
}
ModelState.AddModelError(string.Empty, "Try Again");
return View(employee);
    }
    [HttpGet]
    public IActionResult EmployeeLogin(Employee employee)
    {
        var cookieValue = Request.Cookies["MyEAuthCookie"];
        if (cookieValue != null)
        {
            var decodedCookie = cookieValue.Split(":");
            employee.employeeEmail = decodedCookie[0];
            employee.employeePassword = decodedCookie[1];
            ViewBag.email=employee.employeeEmail;
            ViewBag.password=employee.employeePassword;
        }
        return View();
    }
    [HttpPost]
    public IActionResult EmployeeLogin(Employee employee,MyCustomValidation validation)
    {
        if(ModelState.IsValid){
            int count =0;
            string? password=" ";
            try{
                
                if(_dbContext.Employee_Details!=null){
                count = _dbContext.Employee_Details
                .Where(emp => emp.employeeEmail == employee.employeeEmail)
                .Count();
                password = _dbContext.Employee_Details
                .Where(emp => emp.employeeEmail == employee.employeeEmail)
                .Select(emp => emp.employeePassword)
                .FirstOrDefault();
                if(employee.employeeEmail!=null && employee.employeePassword!=null){
                if(count>0 ){
                    if(password==employee.employeePassword )
                    {
                            HttpContext.Session.SetObjectAsJson("users", employee);
                            
                            var cookieValue = $"{employee.employeeEmail}:{employee.employeePassword}";
                            var cookieOptions = new CookieOptions
                            {
                                Expires = DateTimeOffset.Now.AddDays(30),
                                HttpOnly = true
                            };
                            Response.Cookies.Append("MyEAuthCookie", cookieValue, cookieOptions);
                            int employeeId;
                            if(_dbContext.Employee_Details!=null){
                            employeeId = _dbContext.Employee_Details.Where(emp => emp.employeeEmail == employee.employeeEmail).Select(emp=> emp.employeeId).FirstOrDefault();
                            TempData["EmpId"]=Convert.ToString(employeeId);
                            var url = Url.Action("EmployeeView","Employee",1);
                            if (url != null){
                                return Redirect(url);
                            }
                            }
                    }
                    else{
                        ViewData["Message"]="incorrect Password";
                    }
                }
                else{
                    ViewData["Message"]="you are not Registered with us.To register click below link";
                }
                }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"{exception.Message}"); 
            }
        }
        return View(employee);
    }
    [HttpGet]
    // custom Iresult Filter
    [TypeFilter(typeof(ActionFilter))]
    [AuthorizeAttributeEmployee]
    public async Task<IActionResult> EmployeeView(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.employeeId=Convert.ToInt16(myData);

        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(emp => emp.employeeId == employee.employeeId ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    public IActionResult GetImage(int id)
    {
        var employee = _dbContext.Employee_Details.FirstOrDefault(emp => emp.employeeId == id);
        if (employee != null && employee.employeeImage != null)
        {
            return File(employee.employeeImage, "~/Images/carouselImages/Aspire.jpg"); 
        }
        return NotFound();
    }
    [HttpGet]
    [AuthorizeAttributeEmployee]
    public async Task<IActionResult> EditEmployee(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.employeeId=Convert.ToInt16(myData);
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(emp => emp.employeeId == employee.employeeId ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpPost]
    [AuthorizeAttributeEmployee]
public async Task<IActionResult> EditEmployee(Employee employee, IFormFile image1)
{
    string myData = TempData["EmpId"] as string;
    TempData.Keep("EmpId");
    employee.employeeId = Convert.ToInt16(myData);

    if (ModelState.IsValid)
    {
        try
        {
            if (_dbContext.Employee_Details != null)
            {
                 using (_dbContext) // Applying  resource management 
                {
                    var employees = await _dbContext.Employee_Details.FindAsync(employee.employeeId);
                    if (employees != null)
                    {
                        employees.employeeName = employee.employeeName;
                        employees.employeeEmail = employee.employeeEmail;
                        employees.employeeDOB = employee.employeeDOB;
                        employee.employeeMobile = employee.employeeMobile;
                        employee.employeeAge = "Not Assigned";
                        employee.employmentStatus = "Trainee";
                        employee.employmentStartDate = null;
                        employee.employeeLocation = "Not Assigned";
                        employee.EmployeeMartialStatus = "Married";

                        if (image1 != null && image1.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await image1.CopyToAsync(memoryStream);
                                employees.employeeImage = memoryStream.ToArray();
                            }
                        }

                        await _dbContext.SaveChangesAsync();

                        var url = Url.Action("EmployeeView", "Employee", new { id = employee.employeeId });
                        if (url != null)
                        {
                            return Redirect(url);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Employee not found");
            }
        }
        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, $"Try Again {exception.Message}");
        }
    }
    return View(employee);
}
    [HttpGet]
    [AuthorizeAttributeEmployee]
    public async Task<IActionResult> LeaveRequest(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.employeeId=Convert.ToInt16(myData);
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(emp => emp.employeeId == employee.employeeId ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpPost]
    [AuthorizeAttributeEmployee]
public async Task<IActionResult> LeaveRequest(Employee employee,int a)
{
        string myData = TempData["EmpId"] as string;
        TempData.Keep("EmpId");
        employee.employeeId = Convert.ToInt16(myData);

        if (ModelState.IsValid)
        {
            try{
                var employees = await _dbContext.Employee_Details.FindAsync(employee.employeeId);
                if (employees != null)
                {
                    employees.employee_vacation_req = "Assigned";
                    employees.employeeLeaveStartDate = employee.employeeLeaveStartDate;
                    employees.employeeLeaveEndDate = employee.employeeLeaveEndDate;
                    employees.leaveReason = employee.leaveReason;
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("EmployeeView", "Employee", new { id = employee.employeeId });
                }
                ModelState.AddModelError(string.Empty, "Employee not found");
            }
            catch (NullReferenceException ex)
            {
                ModelState.AddModelError(string.Empty, "NUll Reference exception occured");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid operation exception occured");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Database Error: " + ex.Message);
                ModelState.AddModelError(string.Empty, "error occured  while accessing database ");
            }
            catch (FormatException ex)
            {
                ModelState.AddModelError(string.Empty, "format exception occured");
            }
            catch (IOException ex)
            {
                ModelState.AddModelError(string.Empty, "IO exception occured");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
        }
    return View(employee);
}
    [AuthorizeAttributeEmployee]
    public ActionResult Logout()
    {
        return RedirectToAction("EmployeeLogin");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpGet]
    public IActionResult VerifyEmail()
    {
        return View();
    }
    [HttpPost]
    public IActionResult VerifyEmail(MyCustomValidation validation,Employee employee)
    {
        string ? myData = TempData["verificationcode"] as String;
        string ? email = TempData["email"] as String;
        if(myData==validation.verifycode){
            employee.employeeEmail=email;
            HttpContext.Session.SetObjectAsJson("users", employee);
           return RedirectToAction("EmployeeView", "Employee");
        }
        else{
            ViewBag.message="OTP is Invalid";
        }
        return View(employee);
    }
    [HttpPost]
    public IActionResult ForgotPassword(Employee employee,MyCustomValidation validation)
    {
        if(ModelState.IsValid){
            try{
                int count =0;
                if(_dbContext.Employee_Details!=null){
                count = _dbContext.Employee_Details.Where(emp => emp.employeeEmail == employee.employeeEmail).Count();
                if(employee.employeeEmail!=null)
                if(count>0 && validation.EmailAddress(employee.employeeEmail)==true){
                        try
                        {
                            Random random = new Random();
                            string code = random.Next(100000, 999999).ToString();
                            TempData["verificationcode"] = code;
                            TempData["email"] = employee.employeeEmail;
                            string from, pass, messageBody;
                            MailMessage message = new MailMessage();
                            from = "sandhiyaganesan76@gmail.com";
                            pass = "xqjefzgxjgqxkgqn";
                            messageBody = "Your Verification Code is " + code;
                            if(employee.employeeEmail != null)
                            {
                                message.To.Add(new MailAddress(employee.employeeEmail));
                            }
                            else
                            {
                                throw new Exception("invalid.");
                            }
                            message.From = new MailAddress(from);
                            message.Body = messageBody;
                            message.Subject = " Enter the OTP";
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                            smtp.EnableSsl = true;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.EnableSsl = true;
                            smtp.Credentials = new NetworkCredential(from, pass);                  
                            smtp.Send(message);
                            ViewData["Message"] = "Verification Code sent to your email Id";
                            return RedirectToAction("VerifyEmail", "Employee");
                        }
                        catch (Exception ex)
                        {
                            // Handle any errors that occur while sending the email
                            ViewData["Message"] = "Error while sending verification code: " + ex.Message;
                        }
                }
                else{
                    ViewData["Message"]="invalid email";
                }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try Again{exception.Message}"); 
            }
        }
        return View(employee);
    }
    [HttpGet]
public IActionResult ForgotPasswordEmployee()
{
    return View();
}
[HttpPost]
public async Task<IActionResult> ForgotPasswordEmployee(ForgotPassword model)
{
    if (ModelState.IsValid)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return View("ForgotPasswordConfirmation");
            Console.WriteLine("Sented");
        }
        // Generate password reset token
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Employee");
        var passwordResetUrl = Url.Action("ResetPassword", "Employee", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

        await _emailSender.SendEmailAsync(
        model.Email,
        "Reset Password",
        $"Please reset your password by clicking here: <a href='{passwordResetUrl}'>link</a>.");

            return View("ForgotPasswordConfirmation");
    return View();
}
return View(model);
}
[HttpGet]
public IActionResult ResetPassword(string? code = null)
{
if (code == null)
{
throw new ApplicationException("A code must be supplied for password reset.");
}
var model = new ResetPassword { Code = code };
return View(model);
}

[HttpPost]


public async Task<IActionResult> ResetPassword(ResetPassword model)
{
if (!ModelState.IsValid)
{
return View(model);
}
var user = await _userManager.FindByEmailAsync(model.Email);
if (user == null)
{
    return RedirectToAction("ResetPasswordConfirmation", "Employee");
}

var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
if (result.Succeeded)
{
    return RedirectToAction("ResetPasswordConfirmation", "Employee");
}

foreach (var error in result.Errors)
{
    ModelState.AddModelError(string.Empty, error.Description);
}

return View(model);
}
[HttpGet]
public IActionResult ResetPasswordConfirmation()
{
return View();
}
}
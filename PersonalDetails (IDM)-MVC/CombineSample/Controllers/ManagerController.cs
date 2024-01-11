using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CombineSample.Models;
using CombineSample.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
namespace CombineSample.Controllers;
[TypeFilter(typeof(LoggingActionFilter))] //  action filter at the controller level
    public class ManagerController:Controller{
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;
    public ManagerController(ApplicationDbContext dbContext, ILogger<HomeController> logger)
    {
        _dbContext = dbContext;
        _logger=logger;
    }
    [HttpGet]
    public IActionResult ManagerRegister()
    {
        return View();
    }
    [HttpPost]
    
    public async Task<IActionResult> ManagerRegister(Manager manager)
    {
        if(ModelState.IsValid){
            try{
                int count = 0;
                
                if(_dbContext.Manager_Details!=null){
                count = _dbContext.Manager_Details.Where(emp => emp.managerEmail == manager.managerEmail).Count();
                if(manager.managerEmail!=null  &&  manager.managerPassword!=null){
                    if(count>0){
                        ViewData["Message"]="this user already exists";
                    }
                    else{
                        _dbContext.Add(manager);
                        await _dbContext.SaveChangesAsync();
                        ViewData["Message"]="your are registered with us";
                    }
                }
                } 
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"try again"); 
        return View(manager);
    }
    [HttpGet]
    public IActionResult ManagerLogin(Manager manager)
    {
        var cookieValue = Request.Cookies["MyMAuthCookie"];
        if (cookieValue != null)
        {
            var decodedCookie = cookieValue.Split(":");
            manager.managerEmail = decodedCookie[0];
            manager.managerPassword = decodedCookie[1];
            ViewBag.email=manager.managerEmail;
            ViewBag.password=manager.managerPassword;
        }
        return View();
    }
    
    [HttpPost]
  
    public IActionResult ManagerLogin(Manager manager,MyCustomValidation validation)
    {
        if(ModelState.IsValid){
            try{
                int count =0;
                string? dbpassword=" ";
                if(_dbContext.Manager_Details!=null){
                count = _dbContext.Manager_Details.Where(emp => emp.managerEmail == manager.managerEmail).Count();
                dbpassword = _dbContext.Manager_Details.Where(emp => emp.managerEmail == manager.managerEmail).Select(emp => emp.managerPassword).FirstOrDefault();
                if(manager.managerEmail!=null && manager.managerPassword!=null)
                if(count>0 ){
                        if(dbpassword==manager.managerPassword)
                        {
                            TempData["email"]=manager.managerEmail;
                            HttpContext.Session.SetObjectAsJson("users", manager);
                            //Cookies 
                            var cookieValue = $"{manager.managerEmail}:{manager.managerPassword}";
                            var cookieOptions = new CookieOptions
                            {
                                Expires = DateTimeOffset.Now.AddDays(30),
                                HttpOnly = true
                            };
                            Response.Cookies.Append("MyMAuthCookie", cookieValue, cookieOptions);
                            return RedirectToAction("EmployeeList", "Manager");
                        }
                        else{
                            ViewData["Message"]="incorrect password";
                        }
                }
                else{
                    ViewData["Message"]="you are not registered with us";
                }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        return View(manager);
    }
    [HttpGet]
    public async Task<IActionResult> EmployeeList()
    {
        if(_dbContext.AddEmployee_Details!=null){
        var employees = await _dbContext.AddEmployee_Details.ToListAsync();
        return View(employees);
        }
        return View();
    }
     [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> NewEmployeeRegisterList()
    {
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.ToListAsync();
        return View(employees);
        }
        return View();
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> NewManageEmployee(int id)
    { 
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(emp => emp.employeeId == id ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> NewUpdateEmployee(int id,Employee employee)
    {
       
        ViewBag.list = employee.myList;
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(emp => emp.employeeId == id ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View(employee);
    }
    [HttpPost]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> NewUpdateEmployee(Employee employee)
    {
        if(ModelState.IsValid){
            try{
                if(_dbContext.AddEmployee_Details!=null){
                var employees = await _dbContext.AddEmployee_Details.Where(emp => emp.employeeId == employee.employeeId ).FirstOrDefaultAsync();
                if(employees != null){
                    employees.employeeName= employee.employeeName;
                    employees.employeeEmail= employee.employeeEmail;
                    employees.employeeDOB=employee.employeeDOB;
                    employees.employeeMobile=employee.employeeMobile;
                    employees.employeeAge=employee.employeeAge;
                    employees.EmployeeMartialStatus=employee.EmployeeMartialStatus;
                    employees.employmentStatus=employee.employmentStatus;
                    employees.employmentStartDate=employee.employmentStartDate;
                    employees.employeeLocation=employee.employeeLocation;
                }
                await _dbContext.SaveChangesAsync();
                var url = Url.Action("NewManageEmployee", "Manager",new { id = employee.employeeId });
                if (url != null){
                    return Redirect(url);
                }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Try again");
        return View(employee);
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> RemoveEmployee(int id)
    {
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(emp=> emp.employeeId == id ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomExceptionFilter]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> RemoveEmployee(Employee employee)
    { 
        if(ModelState.IsValid){
            try{
                if(_dbContext.Employee_Details!=null){
                var employees = _dbContext.Employee_Details.FirstOrDefault(emp => emp.employeeId == employee.employeeId);
                if(employees != null){
                    _dbContext.Remove(employees);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("NewEmployeeRegisterList");
                }
                }
            }
            
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Try again"); 
        return View(employee);
    }
    [HttpGet]
    
    public IActionResult AddEmployee()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddEmployee(AddEmployee employee)
    {
        if(ModelState.IsValid){
            try{
                string myData = TempData["email"] as string;
                TempData.Keep(myData);
                int count = 0;
                if(_dbContext.AddEmployee_Details!=null){
                count = _dbContext.AddEmployee_Details.Count(emp => emp.employeeName == employee.employeeName);
                
                if(count>0){
                    ViewData["Message"]="username already exists";
                }
                else if(employee.employmentStartDate < employee.employeeDOB){
                    ModelState.AddModelError("JoiningDate", "Joining date cannot be earlier than the date of birth.");
                    return View(employee);
        
                }
                else{
                    
                    _dbContext.Add(employee);
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("EmployeeList", "Manager");
                    if (url != null){
                        return Redirect(url);
                    }
                }
                }
            }
            
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Try again");
        return View();
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> UpdateEmployee(int id,AddEmployee employee)
    {
       
        ViewBag.list = employee.myList;
        if(_dbContext.AddEmployee_Details!=null){
        var employees = await _dbContext.AddEmployee_Details.Where(emp => emp.employeeId == id ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpPost]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> UpdateEmployee(AddEmployee employee)
    {
        if(ModelState.IsValid){
            try{
                if(_dbContext.AddEmployee_Details!=null){
                var employees = await _dbContext.AddEmployee_Details.Where(emp => emp.employeeId == employee.employeeId ).FirstOrDefaultAsync();
                if(employees != null){
                    employees.employeeName= employee.employeeName;
                    employees.employeeEmail= employee.employeeEmail;
                    employees.employeeDOB=employee.employeeDOB;
                    employees.employeeMobile=employee.employeeMobile;
                    employees.employeeAge=employee.employeeAge;
                    employees.EmployeeMartialStatus=employee.EmployeeMartialStatus;
                    employees.employmentStatus=employee.employmentStatus;
                    employees.employmentStartDate=employee.employmentStartDate;
                    employees.employeeLocation=employee.employeeLocation;
                }
                await _dbContext.SaveChangesAsync();
                var url = Url.Action("ManageEmployee", "Manager",new { id = employee.employeeId });
                if (url != null){
                    return Redirect(url);
                }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Try again");
        return View(employee);
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        if(_dbContext.AddEmployee_Details!=null){
        var employees = await _dbContext.AddEmployee_Details.Where(emp => emp.employeeId == id ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpPost]
    //custom exception filter
    [CustomExceptionFilter]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> DeleteEmployee(AddEmployee employee)
    { 
        if(ModelState.IsValid){
            
                if(_dbContext.AddEmployee_Details!=null){
                var employees = _dbContext.AddEmployee_Details.FirstOrDefault(emp => emp.employeeId == employee.employeeId);
                if(employees != null){
                    _dbContext.Remove(employees);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("EmployeeList");
                }
                }
        }
        ModelState.AddModelError(String.Empty,$"Try again"); 
        return View(employee);
    }
    public IActionResult GetImage(int id)
    {
        var employee = _dbContext.Employee_Details.FirstOrDefault(emp => emp.employeeId == id);
        if (employee != null && employee.employeeImage != null)
        {
            return File(employee.employeeImage, "image/jpeg"); // adjust the content type to match your image type
        }
        return NotFound();
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> ManageEmployee(int id)
    { 
        string myData = TempData["email"] as string;
        TempData.Keep(myData);
        if(_dbContext.AddEmployee_Details!=null){
        var employees = await _dbContext.AddEmployee_Details.Where(emp => emp.employeeId == id ).FirstOrDefaultAsync();
        if(employees!=null){
        TempData["myData"] = employees.employeeName;
        return View(employees);
        }
        }
        return View();
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> LeaveList()
    {
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(e => e.employee_vacation_req == "Assigned" ||  e.employee_vacation_req == "Approved").ToListAsync();
        return View(employees);
        }
        return View();
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> ManageRequest(int id,Employee employee)
    {
        
        if(_dbContext.Employee_Details!=null){
        
        var employees = await _dbContext.Employee_Details.Where(x => x.employeeId == id ).FirstOrDefaultAsync();
        if(employees!=null){
        TempData["reqemail"]=employees.employeeEmail;}
        return View(employees);
        }
        return View();
    }
    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> ApproveLeave(int id,Employee employee)
    {
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(x => x.employeeId == id ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpPost]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> ApproveLeave(Employee employee)
    {
        if(ModelState.IsValid){
            try{
                string ? myData =TempData["reqemail"] as string; 
                if(myData!=null){
                            string temp_email = myData;
                            Console.WriteLine(myData);
                            string from, pass, messageBody;
                            MailMessage message = new MailMessage();
                            from = "sandhiyaganesan76@gmail.com";
                            pass = "xqjefzgxjgqxkgqn";
                            messageBody = "your leave has been approved by manager";
                            if(temp_email != null)
                            {
                                message.To.Add(new MailAddress(temp_email));
                            }
                            else
                            {
                                throw new Exception("invalid");
                            }
                            message.From = new MailAddress(from);
                            message.Body = messageBody;
                            message.Subject = "Leave Details ";
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                            smtp.EnableSsl = true;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.EnableSsl = true;
                            smtp.Credentials = new NetworkCredential(from, pass);                  
                            smtp.Send(message);
                }
                if(_dbContext.Employee_Details!=null){
                var employees = _dbContext.Employee_Details.FirstOrDefault(p => p.employeeId == employee.employeeId);
                if(employees != null){
                    employees.employee_vacation_req="Approved";
                }
                await _dbContext.SaveChangesAsync();
                }
                var url = Url.Action("LeaveList", "Manager",new { id = employee.employeeId });
                if (url != null){
                    return Redirect(url);
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Try again"); 
        return View(employee); 
    }

    [HttpGet]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> RejectLeave(int id,Employee employee)
    {
        if(_dbContext.Employee_Details!=null){
        var employees = await _dbContext.Employee_Details.Where(emp => emp.employeeId == id ).FirstOrDefaultAsync();
        return View(employees);
        }
        return View();
    }
    [HttpPost]
    [AuthorizeAttributeManager]
    public async Task<IActionResult> RejectLeave(Employee employee)
    {
        if(ModelState.IsValid){
            try{
                string ? myData =TempData["reqemail"] as string;
                if(myData!=null){
                            string temp_email = myData;
                            Console.WriteLine(myData);
                            string from, pass, messageBody;
                            MailMessage message = new MailMessage();
                            from = "sandhiyaganesan76@gmail.com";
                            pass = "xqjefzgxjgqxkgqn";
                            messageBody = "your leave has been rejected by manager";
                            if(temp_email != null)
                            {
                                message.To.Add(new MailAddress(temp_email));
                            }
                            else
                            {
                                throw new Exception(" invalid.");
                            }
                            message.From = new MailAddress(from);
                            message.Body = messageBody;
                            message.Subject = "Leave Details ";
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                            smtp.EnableSsl = true;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.EnableSsl = true;
                            smtp.Credentials = new NetworkCredential(from, pass);                  
                            smtp.Send(message);
                }
                if(_dbContext.Employee_Details!=null){
                var employees = _dbContext.Employee_Details.FirstOrDefault(emp => emp.employeeId == employee.employeeId);
                if(employees != null){
                    employees.employee_vacation_req="Not Assigned";
                    
                    employees.employeeLeaveStartDate="Not Assigned";
                    employees.employeeLeaveEndDate="Not Assigned";
                }
                await _dbContext.SaveChangesAsync();
                var url = Url.Action("LeaveList", "Manager",new { id = employee.employeeId });
                if (url != null){
                    return Redirect(url);
                }
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Try again{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Try again"); 
        return View(employee); 
    }
    [AuthorizeAttributeManager]
    public IActionResult Logout()
    {
        return RedirectToAction("ManagerLogin", "Manager");
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
    public IActionResult VerifyEmail(MyCustomValidation validation,Manager manager)
    {
        string ? myData = TempData["verificationcode"] as String;
        string ? email = TempData["email"] as String;
        if(myData==validation.verifycode){
            manager.managerEmail=email;
            HttpContext.Session.SetObjectAsJson("users", manager);
            
           return RedirectToAction("ManageEmployee", "Manager");
        }
        else{
            ViewBag.message="OTP is Invalid";
        }
        return View(manager);
    }
    [HttpPost]
    public IActionResult ForgotPassword(Manager manager,MyCustomValidation validation)
    {
        if(ModelState.IsValid){
            try{
                int count =0;
                if(_dbContext.Manager_Details!=null){
                count = _dbContext.Manager_Details.Where(emp => emp.managerEmail == manager.managerEmail).Count();
                if(manager.managerEmail!=null)
                if(count>0 && validation.EmailAddress(manager.managerEmail)==true){
                        try
                        {
                            Random random = new Random();
                            string code = random.Next(100000, 999999).ToString();
                            TempData["verificationcode"] = code;
                            TempData["email"] = manager.managerEmail;
                            string from, pass, messageBody;
                            MailMessage message = new MailMessage();
                            from = "sandhiyaganesan76@gmail.com";
                            pass = "xqjefzgxjgqxkgqn";
                            messageBody = "Your Verification Code is " + code;
                            if(manager.managerEmail != null)
                            {
                                message.To.Add(new MailAddress(manager.managerEmail));
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
                            return RedirectToAction("VerifyEmail", "Manager");
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
        return View(manager);
    }
}
    

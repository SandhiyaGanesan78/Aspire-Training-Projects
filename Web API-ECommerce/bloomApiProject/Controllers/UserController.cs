using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bloomApiProject.Data;
using bloomApiProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BloomProjectApi.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class UserController: Controller{
        private readonly bloomApiProjectDbContext _context;

public UserController(bloomApiProjectDbContext context)
{
    _context = context;
}
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Users userobj){
            if(userobj==null){
                 return BadRequest();
            }
           
            var obj =await _context.users.FirstOrDefaultAsync(x=>x.email==userobj.email );

            if(obj==null){
                return NotFound(new { Message="User not found"});

            }
            
            obj.token=CreateJwtToken(obj);
            return Ok(new{
                token=obj.token,
                Message="logged in Successfully"
            });
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] Users user){
            if(user ==null){
                return BadRequest();
            }
            if(await CheckUserNameExistAsync(user.name))
            {
                return BadRequest(new{Message="UserName already exist"});
            }

            if(await CheckEmailExistAsync(user.email))
            {
                return BadRequest(new{Message="Email already exist"});
            }
            user.role="User";
            user.token="";
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(new{
                Message="User Registered Successfully"
            });
        }
        private  Task<bool> CheckUserNameExistAsync(string name)
        =>_context.users.AnyAsync(x=>x.name== name);

        private  Task<bool> CheckEmailExistAsync(string email)
        =>_context.users.AnyAsync(x=>x.email== email);
        
        private string CreateJwtToken(Users users)
        {
            var jwtTokenHandler= new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes("veryverysceret....");
            var identity=new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.Role, users.role),
                new Claim(ClaimTypes.Email,$"{users.email}")
            });

            var credentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256 );
            var tokenDescriptor=new SecurityTokenDescriptor{
                Subject=identity,
                Expires= DateTime.Now.AddDays(10),
                SigningCredentials=credentials
            };
            var token=jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var products= await _context.users.ToListAsync();
            return Ok(products);
        }
        [HttpGet]
        [Route("data")]
        [HttpGet]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.email == email && u.password == password);
            if (user != null)
            {
                return Ok(new[] { user });
            }

            return NotFound();
        }
    }
}

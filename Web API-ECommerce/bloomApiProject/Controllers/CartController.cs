using bloomApiProject.Data;
using bloomApiProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly bloomApiProjectDbContext _context;

    public CartController(bloomApiProjectDbContext context)
    {
        _context = context;
    }

    [HttpGet("getcartitems")]
    public IActionResult GetCartItems()
    {
        var userId = GetCurrentUserId();
        var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();
        return Ok(cartItems);
    }

    [HttpPost("addtocart")]
    public IActionResult AddToCart(CartItem item)
    {
        var userId = GetCurrentUserId();
        item.UserId = userId;

        _context.CartItems.Add(item);
        _context.SaveChanges();

        return Ok(new { message = "Item added to cart successfully" });
    }

    [HttpDelete("removefromcart/{id}")]
    public IActionResult RemoveFromCart(int id)
    {
        var userId = GetCurrentUserId();
        var item = _context.CartItems.FirstOrDefault(c => c.Id == id && c.UserId == userId);

        if (item == null)
        {
            return NotFound();
        }

        _context.CartItems.Remove(item);
        _context.SaveChanges();

        return Ok(new { message = "Item removed from cart successfully" });
    }

    private int GetCurrentUserId()
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    if (token == null)
    {
        // Token is not present in the headers, handle this case as per your application logic
        // For simplicity, we'll return a default user ID
        return 1;
    }

    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryverysceret....")), // Replace with your secret key
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

    try
    {
        var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
        var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            // User ID extracted successfully from the token
            return userId;
        }
        else
        {
            // Failed to extract user ID from the token, handle this case as per your application logic
            // For simplicity, we'll return a default user ID
            return 1;
        }
    }
    catch (Exception ex)
    {
        // Token validation failed, handle this case as per your application logic
        // For simplicity, we'll return a default user ID
        return 1;
    }
}
    [HttpPut("updatecartitems")]
    public IActionResult UpdateCartItems(List<CartItem> cartItems)
    {
        var userId = GetCurrentUserId();
        var userCartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();

        // Handle duplicates and updates based on your server implementation
        foreach (var item in cartItems)
        {
            var existingItem = userCartItems.FirstOrDefault(c => c.Id == item.Id);
            if (existingItem != null)
            {
                // Update existing item properties if necessary
                existingItem.Name = item.Name;
                // Add more properties as needed
            }
            else
            {
                // Handle the case when an item is in the mergedCartItems but not in userCartItems
                // Add the new item to the user's cart
                item.UserId = userId;
                _context.CartItems.Add(item);
            }
        }

        _context.SaveChanges();

        return Ok(new { message = "Cart items updated successfully" });
    }
}

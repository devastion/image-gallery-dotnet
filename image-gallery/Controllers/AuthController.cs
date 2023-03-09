using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace image_gallery.Controllers;

public class AuthController : Controller
{
    
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;

    public AuthController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public IActionResult Login()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Logged");


        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([Bind("Id", "Username", "Password")]User newUser)
    {
        var user = _context.Users.Where(b => b.Username == newUser.Username && b.Password == newUser.Password).FirstOrDefault();
        if (user is not null)
        { 
            List<Claim> claims = new List<Claim>() { 
                new Claim(ClaimTypes.Name, user.Username),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme );

            AuthenticationProperties properties = new AuthenticationProperties() {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);
            
            return RedirectToAction("Index", "Logged");
        }



        ViewData["ValidateMessage"] = "User is not found!";
        return View();
    }
    
    public IActionResult Register()
    {
        ClaimsPrincipal claimUser = HttpContext.User;

        if (claimUser.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Logged");


        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(User newUser)
    {
        
        var user =  _context.Users.Where(b => b.Username == newUser.Username).FirstOrDefault();
        
        if (user is not null)
        {
            ViewData["ValidateMessage"] = "Username is taken!";
            return View();
        }
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace image_gallery.Controllers;

public class ImageController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageController(ILogger<HomeController> logger, DataContext context, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _context = context;
        _webHostEnvironment = webHostEnvironment;

    }

    [HttpGet]
    public JsonResult GetUserImages()
    {
        ClaimsPrincipal claimUser = HttpContext.User;

        var user = _context.Users.FirstOrDefault(u => u.Username == claimUser.Identity.Name);
        var images = _context.Images.Where(image => image.User == user);
        var imagesPath = new List<string>();
        foreach (var image in images)
        {
            imagesPath.Add(image.Url);
        }

        return Json(imagesPath);
    }

    public IActionResult Index()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        
        if (!claimUser.Identity.IsAuthenticated)
            return RedirectToAction("Login", "Auth");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(List<IFormFile> files, bool isPrivate)
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        var user = _context.Users.FirstOrDefault(u => u.Username == claimUser.Identity.Name);
        var size = files.Sum(h => h.Length);
        // var filePaths = new List<string>();
        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                Guid myuuid = Guid.NewGuid();
                string myuuidAsString = myuuid.ToString();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", myuuidAsString + "_" +formFile.FileName );
                var dbFilePath = Path.Combine("images", myuuidAsString + "_" +formFile.FileName);
                Image image = new Image();
                image.User = user;
                image.Url = dbFilePath;
                image.Private = isPrivate;
                _context.Images.Add(image);
                await _context.SaveChangesAsync();
                // filePaths.Add(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
        }

        return RedirectToAction("Index", "Logged");
    }
    
    public IActionResult PublicImages()
    {
        return View();
    }
    
    [HttpGet]
    public JsonResult GetPublicImages()
    {
        var images = _context.Images.Where(image => image.Private == false);
        var imagesPath = new List<string>();
        foreach (var image in images)
        {
            imagesPath.Add(image.Url);
        }

        return Json(imagesPath);
    }
}
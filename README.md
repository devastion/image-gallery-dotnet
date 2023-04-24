# Image Gallery .NET v0.1

**Author: Dimitar Banev F95107**  
University project building image gallery using dotnet, entityframework and mssql.

- [Image Gallery .NET v0.1](#image-gallery-net-v01)
  - [Startup Instruction](#startup-instruction)
  - [Functionalities](#functionalities)
    - [Login / Register](#login--register)
    - [Public Images](#public-images)
    - [Upload Image](#upload-image)
  - [Project Structure](#project-structure)
  - [Code Documentation](#code-documentation)
    - [Models](#models)
      - [User Model](#user-model)
      - [Image Model](#image-model)
    - [Views](#views)
      - [Shared Layouts](#shared-layouts)
      - [Home](#home)
      - [Auth](#auth)
      - [Logged](#logged)
      - [Image](#image)
    - [Controllers](#controllers)
      - [HomeController](#homecontroller)
      - [LoggedController](#loggedcontroller)
      - [AuthController](#authcontroller)
      - [ImageController](#imagecontroller)
    - [Data](#data)
      - [DataContext](#datacontext)

## Startup Instruction

1. Make sure you have docker and docker-compose installed.
2. Run `docker-compose up`.
3. Go to `localhost:5166`.
4. Database Connection for tools.

   - Server: `localhost` Port: `8002`
   - Database Name: `image_gallery`
   - Password: `test12!@#$@asd`

## Functionalities

### Login / Register

User can register and then login. Once the user is logged in, the state is preserved through cookie for 20 minutes. After 20 minutes the user needs to login again.

### Public Images

Images that are uploaded by all the users and marked as "Private image". Everyone can see them.

### Upload Image

Viewed only by logged users. User can upload image. If the private box is not checked the image will be visible to all visitors of the website, otherwise the image will be visible only to the user.

## Project Structure

- image-gallery
  - `.config` - configuration files
  - `bin` - dotnet output executables
  - `build` - dotnet project build (deploy ready)
  - `Controllers` - contains all controllers
  - `Data` - contains DataContext with the database connection string and tables
  - `Migrations` - contains all migrations in case of rollback needed
  - `Models` - contains the models
  - `obj` - stores temporary object files and other files used to compile the final binary.
  - `Properties` - contains project settings
  - `Views` - all controller views and shared ones
  - `wwwroot` - all static files (bootstrap, js, etc.)

## Code Documentation

### Models

#### User Model

- `Id` - autoincremented id
- `Username` - required field with maximum 16 characters
- `Password` - required field with maximum 20 characters

#### Image Model

- `Id` - autoincremented id
- `User` - foreign key. It saves the user's `Id`. This way you can select all images from a given user.
- `Url` - where the image is saved'
- `Private` - whether image is private or not

### Views

#### Shared Layouts

- `_Layout.cshtml` - contais the main menu and footer. Included in non-authorized views.
- `_Logged.cshtml` - contains the main menu for logged users. Included in authorized views.

#### Home

- `Index.cshtml` - Homepage.
- `Privacy.cshtml` - Privacy page.

#### Auth

- `Login.cshtml` - login form
- `Register.cshtml` - register form

#### Logged

- `Index.cshtml` - homepage for logged users. Contains javascript ajax request to `ImageController`'s `GetUserImages` action to render all images uploaded by the user.

#### Image

- `Index.cshtml` - upload image page. Contains upload form with privacy checkbox.
- `PublicImages.cshtml` - contains public images page. Also have javascript ajax request to `ImageController`'s `GetPublicImages` action to render all public images.

### Controllers

#### HomeController

- `public IActionResult Index()` - returns Home's view (Views/Home/Index.cshtml) if user is not logged or redirect to `LoggedController`'s `Index()` if user is logged (cookie is available).
- `public IActionResult Privacy()` - return's Home's privacy view.

#### LoggedController

- `public IActionResult Index()` - returns Logged's index view with user's name as ViewData.
- `public async Task<IActionResult> LogOut()` - logouts the user and deletes the cookie. Redirects to login page.

#### AuthController

- `public IActionResult Login()` - returns Login page view if the user is not logged, otherwise it riderect's to `LoggedController`'s `Index()` view.
- `public async Task<IActionResult> Login([Bind("Id", "Username", "Password")]User newUser)` - httpPost request for logging the user. If the user is not found the action return message "User is not found" to the corresponding view.
  - `User newUser` - accepts User model
- `public IActionResult Register()` - returns Register page view if the user is not logged, otherwise it riderect's to `LoggedController`'s `Index()` view.
- `public async Task<IActionResult> Register(User newUser)` - httpPost request that registers a user. If a username is taken, the action returns message "Username is taken" to the Register View.
  - `User newUser` - accepts User model

#### ImageController

- `public JsonResult GetUserImages()` - http post request, that returns list with user's all images. This action is used in the `LoggedController`'s `Index()` view for rendering all user's images.
- `public IActionResult Index()` - return's Upload Image page if user is logged, otherwise redirect's to login page.
- `public async Task<IActionResult> Index(List<IFormFile> files, bool isPrivate)` - http post request for uploading a file from the Upload Image page form.
  - `List<IFormFile> files` - accept single or multiple files
  - `bool isPrivate` - whether image is private or not
- `public IActionResult PublicImages()` - return's Public Images page
- `public JsonResult GetPublicImages()` - http post request that return's json with all public images paths.

### Data

#### DataContext

- connection string - database's connection credentials.
- `public DbSet<User> Users { get; set; }` - creates Users table.
- `public DbSet<Image> Images { get; set; }` - creates Images table

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username })
            .IsUnique(true);
    }
```

Ensures every username is unique.

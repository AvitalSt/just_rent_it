using JustRentItAPI.Data;
using JustRentItAPI.Repositories.Classes;
using JustRentItAPI.Repositories.Interfaces;
using JustRentItAPI.Services.Classes;
using JustRentItAPI.Services.Interfaces;
using JustRentItAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Text;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Xml Settings 
AppContext.SetSwitch("EPPlus:UseOfficeOpenXmlLicense", true);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//JWT Settings
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
var issuer = builder.Configuration["JwtSettings:Issuer"];
var audience = builder.Configuration["JwtSettings:Audience"];

//Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Dependency Injection
// Auth
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<TokenService>();

// Dress
builder.Services.AddScoped<IDressRepository, DressRepository>();
builder.Services.AddScoped<IDressService, DressService>();

var cloudinaryAccount = new Account(
    builder.Configuration["CLOUDINARY_CLOUD_NAME"],
    builder.Configuration["CLOUDINARY_API_KEY"],
    builder.Configuration["CLOUDINARY_API_SECRET"]
);

Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);

builder.Services.AddSingleton(cloudinary);

// Images
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();

// Wishlist
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();

// Users
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Interest
builder.Services.AddScoped<IInterestRepository, InterestRepository>();
builder.Services.AddScoped<IInterestService, InterestService>();

// Options
builder.Services.AddScoped<IOptionsRepository, OptionsRepository>();
builder.Services.AddScoped<IOptionsService, OptionsService>();

// Mail
builder.Services.AddScoped<IMailService, MailService>();

//MonthlySummary
builder.Services.AddScoped<IMonthlySummaryRepository, MonthlySummaryRepository>();
builder.Services.AddScoped<IMonthlySummaryService, MonthlySummaryService>();

// Export
builder.Services.AddScoped<IInterestExportService, InterestExportService>();

//Catalog
builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.AddHttpContextAccessor();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

//Middleware Pipeline 
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
/*app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});*/

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


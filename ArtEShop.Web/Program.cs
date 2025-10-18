using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ArtEShop.Domain.IdentityModels;
using ArtEShop.Repository;
using ArtEShop.Repository.Implementation;
using ArtEShop.Repository.Interface;
using ArtEShop.Service.Interface;
using ArtEShop.Service.Implementation;
using ArtEShop.Domain.Email;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddTransient<IArtPieceService, ArtPieceService>();
builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IRequestService, RequestService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IShoppingCartItemService, ShoppingCartItemService>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.WebRootPath, "uploads")),
    RequestPath = "/resources"
});


app.UseCors();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ArtPieces}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string role = "Admin";

    var roleExist = await RoleManager.RoleExistsAsync(role);
    if (!roleExist)
    {
        await RoleManager.CreateAsync(new IdentityRole(role));
    }

    var admin = new ApplicationUser
    {
        Email = builder.Configuration.GetSection("AdminSettings")["UserEmail"],
        UserName = builder.Configuration.GetSection("AdminSettings")["UserEmail"],
        FirstName = "admin",
        LastName = "l",
    };

    var _user = await UserManager.FindByEmailAsync(builder.Configuration.GetSection("AdminSettings")["UserEmail"]);
    string UserPassword = builder.Configuration.GetSection("AdminSettings")["UserPassword"];

    if (_user == null)
    {
        var createPowerUser = await UserManager.CreateAsync(admin, UserPassword);
        if (createPowerUser.Succeeded)
        {
            await UserManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

app.Run();

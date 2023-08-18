using ASNClub.Data;
using ASNClub.Data.Models;
using ASNClub.Hubs;
using ASNClub.Services.AddressServices;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Services.CategoryServices;
using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.Services.ColorServices;
using ASNClub.Services.ColorServices.Contracts;
using ASNClub.Services.CountyServices;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.Services.OrderServices;
using ASNClub.Services.OrderServices.Contracts;
using ASNClub.Services.ProductServices;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.ProfileServices;
using ASNClub.Services.ProfileServices.Contracts;
using ASNClub.Services.ShoppingCartServices;
using ASNClub.Services.ShoppingCartServices.Contracts;
using ASNClub.Services.TypeServices;
using ASNClub.Services.TypeServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ASNClubDbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ASNClubDbContext>();

builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IMaterialService,MaterialService>();
builder.Services.AddScoped<IColorService,ColorService>();
builder.Services.AddScoped<ITypeService,TypeService>();
builder.Services.AddScoped<IProfileService,ProfileService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSignalR();

builder.Services.AddControllersWithViews();

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
app.Use(async (context, next) =>
{
    if (context.Response.StatusCode == 500)
    {
        context.Request.Path = "/Error/InternalServerError";
        await next();
    }
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Error/NotFound";
        await next();
    }
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<CommentsHub>("/commentsHub");
app.Run();

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Project_Summer.DataContext;
using Project_Summer.Helper;
using Project_Summer.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SummerrContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbSummerr"));
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);// th?i gian ch? c?a session
    options.Cookie.HttpOnly = true;//B?o m?t cookie session
    options.Cookie.IsEssential = true;
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IEmailSender,EmailSender>();
// đăng ký cookie :https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0
builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options
  =>
    {
        options.LoginPath = "/KhachHang/DangNhap";// chưa đăng nhập thì tới trang đăng nhập
        options.AccessDeniedPath = "/KhachHang/DangNhap";// k có quyền cũng tới 
    }
    );
// đăng ký Paypalclient với đối tượng PaypalClient chứa các thuộc tính
builder.Services.AddSingleton( x => new PaypalClient
(
    builder.Configuration["PaypalOptions:AppId"],
    builder.Configuration["PaypalOptions:AppSecret"],
    builder.Configuration["PaypalOptions:Mode"]
));
builder.Services.AddAuthorization();
var app = builder.Build();
builder.Services.AddDistributedMemoryCache();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=ProductManager}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

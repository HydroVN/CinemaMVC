using MVCDatVePhim.Data;
using MVCDatVePhim.Models;
using MVCDatVePhim.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── MVC ───────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── EF Core + SQL Server ──────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── ASP.NET Core Identity ─────────────────────────────────────────
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit           = true;
    options.Password.RequiredLength         = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase       = false;
    options.SignIn.RequireConfirmedAccount   = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Identity cookie paths (Vietnamese routes)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath       = "/TaiKhoan/DangNhap";
    options.LogoutPath      = "/TaiKhoan/DangXuat";
    options.AccessDeniedPath = "/TaiKhoan/KhongCoQuyen";
    options.ExpireTimeSpan  = TimeSpan.FromDays(7);
});

// ── Business Services ─────────────────────────────────────────────
builder.Services.AddScoped<IPhimService,      PhimService>();
builder.Services.AddScoped<ISuatChieuService, SuatChieuService>();
builder.Services.AddScoped<IDatVeService,     DatVeService>();
builder.Services.AddScoped<IGheService,       GheService>();

// ── Session ───────────────────────────────────────────────────────
builder.Services.AddSession(options =>
{
    options.IdleTimeout        = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly    = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapStaticAssets();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ── Routes ────────────────────────────────────────────────────────
// Admin Area route (must come before default)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// ── Seed Data ─────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

app.Run();

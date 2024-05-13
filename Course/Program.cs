using Course.Data;
using Course.Data.DAL;
using Course.Middleware;
using Course.Services.Email;
using Course.Services.Hash;
using Course.Services.Kdf;
using Course.Services.Hash;
using Course.Services.Kdf;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("emailconfig.json", true);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("LocalMSSQL"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(60),
            errorNumbersToAdd: null);
        }), ServiceLifetime.Singleton
);

//builder.Services.AddSingleton<IHashService, Md5HashService>();

builder.Services.AddSingleton<IHashService, ShaHashService>();

/*builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("LocalMSSQL")),
    ServiceLifetime.Singleton);*/

builder.Services.AddSingleton<DataAccessor>();
builder.Services.AddSingleton<IKdfService, Pbkdf1Service>();
builder.Services.AddSingleton<IEmailService, GmailService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseCors(builder => builder

                .AllowAnyMethod()

                .AllowAnyHeader()

                .SetIsOriginAllowed(origin => true) // allow any origin

                .AllowCredentials());

app.UseAuthorization();

app.UseSession();

app.UseAuthSession();

app.UseMiddleware<AuthSessionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

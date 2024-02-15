using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NpgSqlBackup;
using NpgSqlBackup.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddSingleton<ImageDirectoryProvider>();

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>()?.Database?.Migrate();

var imagePathProvider = app.Services.GetService<ImageDirectoryProvider>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// app.UseStaticFiles(new StaticFileOptions()
// {
//     FileProvider = new PhysicalFileProvider(imagePathProvider!.GetPath()),
//     RequestPath = "/images"
// });
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
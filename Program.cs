using System;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 🔥 IMPORTANT FOR RENDER: configure port properly
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
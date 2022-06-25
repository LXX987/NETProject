using BlazorApp.Data;
using BlazorApp.Service;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
/*builder.Services.AddHttpClient("UserApi", c =>
{
    c.BaseAddress = new Uri("https://localhost:7232/");
    //c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
});*/
builder.Services.AddHttpClient<UserApiClient>(client => client.BaseAddress = new Uri("https://localhost:7232/"));
builder.Services.AddHttpClient<UserApiService>(client => client.BaseAddress = new Uri("https://localhost:7232/"));
builder.Services.AddHttpClient<AuthProvider>(client => client.BaseAddress = new Uri("https://localhost:7232/"));
builder.Services.AddAntDesign();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore(option =>
{
    option.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

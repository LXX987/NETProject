using BlazorApp.Data;
using BlazorApp.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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

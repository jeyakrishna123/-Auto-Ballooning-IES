using BubbleDrawingAutomationWeb.Middleware;
using BubbleDrawingAutomationWeb.Models;
using BubbleDrawingAutomationWeb.Models.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Reflection;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore;
using System;
using Azure.Core;
using System.Net.Sockets;
using Emgu.CV.Ocl;
using System.Net;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using BubbleDrawingAutomationWeb.Controllers;

var builder = WebApplication.CreateBuilder(args);
int timespan = 20;
var corsPolicies = builder.Configuration.GetSection("AppSettings:CorsPolicies").GetChildren();
// builder.Logging.AddLog4Net();
// Add the memory cache services
// Add services to the container.
builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
builder.Services.AddDbContext<DimTestContext>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
string ClientAppUrl = string.Empty;
builder.Services.AddCors(c =>
{
    foreach (var policy in corsPolicies)
    {
        c.AddPolicy(policy.Key, options =>
        {
            var origins = policy.GetSection("Origins");
            foreach (var origin in origins.GetChildren())
            {
                string corsurl = origin.Value;
                ClientAppUrl = corsurl;
                options.WithOrigins(corsurl)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();               //preflight cors error in deployemnt
            }

        });
    }

    c.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
    });
});

builder.Services.AddEndpointsApiExplorer();

// Step 1 to use session  
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.Name = "bubble";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services
    .AddHttpClient<TcpClient>("GetDrawingServiceUrlList").ConfigureHttpClient(
                (serviceProvider, httpClient) =>
                {
                    var httpClientOptions = serviceProvider
                        .GetRequiredService<IOptions<AppSettings>>()
                        .Value;
                    httpClient.BaseAddress = new Uri(httpClientOptions.GetDrawingServiceUrlList);
                    httpClient.Timeout = new TimeSpan(0, 0, 30); // 30 seconds
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))    // Default is 2 mins
    .ConfigurePrimaryHttpMessageHandler(x =>
            new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = false,
                AllowAutoRedirect = false,
                UseDefaultCredentials = true,
            });
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.WebHost.ConfigureKestrel(c =>
{
    c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(timespan);
    c.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
});

// builder.Services.AddLogging(builder => builder.AddConsole());
// builder.WebHost.UseKestrel();


var app = builder.Build();
//ILogger logger = app.Logger;

string ErrorLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog");
string serverDrawing = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClientApp\\src\\drawing");

// Create folders if they do not exist
if (!Directory.Exists(ErrorLog))
{
    Directory.CreateDirectory(ErrorLog);
}

if (!Directory.Exists(serverDrawing))
{
    Directory.CreateDirectory(serverDrawing);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}
app.Use(async (context, next) =>
{
    try
    {
        if (context.Request.Method == "OPTIONS")
        {
            if (!builder.Environment.IsDevelopment())
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", ClientAppUrl);
            }
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
            // context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, application/json");            //preflight cors error in deployemnt -cmd
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With");              //preflight cors error in deployemnt - add
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.StatusCode = 200;
            return;
        }
        await next();
    }
    catch (Exception ex)
    {
        throw;
    }
});
// app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "ClientApp")),
    RequestPath = "/StaticFiles"
});
app.UseStatusCodePages();
app.UseCookiePolicy();
app.UseRouting();

foreach (var policy in corsPolicies)
{
    app.UseCors(policy.Key);
}

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
// use Middleware
app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
//app.UseMiddleware<RequestTimeoutMiddleware>(TimeSpan.FromMinutes(timespan));


app.MapControllers();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
if (!app.Environment.IsDevelopment())
{
    app.MapGet("/", () => "Welcome to Halliburton test");
}
app.Run();

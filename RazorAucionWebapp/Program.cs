global using Repository.Interfaces.AppAccount;
using RazorAucionWebapp.BackgroundServices;
using RazorAucionWebapp.Configure;
using Repository.Database;
using Repository.Database.Model.Enum;
using System.Security.Claims;
using System.Text.Json;

string json = File.ReadAllText("appsettings.json");
var appSettings = JsonSerializer.Deserialize<BindAppsettings>(json, new JsonSerializerOptions()
{
    AllowTrailingCommas = true,
    ReadCommentHandling = JsonCommentHandling.Skip
}) ;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(opt => 
{
    //opt.Conventions.AuthorizeFolder("/AdminPages", "Admin");
    //opt.Conventions.AuthorizeFolder("/CompanyPages", "Company");
    //opt.Conventions.AuthorizeFolder("/CustomerPages", "Customer");

});

builder.Services.AddDbContext<AuctionRealEstateDbContext>();

builder.Services.AddMyRepositories();
builder.Services.AddMyServices();

builder.Services.AddSingleton<ServerDefaultValue>();
builder.Services.AddSingleton(appSettings);

builder.Services.AddHostedService<CheckAuctionTimeWorker>();
builder.Services.AddHostedService<CheckPaymentReachDeadline>();

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie", options =>
    {
        options.LoginPath = "/Registration/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/Unauthorized";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

//builder.Services.AddAuthorization(config =>
//{
//    config.AddPolicy("Admin", c =>
//    {
//        c.RequireAuthenticatedUser();
//        c.RequireRole("ADMIN");
//    });
//    config.AddPolicy("Customer", c =>
//    {
//        c.RequireAuthenticatedUser();
//        c.RequireRole("CUSTOMER");
//    });
//    config.AddPolicy("Company", c =>
//    {
//        c.RequireAuthenticatedUser();
//        c.RequireRole("COMPANY");
//    });
//    config.AddPolicy("VerifiedUser", c =>
//    {
//        c.RequireClaim("IsVerified", "true");
//    });
//});
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
app.Use( async (context, next) => 
{
    await next(context);
    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        context.Response.Redirect("/Unauthorized");
});
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

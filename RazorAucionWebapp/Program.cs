global using Repository.Interfaces.AppAccount;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using RazorAucionWebapp.BackgroundServices;
using RazorAucionWebapp.Configure;
using Repository.Database;
using Repository.Database.Model.Enum;
using Repository.Interfaces.DbTransaction;
using Service.MyHub;
using Service.MyHub.HubServices;
using System.Security.Claims;
using System.Text.Json;

string json = File.ReadAllText("appsettings.json");
var appSettings = JsonSerializer.Deserialize<BindAppsettings>(json, new JsonSerializerOptions()
{
    AllowTrailingCommas = true,
    ReadCommentHandling = JsonCommentHandling.Skip
});
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddRazorPages(opt => 
//{
//    opt.Conventions.AuthorizeFolder("/AdminPages", "Admin");
//	opt.Conventions.AuthorizeFolder("/AdminPages", "VerifiedUser");

//	opt.Conventions.AuthorizeFolder("/CompanyPages", "Company");
//	opt.Conventions.AuthorizeFolder("/CompanyPages", "Admin");
//	opt.Conventions.AuthorizeFolder("/CompanyPages", "VerifiedUser");

//	opt.Conventions.AuthorizeFolder("/Vnpay", "VerifiedUser");

//	opt.Conventions.AuthorizeFolder("/CustomerPages", "Customer");
//	opt.Conventions.AuthorizeFolder("/CustomerPages", "Company");
//	opt.Conventions.AuthorizeFolder("/CustomerPages", "Admin");
//	opt.Conventions.AllowAnonymousToPage("/CustomerPages/DetailAuction");

//	opt.Conventions.AuthorizePage("/CustomerPages/Transactions/Create", "VerifiedUser");
//	opt.Conventions.AuthorizePage("/CustomerPages/ReceiptPayment/Create", "VerifiedUser");
//	opt.Conventions.AuthorizePage("/CustomerPages/BidAuction", "VerifiedUser");
//	opt.Conventions.AuthorizePage("/CustomerPages/JoinAuction", "VerifiedUser");


//});

builder.Services.AddDbContext<AuctionRealEstateDbContext>();

builder.Services.AddMyRepositories();
builder.Services.AddMyServices();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ServerDefaultValue>();
builder.Services.AddSingleton(appSettings);

builder.Services.AddHostedService<CheckAuctionTimeWorker>();
builder.Services.AddHostedService<CheckPaymentReachDeadline>();

builder.Services.AddSignalR(config =>
{
    config.MaximumReceiveMessageSize = 64 * 1024;
}).AddNewtonsoftJsonProtocol(options =>
{
    options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddScoped<AuctionHubService>();
builder.Services.AddScoped<AccountHubService>();
builder.Services.AddScoped<BidHubServices>();
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie", options =>
    {
        options.LoginPath = "/Registration/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/Unauthorized";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("Admin", c =>
    {
        c.RequireRole("ADMIN");
        c.RequireClaim("Status", "ACTIVED");
        c.RequireAuthenticatedUser();
    });
    config.AddPolicy("Customer", c =>
    {
        c.RequireRole("CUSTOMER");
        c.RequireAuthenticatedUser();
    });
    config.AddPolicy("Company", c =>
    {
        c.RequireRole("COMPANY");
        c.RequireClaim("Status", "ACTIVED");
        c.RequireAuthenticatedUser();
    });
    config.AddPolicy("VerifiedUser", c =>
    {
        c.RequireClaim("Status", "ACTIVED");
        c.RequireAuthenticatedUser();
    });
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
app.Use(async (context, next) =>
{
    await next(context);
    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        var getMessageIfPossible = context.Items.TryGetValue( "UnauthorizedMessage", out var message);
        if(getMessageIfPossible is true)
        {
            context.Response.Redirect("/Unauthorized?message=" + message);
        }
        else
        {
            context.Response.Redirect("/Unauthorized");
        }
    }
});
app.UseAuthentication();
app.Use(async (context, next) =>
{
    var getIdentity = context.User.Identity;
    if (getIdentity is not null)
    {
        if (getIdentity.IsAuthenticated)
        {
            var getRole = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role)).Value;
            // admin pass this
            if (getRole != Role.ADMIN.ToString())
            {
                var getActiveClaim = context.User.Claims.FirstOrDefault(c => c.Type.Equals("Status")).Value;
                var getUserId = context.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;
                var scope = context.RequestServices.GetRequiredService<IUnitOfWork>();

                var getUser = await scope.Repositories.accountRepository.GetAsync(int.Parse(getUserId));
                if (getUser.Status.Equals(AccountStatus.DEACTIVED) && getUser.Status.ToString() != getActiveClaim)
                {
                    await context.SignOutAsync("cookie");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Items.Add("UnauthorizedMessage", "user has been banned, please contact admin to active your account");
                    return;
                }
            }
        }
        else
        {
            // do nothing, continue run next middleware
        }
    }
    await next(context);
});
app.UseAuthorization();
app.UseEndpoints(endpoint =>
{
    endpoint.MapRazorPages();
    endpoint.MapHub<AuctionHub>("auctionrealtime");
    endpoint.MapHub<AccountHub>("accountrealtime");
    endpoint.MapHub<BidHub>("bidrealtime");
});

app.Run();

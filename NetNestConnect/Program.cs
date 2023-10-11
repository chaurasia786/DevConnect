using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NetNestConnect;
using NetNestConnect.Model;
using NetNestConnect.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DatabaseContext>
(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"), opts => opts.EnableRetryOnFailure()));
builder.Services.AddScoped<IUser, UserCore>();
builder.Services.AddScoped<IEmailService, EmailService>();
// Add services to the container.
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Set this to true in production
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddSwaggerGen(c =>
c.AddSecurityDefinition("token", new OpenApiSecurityScheme
{
    Type = SecuritySchemeType.Http,
    In = ParameterLocation.Header,
    Name = HeaderNames.Authorization,
    Scheme = "Bearer"
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add the static file middleware here
app.UseStaticFiles();

// If you want to serve AngularJS or other client-side apps from the "ClientApp" folder
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "ClientApp")),
    RequestPath = "/ClientApp",
    EnableDirectoryBrowsing = true  // Only enable this for development for security reasons
});

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseSession();

app.Run();

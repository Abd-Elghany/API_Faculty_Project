using API_Lab1.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVCClient", policy =>
    {
        policy.WithOrigins("https://localhost:7109") // your MVC client URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure DbContext in
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<Context>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true; //Not Expired?
    options.RequireHttpsMetadata = false; //specfic Protocol Https

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  
        ValidIssuer = builder.Configuration["JWT:IssuerIP"],
        ValidateAudience = true,  
        ValidAudience = builder.Configuration["JWT:AudienceIP"], 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecertKey"]))

    };

});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddSwaggerGen();

// Customize swagger to test Authorization

#region Customize swagger to test Authorization

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
        Description = " ITI Projrcy"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "\"Enter 'Bearer' [space] and then your valid token in the text input below.\\r\\n\\r\\nExample: \\\"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6\r\n IkpXVCJ9\"",

    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
              new OpenApiSecurityScheme{Reference = new OpenApiReference
              {
                  Type = ReferenceType.SecurityScheme,Id = "Bearer"}
              },
             new string[] {}

         }
     });
});
#endregion




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowMVCClient"); // Apply the named policy


app.UseAuthorization();

app.MapControllers();

app.Run();

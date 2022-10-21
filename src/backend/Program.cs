using backend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.Authority = configuration["Jwt:Authority"];
    options.Audience = configuration["Jwt:Audience"];
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        //ValidAudiences = new[] { "master-realm", "account" },
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Authority"],
        ValidateLifetime = true, 
        RequireExpirationTime = true                        
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});


// Scoped access handler, attribute will not work without it: Authorize("my_scope")
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


// Restrict access by scope for the entire application

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("my_scope", policy => policy.Requirements.Add(new HasScopeRequirement("my_scope", configuration["Jwt:Authority"])));
// });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("ReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

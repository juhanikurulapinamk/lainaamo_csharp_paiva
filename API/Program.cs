


using System.Text;
using API.Data;
using API.Interfaces;
using API.Models;
using API.Services;
using API.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// nämä kolme liitännäistä tarvitaan siihen, että swagger / OpenAPI-dokumentaatio toimii
// automaattisesti
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddScoped<IUserService, UserService>();
// PasswordHasher on tilaton, joten se voi olla singleton
builder.Services.AddSingleton<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    // varmistetaan, että TokenKey löytyy
    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("token key not found");
    // konfataan tässä, mitä tarkistetaan
    o.TokenValidationParameters = new TokenValidationParameters
    {

        // varmistaa allekirjoituksen
        ValidateIssuerSigningKey = true,
        // jotta allekirjoituksen voi tarkistaa,
        // pitää kertoa, mitä avainta allekirjoituksessa käytetään
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        // issuerin tarkistus on pois päältä
        
        ValidateIssuer = false,
        // myös audiencen tarkistus on pois päältä
        ValidateAudience = false
       
    };

     o.MapInboundClaims = false;

});


// yläpuolella automapper
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
});


builder.Services.AddScoped<ITokenTool, SymmetricTokenTool>();


// AutoMapper 15:stä lähtien ensimmäinen parametri on konfiguraatiofunktio.
// Tyhjä cfg => { } riittää, koska profiilit haetaan typeof(Program)-assemblysta.
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
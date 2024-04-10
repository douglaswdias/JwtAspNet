using System.Security.Claims;
using System.Text;
using JwtAspNet;
using JwtAspNet.Extensions;
using JwtAspNet.Models;
using JwtAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication(x => {
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
  x.TokenValidationParameters = new TokenValidationParameters
  {
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
    ValidateIssuer = false,
    ValidateAudience = false
  };
});
builder.Services.AddAuthorization(x =>
{
  x.AddPolicy("Admin", p => p.RequireRole("admin"));
}

);

var app = builder.Build();
app.UseAuthorization();
app.UseAuthorization();

app.MapGet("/login", (TokenService service) 
  => 
  {
    var user = new User(
      1,
      "Douglas",
      "douglas@hotmail.com",
      "image",
      "1234",
      ["student", "student"]
    );
    return    service.Create(user);
  }
);

app.MapGet("/restrict", (ClaimsPrincipal user) => new
{
  id = user.GetId(),
  name = user.GetName(),
  email = user.GetEmail(),
  givenName = user.GetGivenName(),
  image = user.GetImage(),
}).RequireAuthorization();

app.MapGet("/admin", () => "Você tem acesso").RequireAuthorization("admin");

app.Run();

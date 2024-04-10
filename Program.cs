using JwtAspNet.Models;
using JwtAspNet.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();

var app = builder.Build();

app.MapGet("/", (TokenService service) 
  => 
  {
    var user = new User(
      1,
      "Douglas",
      "douglas@gmail.com",
      "image",
      "1234",
      ["student", "student"]
    );
    return    service.Create(user);
  });

app.Run();

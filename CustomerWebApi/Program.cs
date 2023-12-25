using Clients;
using CustomerWebApi;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddCustomJwtAuthentication();
builder.Services.AddAuthentication("token")
                .AddJwtBearer("token", options =>
                {
                    options.Authority = Constants.Authority;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });
var connectionString = $"Server=(localdb)\\mssqllocaldb;Database=SMCT.Dummy;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<CustomerDbContext>(opt => opt.UseSqlServer(connectionString));
/* ===================================== */

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

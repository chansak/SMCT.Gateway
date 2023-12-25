using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Text;
using Clients;
using IdentityModel;

namespace JwtAuthenticationManager
{
    public static class CustomJwtAuthExtension
    {
        public static void AddCustomJwtAuthentication(this IServiceCollection services)
        {
            
            services.AddAuthentication(options =>
            {
                //Combining cookie and JWT bearer authentication for supporting both cookie-based authentication and token based authentication (modern)
                //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                //OpenIdConnection
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = "/Account/Login";
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = Clients.Constants.Authority;

                    options.ClientId = "mvc.code";
                    options.ClientSecret = "secret";

                    // code flow + PKCE (PKCE is turned on by default)
                    options.ResponseType = "code";
                    options.UsePkce = true;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("custom.profile");
                    options.Scope.Add("resource1.scope1");
                    options.Scope.Add("resource2.scope1");
                    options.Scope.Add("offline_access");

                    // not mapped by default
                    options.ClaimActions.MapAll();
                    options.ClaimActions.MapJsonKey("website", "website");
                    options.ClaimActions.MapCustomJson("address", (json) => json.GetRawText());

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,
                    };

                    options.Events.OnRedirectToIdentityProvider = ctx =>
                    {
                        // ctx.ProtocolMessage.Prompt = "create";
                        return Task.CompletedTask;
                    };
                });
            //.AddOpenIdConnect(options =>
            //{
            //    options.ClientId = "client_id";
            //    options.ClientSecret = "client_secret";
            //    options.Authority = String.Format("https://localhost:5101/", "us");

            //    options.ResponseType = "code";
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //});
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtTokenHandler.JWT_SECURITY_KEY)),
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});
        }
    }
}

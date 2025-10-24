using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JWTAuthentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Configure rate limiting policies
            builder.Services.AddRateLimiter(options =>
            {
                // Global fixed window limiter
                options.AddFixedWindowLimiter("login", opt =>
                {
                    opt.PermitLimit = 5;    // Allow 5 requests
                    opt.Window = TimeSpan.FromSeconds(10); // Every 10 seconds
                    opt.QueueLimit = 0;     // No queued requests
                });
                // General API policy
                options.AddFixedWindowLimiter("general", opt =>
                {
                    opt.PermitLimit = 100;  // 100 requests
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueLimit = 0;
                });
            });
            //builder.Services.AddAuthentication();
            builder.Services.AddAuthentication(s =>
            {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTPrivateKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                // 👇 Add event handlers for debugging
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"❌ Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var claims = context.Principal.Claims
                            .Select(c => $"{c.Type}: {c.Value}");
                        Console.WriteLine("✅ Token validated. Claims: " + string.Join(", ", claims));
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        Console.WriteLine($"📥 Token received: {context.Token}");
                        return Task.CompletedTask;
                    }
                };
            });
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTAuthetication", Version = "v1" });

                // 🔑 Add JWT Bearer security definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer prefix (Example: 'Bearer eyJhbGci...')",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // 🔒 Apply the scheme globally
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
            });
            builder.Services.AddSingleton<IJwtAuthentication>(new JwtAuthentication(builder.Configuration.GetValue<string>("JWTPrivateKey")));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT API token");
                });
            }
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

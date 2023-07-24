using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeFly.Helper;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            // ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<CodeFlyDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddCors(options =>
            {
                options.AddPolicy("YouShallPass", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "https://epita.net",
                    ValidAudience = "https://codefly.com",
                    IssuerSigningKey = new SymmetricSecurityKey(AuthManager.SecretBytes),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });


            services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); });
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CodeFly", Version = "v1" }); 
                    
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] { }
                        }
                    });

                });
            services.AddTransient<Repository>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CodeFly v1"));


            // app.UseHttpsRedirection();

            app.UseCors("YouShallPass");


            app.UseRouting();

            // Use authentication middleware
            app.UseAuthentication();

            // Use authorization middleware
            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
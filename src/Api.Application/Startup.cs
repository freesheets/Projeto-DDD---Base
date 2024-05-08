using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace application
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
            //Adicionando o Cors para liberar o acesso uma chamada especifica
            services.AddCors();
            services.AddControllers();

            // Na Startup.cs, depende da versão do ASP.NET
            // ASP.NET HttpContext dependency

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Projeto (Domain Drive Desing)",
                    Description = "",
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //String para chamar a API 
            var urlCliente = "";
            app.UseCors(c => c.WithOrigins(urlCliente));

            //String para chamar a API //http://localhost:5000/api/urldacontroller

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projeto DDD v1");
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();

            // Swagger Bearer
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

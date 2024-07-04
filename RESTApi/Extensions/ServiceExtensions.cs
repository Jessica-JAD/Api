using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using RESTApi.Utiles;
using System;
using System.IO;
using System.Text;

namespace RESTApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
                options.AuthenticationDisplayName = null;
                options.ForwardClientCertificate = true;
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        //Configurar la autenticacion a usar (TOKENs)
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "get.cu",
                    ValidAudience = "get.cu",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["secret_jwt"]))
                };
            });
        }
        
        //Configurar Swagger
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = General.RESTApiName, Version = "v1" });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "RESTApi.xml");
                c.IncludeXmlComments(filePath);

                //Para hacer uso del token desde el propio Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header {token}",
                    Name = "token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
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
                            Array.Empty<string>()
                        }
                    });
            });
        }


        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration config)
        {
            string host = config.GetSection("DBZun").GetValue<string>("host");
            string dbname = "IHSecurity";
            string username = config.GetSection("DBZun").GetValue<string>("username");
            string password = config.GetSection("DBZun").GetValue<string>("password");

            General.GetConnectionData(username, password, host, dbname);

            var connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", General.RESTServer, General.RESTDBName, username, password);
            //connectionString = config["SQLConnections:RESTIntegracionContext"];

            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging();
            });
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

    }

}

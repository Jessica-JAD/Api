using Contracts;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTApi.Extensions;
using RESTApi.Utiles;

namespace RESTApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // C* LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.ConfigureSQLContext(Configuration);
            services.ConfigureCors();
            services.ConfigureAuthentication(Configuration);
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSwagger();
            services.ConfigureRepositoryWrapper();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c => { 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{General.RESTApiName} v1");
                    //c.RoutePrefix = ""; //para evitar problemas con la ruta en la que se lanza la aplicación al correr el proyecto
                });
            }

            //app.ConfigureExceptionHandler(logger);
            //app.ConfigureCustomExceptionMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseRouting();

            app.UseCors("CORSPolicy"); // Es recomendable incluir CORS entre UseRouting y UseAuthorization

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

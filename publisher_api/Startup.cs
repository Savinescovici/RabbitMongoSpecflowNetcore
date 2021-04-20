using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using publisher_api.Mongo;
using publisher_api.Services;
using rabbit;

namespace publisher_api
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
            services.AddMvc();

            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<IMongoSettings>(x => x.GetRequiredService<IOptions<MongoSettings>>().Value);
            services.AddSingleton<IMongoService, MongoService>();

            services.Configure<RabbitSettings>(ops => Configuration.GetSection(nameof(RabbitSettings)).Bind(ops));

            services.AddScoped<IUsersService, UsersService>();
            services.AddSingleton<IRabbitConnectionFactory, RabbitConnectionFactory>();
            services.AddSingleton<IRabbitService, RabbitService>();

            var version = GetType().Assembly.GetName().Version;
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc($"v{version}",
                    new OpenApiInfo
                    {
                        Version = version.ToString(),
                        Title = Configuration["Swagger:Title"],
                        Description = Configuration["Swagger:Description"]
                    });

                swaggerGenOptions.CustomSchemaIds(id => id.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var version = GetType().Assembly.GetName().Version;
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"Version {version}");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
} 

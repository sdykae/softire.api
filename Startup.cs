using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using softire.api.Models;

namespace softire.api
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddControllers();
            services.AddDbContext<NeumaticosContext>();
            /* Dependency injection for Types */
            /* fromVehicles */
            services.AddScoped<ISnVehiculoRepository, SnVehiculoRepository>();  
            services.AddScoped<ISnMarcaVehiculoRepository, SnMarcaVehiculoRepository>();  
            services.AddScoped<ISnModeloVehiculoRepository, SnModeloVehiculoRepository>();  
            services.AddScoped<ISnTipoVehiculoRepository, SnTipoVehiculoRepository>();  
            services.AddScoped<ISnConfiguracionRepository, SnConfiguracionRepository>();  
            /* fromTires */
            services.AddScoped<ISnNeumaticosRepository, SnNeumaticosRepository>();  
            services.AddScoped<ISnCondicionesNeumaticoRepository, SnCondicionesNeumaticoRepository>();
            services.AddScoped<ISnDisenosNeumaticoRepository, SnDisenosNeumaticoRepository>();
            services.AddScoped<ISnMarcaNeumaticoRepository, SnMarcaNeumaticoRepository>();
            services.AddScoped<ISnMedidaNeumaticoRepository, SnMedidaNeumaticoRepository>();
            services.AddScoped<ISnModeloNeumaticoRepository, SnModeloNeumaticoRepository>();
            /*others */
            services.AddScoped<ISnNeumaticosDetRepository, SnNeumaticosDetRepository>();  
            services.AddScoped<ISnProveedoresRepository, SnProveedoresRepository>();
            services.AddScoped<ITiresDetRepository, TiresDetRepository>();
            //GraphQL configuration  
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));  
            services.AddScoped<ISnNeumaticosViewRepository,SnNeumaticosViewRepository>();
            services.AddScoped<SnNeumaticosDetSchema>();  
            services.AddGraphQL(o => { o.ExposeExceptions = false; })  
                .AddGraphTypes(ServiceLifetime.Scoped);

            //enable synchronous op
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("MyPolicy");

            app.UseGraphQL<SnNeumaticosDetSchema>();  
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()); 

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

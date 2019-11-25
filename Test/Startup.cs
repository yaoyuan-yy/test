using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            #region Swagger
            services.AddSwaggerGen(option=> {
                option.SwaggerDoc("v1",new Swashbuckle.AspNetCore.Swagger.Info{
                    Title="Test API",
                    Version="V1"
                });
                string xmlPath = Path.Combine(basePath,"Test.xml");
                option.IncludeXmlComments(xmlPath);
            });
            #endregion

            services.AddStudent(Configuration);

            #region AutoFac
            var build = new ContainerBuilder();
            string assemblyPath = Path.Combine(basePath,"Repository.dll");
            var assembly = Assembly.LoadFile(assemblyPath);
            build.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
            build.Populate(services);
            var serverProvider = build.Build();
            return new AutofacServiceProvider(serverProvider);
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                #region Swagge
                app.UseSwagger();
                app.UseSwaggerUI(option => {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API Name");
                    option.RoutePrefix = "";
                });
                #endregion
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

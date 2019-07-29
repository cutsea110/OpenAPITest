using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.XPath;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace OpenAPITest
{
    #region LinqToDB設定
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class DbSettings : ILinqToDBSettings
    {
        private IConfiguration configuration;
        private IConfigurationSection section;

        public DbSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.section = configuration.GetSection("ConnectionStrings");
        }

        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "SqlServer";

        public string DefaultDataProvider => "SqlServer";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = section.GetValue<string>("Name"),
                        ProviderName = section.GetValue<string>("ProviderName"),
                        ConnectionString = section.GetValue<string>("ConnectionString"),
                    };
            }
        }
    }
    #endregion

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
            DataConnection.DefaultSettings = new DbSettings(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(option =>
            {
                option.IncludeXmlComments(GetXmlCommentsPath());
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "PeppaWeb API", Version = "v1" });
            });
        }

        private string GetXmlCommentsPath()
        {
            var baseDirectory = System.AppContext.BaseDirectory;
            var commentsFileName = string.Format("{0}.XML", Assembly.GetExecutingAssembly().GetName().Name);
            var commentsFilePath = Path.Combine(baseDirectory, commentsFileName);

            return commentsFilePath;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PeppaWeb API V1");
            });
        }
    }
}

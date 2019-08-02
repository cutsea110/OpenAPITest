using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

    #region 鍵設定
    public static class AppConfiguration
    {
        public static string SiteUrl => "http://localhost:5000";
        public static string SecretKey { get; } = Guid.NewGuid().ToString();
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Audience = AppConfiguration.SiteUrl;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = AppConfiguration.SiteUrl,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.SecretKey))
                };
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(option =>
            {
                option.IncludeXmlComments(XmlCommentsPath);
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "PeppaWeb API", Version = "v1" });
            });
        }

        private string XmlCommentsPath => Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML");

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

            app.UseAuthentication();
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(opions =>
            {
                opions.SwaggerEndpoint("/swagger/v1/swagger.json", "PeppaWeb API V1");
            });
        }
    }
}

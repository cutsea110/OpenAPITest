using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using LinqToDB.Configuration;
using LinqToDB.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    public class JwtSecretKey
    {
        public string SiteUri { get; set; }
        public string SecretKey { get; set; }
        public int Life { get; set; }

        public JwtSecretKey(IConfiguration conf)
        {
            var section = conf.GetSection("JwtSecretKey");
            SiteUri = section.GetValue<string>("SiteUri");
            SecretKey = section.GetValue<string>("SecretKey");
            Life = section.GetValue<int>("Life");
        }
    }
    public static class AppConfiguration
    {
        public static JwtSecretKey JwtSecret { get; set; }
    }
    #endregion

    #region SwaggerUIへのJwt対応
    public class AssignJwtSecurityRequirements : IOperationFilter
    {
        /// <summary>
        /// Swagger UI用のフィルタ。
        /// Swagger上でAPIを実行する際のJWTトークン認証対応を実現する。
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //AllowAnonymousが付いている場合は、アクセスコードを要求しない
            var allowAnonymousAccess = context.MethodInfo.CustomAttributes
                .Any(a => a.AttributeType == typeof(AllowAnonymousAttribute));

            if (allowAnonymousAccess == false)
            {
                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "JwtBearerAuth" }
                };
                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement
                    {
                        { oAuthScheme, new List<string>() }
                    }
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
            AppConfiguration.JwtSecret = new JwtSecretKey(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Audience = AppConfiguration.JwtSecret.SiteUri;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = AppConfiguration.JwtSecret.SiteUri,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.JwtSecret.SecretKey))
                };
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(option =>
            {
                //トークン認証用のUIを追加する
                option.AddSecurityDefinition("JwtBearerAuth",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                    });

                // 入力したトークンをリクエストに含めるためのフィルタを追加
                option.OperationFilter<AssignJwtSecurityRequirements>();

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

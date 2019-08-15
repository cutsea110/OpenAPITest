using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using LinqToDB.Configuration;
using LinqToDB.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using peppa.util;

using OpenAPITest.CustomPolicyProvider;
using OpenAPITest.CustomFilter;

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
    /// <summary>
    /// JWT Bearer認証鍵
    /// </summary>
    public class JwtSecretKey
    {
        /// <summary>
        /// 認証されるサービスサイト
        /// </summary>
        public string SiteUri { get; set; }
        /// <summary>
        /// 秘密鍵
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// セッションの有効期間(日数)
        /// </summary>
        public int Life { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="conf"></param>
        public JwtSecretKey(IConfiguration conf)
        {
            var section = conf.GetSection("JwtSecretKey");
            SiteUri = section.GetValue<string>("SiteUri");
            SecretKey = section.GetValue<string>("SecretKey");
            Life = section.GetValue<int>("Life");
        }
    }
    #endregion

    #region アクセス制御設定
    /// <summary>
    /// アクセス制御
    /// </summary>
    public class AccessControl
    {
        #region properties
        /// <summary>
        /// アクセス許可ネットワークリスト
        /// </summary>
        public IPNetwork[] AllowedNetworks { get; set; }
        /// <summary>
        /// アクセス許可IPリスト
        /// </summary>
        public IPAddress[] AllowedIpAddresses { get; set; }
        /// <summary>
        /// アクセス拒否ネットワークリスト
        /// </summary>
        public IPNetwork[] DeniedNetworks { get; set; }
        /// <summary>
        /// アクセス拒否IPリスト
        /// </summary>
        public IPAddress[] DeniedIpAddresses { get; set; }
        /// <summary>
        /// 内部アクセスIPリスト
        /// </summary>
        public IPAddress[] InsiderIpAddresses { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="conf"></param>
        public AccessControl(IConfiguration conf)
        {
            var section = conf.GetSection("AccessControl");
            AllowedNetworks = section.GetValue<string>("AllowedNetworks").ParseIPNetworks().ToArray();
            AllowedIpAddresses = section.GetValue<string>("AllowedIpAddresses").ParseIPAddresses().ToArray();
            DeniedNetworks = section.GetValue<string>("DeniedNetworks").ParseIPNetworks().ToArray();
            DeniedIpAddresses = section.GetValue<string>("DeniedIpAddresses").ParseIPAddresses().ToArray();
            InsiderIpAddresses = section.GetValue<string>("InsiderIpAddresses").ParseIPAddresses().ToArray();
        }
        #endregion

        #region methods
        /// <summary>                                                                                                                                                         
        /// 明示的に許可されている                                                                                                                                                       
        /// </summary>                                                                                                                                                        
        /// <param name="ip"></param>                                                                                                                                         
        /// <returns></returns>                                                                                                                                               
        private bool explicitAllowed(IPAddress ip)
        {
            return
                AppConfiguration.AccessControl.AllowedIpAddresses.Contains(ip) || AppConfiguration.AccessControl.AllowedNetworks.Any(net => net.Contains(ip))
                ;
        }

        /// <summary>                                                                                                                                                         
        /// 暗黙的に許可されている                                                                                                                                                       
        /// </summary>                                                                                                                                                        
        /// <param name="ip"></param>                                                                                                                                         
        /// <returns></returns>                                                                                                                                               
        private bool implicitAllowed(IPAddress ip)
        {
            return
                AppConfiguration.AccessControl.AllowedIpAddresses.Length == 0 && AppConfiguration.AccessControl.AllowedNetworks.Length == 0
                ;
        }
        /// <summary>                                                                                                                                                         
        /// 明示的もしくは暗黙的に許可されている                                                                                                                                                
        /// </summary>                                                                                                                                                        
        /// <param name="ip"></param>                                                                                                                                         
        /// <returns></returns>                                                                                                                                               
        private bool allowed(IPAddress ip)
        {
            return explicitDenied(ip) || implicitAllowed(ip);
        }
        /// <summary>                                                                                                                                                         
        /// 明示的に拒否されている                                                                                                                                                       
        /// </summary>                                                                                                                                                        
        /// <param name="ip"></param>                                                                                                                                         
        /// <returns></returns>                                                                                                                                               
        private bool explicitDenied(IPAddress ip)
        {
            return
                AppConfiguration.AccessControl.DeniedIpAddresses.Contains(ip) || AppConfiguration.AccessControl.DeniedNetworks.Any(net => net.Contains(ip))
                ;
        }

        /// <summary>                                                                                                                                                         
        /// 内部アクセスなら常に許可される                                                                                                                                                   
        /// 内部アクセスでない場合、アクセス許可されていてかつ拒否されていない場合のみ真となる                                                                                                                         
        /// </summary>                                                                                                                                                        
        /// <param name="clientIp"></param>                                                                                                                                   
        /// <returns></returns>                                                                                                                                               
        public bool Allow(IPAddress clientIp)
        {
            return
                AppConfiguration.AccessControl.InsiderIpAddresses.Contains(clientIp) ||
                (allowed(clientIp) && explicitDenied(clientIp) == false)
                ;
        }
        #endregion
    }
    #endregion

    #region AppSetting
    /// <summary>
    /// アプリの設定
    /// </summary>
    public static class AppConfiguration
    {
        /// <summary>
        /// JWT Bearerの鍵情報
        /// </summary>
        public static JwtSecretKey JwtSecret { get; set; }
        /// <summary>
        /// アクセス制御
        /// </summary>
        public static AccessControl AccessControl { get; set; }
    }
    #endregion

    #region SwaggerUIへのJwt対応
    /// <summary>
    /// Jwt認証要求クラス
    /// </summary>
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

    /// <summary>
    /// アプリのStartup
    /// </summary>
    public class Startup
    {
        #region プロパティ
        /// <summary>
        /// appsettings.jsonそのもの.
        /// コンストラクタの引数として渡ってくるので保持するため
        /// </summary>
        public IConfiguration Configuration { get; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            DataConnection.DefaultSettings = new DbSettings(Configuration);
            AppConfiguration.JwtSecret = new JwtSecretKey(Configuration);
            AppConfiguration.AccessControl = new AccessControl(Configuration);

            // Check Client IP Address
            services.AddScoped<ClientIpCheckFilter>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Add to support Authentication With Jwt
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Audience = AppConfiguration.JwtSecret.SiteUri;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = AppConfiguration.JwtSecret.SiteUri,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.JwtSecret.SecretKey))
                };
            });
            #endregion

            #region Add to support Authorization by using PermissionType
            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionTypePolicyProvider>();

            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddSingleton<IAuthorizationHandler, PermissionTypeHandler>();

            //// PermissionTypeHandlerのコンストラクタが引数にIHttpContextAccessorを要求するため
            services.AddHttpContextAccessor();
            #endregion

            #region Add to support Swagger UI
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
            #endregion
        }

        private string XmlCommentsPath => Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML");

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

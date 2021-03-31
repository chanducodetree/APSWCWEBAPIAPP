using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APSWCWEBAPIAPP.DBConnection;
using APSWCWEBAPIAPP.Models;
using AuthService;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelService;

namespace APSWCWEBAPIAPP
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
            //services.AddControllers();
                        services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<ApplicationAPSWCCDbContext>(context => { context.UseInMemoryDatabase("ApswcCaptcha").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); });
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddScoped<SqlCon>();
            services.AddTransient<ICaptchaService, CaptchaService>();
            /*---------------------------------------------------------------------------------------------------*/
            /*                                 JWT AUTHENTICATION SERVICE                                        */

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.RequireHttpsMetadata = false;
       options.SaveToken = true;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = Configuration["Jwt:Issuer"],
           ValidAudience = Configuration["Jwt:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
           ClockSkew = TimeSpan.Zero
       };
       
   });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });


            services.AddCors(options => {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });

            /*---------------------------------------------------------------------------------------------------*/
            /*                              ENABLE API Versioning                                                */
            /*---------------------------------------------------------------------------------------------------*/
            /*   services.AddApiVersioning(
                 options =>
                 {
                     options.ReportApiVersions = true;
                     options.AssumeDefaultVersionWhenUnspecified = true;
                     options.DefaultApiVersion = new ApiVersion(1, 0);
                 });

              services.Configure<FormOptions>(o => {
                  o.ValueLengthLimit = int.MaxValue;
                  o.MultipartBodyLengthLimit = int.MaxValue;
                  o.MemoryBufferThreshold = int.MaxValue;
              }); */

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

        }

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  if (env.IsDevelopment())
  {
      app.UseDeveloperExceptionPage();
  }

  app.UseHttpsRedirection();
  app.UseCors("EnableCORS");

  app.UseRouting();
  app.UseAuthentication();
  app.UseAuthorization();
          
  app.UseStaticFiles(new StaticFileOptions()
  {
      FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Inspection")),
      RequestPath = new PathString("/Inspection")
  });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
               // context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
                await next();
            });

            /*app.Use(nextDelegate => context =>
            {
                string path = context.Request.Path.Value.ToLower();
                string[] directUrls = { "/admin", "/store", "/cart", "checkout", "/login" };
                if (path.StartsWith("/swagger") || path.StartsWith("/api") || string.Equals("/", path) || directUrls.Any(url => path.StartsWith(url)))
                {
                    AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions()
                    {
                        HttpOnly = false,
                        Secure = false,
                        IsEssential = true,
                        SameSite = SameSiteMode.Strict
                    });

                }

                return nextDelegate(context);
            });
            */

            app.UseEndpoints(endpoints =>
  {
      endpoints.MapControllerRoute(
           name: "default",
           pattern: "{controller}/{action=Index}/{id?}");
  });
}
}
}

using IdentityProject.Authorization.Handlers;
using IdentityProject.Authorization.Requirements;
using IdentityProject.Data;
using IdentityProject.Middlewares;
using IdentityProject.Models;
using IdentityProject.Services;
using IdentityProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace IdentityProject
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddUserStore<CustomUserStore>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CoolName", policy => policy.Requirements.Add(new CoolNameRequirement("Vladislav")));
            });

            services.AddScoped<IAuthorizationHandler, CoolNameHandler>();

            services.AddScoped<RoleManager<CustomRole>>();

            services.AddScoped<IRoleStore<CustomRole>, CustomRoleStore>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<TokenMiddleware>();

            services.AddHostedService<TimedHostedService>();

            services.AddHealthChecks()
                .AddCheck("Foo", () =>
                    HealthCheckResult.Healthy("Foo is OK!"), tags: new[] { "foo_tag" })
                .AddCheck("Bar", () =>
                    HealthCheckResult.Unhealthy("Bar is unhealthy!"), tags: new[] { "bar_tag" })
                .AddCheck("Baz", () =>
                    HealthCheckResult.Unhealthy("Baz is OK!"), tags: new[] { "baz_tag" });

            services.AddRazorPages();

            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<TokenMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "CustomStaticFiles")),
                RequestPath = "/files"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("foo_tag") || check.Tags.Contains("baz_tag"),
                    ResponseWriter = WriteResponse
                });

                endpoints.MapHealthChecks("/health/bar", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("bar_tag")
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}

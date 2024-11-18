using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BlogDALLibrary.Repositories;
using BlogDALLibrary;

namespace HedonismBlog
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

            var mapperConfig = new MapperConfiguration(v =>
            {
                v.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddControllersWithViews();
            services.AddDbContext<HedonismBlogContext>();

            //services.AddIdentityCore<User>()
            //    .AddRoles<Role>()
            //    .AddEntityFrameworkStores<HedonismBlogContext>();
            //services.AddIdentity<User, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 5;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireDigit = false;
            //})
            //    .AddEntityFrameworkStores<HedonismBlogContext>();

            services.AddAuthentication(options => options.DefaultScheme = "Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContext =>
                        {
                            redirectContext.HttpContext.Response.Redirect("/SignIn");
                            return Task.CompletedTask;
                        }
                    };
                    //options.LoginPath = "/Account/Signin";
                    options.AccessDeniedPath = "/AccessDenied";
                });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.Use(async (httpContext, next) =>
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                try
                {
                    logger.Info($"{httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "Guest"} requested URL: {httpContext.Request.Path}");
                    await next.Invoke();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred: {Message}", ex.Message);
                    throw;
                }
            });

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error500");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error500");
            }
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

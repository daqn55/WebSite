using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Services.DataServices;
using AreaBg.Services.DataServices.Interfaces;
using AreaBg.Web.Areas.Admin.Services;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Interfaces.Services;
using AreaBg.Web.Services;
using Microsoft.Extensions.Hosting;
using GoogleReCaptcha.V3.Interface;
using GoogleReCaptcha.V3;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Autofac;

namespace AreaBg.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddIdentity<MyIdentityUser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                var allowed = option.User.AllowedUserNameCharacters
                  + "юабцдефгхийклмнопярстужвьызшэщчъЮАБЦДЕФГХИЙКЛМНОПЯРСТУЖВЬЫЗШЭЩЧ";
                option.User.AllowedUserNameCharacters = allowed;
                //option.SignIn.RequireConfirmedEmail = true;
            })
                //.AddDefaultTokenProviders()
                //.AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<MyDbContext>();


            services.AddScoped<IRoleSevice, RoleService>();
            services.AddScoped<IProductAdminService, ProductAdminService>();
            services.AddScoped<IProductService, ProductService>();

            //services.AddMvc(o => o.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddControllers();
            services.AddControllersWithViews();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
            services.AddRazorPages(o =>
            {
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Index", "account");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/FavoriteProducts", "account/favorite-products");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/MyOrders", "account/my-orders");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/MyAddresses", "account/my-addresses");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ChangePassword", "account/settings");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/AddAddress", "account/add-address");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/EditAddress", "account/edit-address");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/OrderDetails", "account/order-details");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "account/register");
                o.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "account/login");
            });

            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                options.AccessDeniedPath = "/AccessDeniedPathInfo";
            });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.ConfigureApplicationCookie(o =>
            {
                o.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
        }

        //  Using a custom DI container
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Configure custom container.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            //app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            //app.UseEndpoints(e =>
            //{
            //    //e.MapControllers();

            //    //e.MapControllerRoute(
            //    //    name: "default",
            //    //    pattern: "{controller=Home}/{action=Index}/{id?}");


            //    //e.MapControllerRoute(
            //    //    name: "default",
            //    //    pattern: "{controller=Checkout}/{action=Cart}");
            //    //e.MapControllerRoute(
            //    //    name: "find",
            //    //    pattern: "{controller=Search}/{action=Find}/{id?}");
            //    //e.MapControllerRoute(
            //    //    name: "Search",
            //    //    pattern: "{controller=Search}/{action=Find}/{keyword}");
            //    //e.MapControllerRoute(
            //    //    name: "by",
            //    //    pattern: "{controller=Search}/{action=By}/category/{id}/{keyword}");
            //    //e.MapControllerRoute(
            //    //    name: "SearchProduct",
            //    //    pattern: "{area:exists}/{controller}/{action}/{keyword}",
            //    //    defaults: new { controller = "Search", action = "Find", keyword = ""});
            //    e.MapRazorPages();

            //    e.MapAreaControllerRoute(
            //        name: "Admin",
            //        areaName: "Admin",
            //        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

            //    e.MapDefaultControllerRoute();


            //});
        }
    }
}

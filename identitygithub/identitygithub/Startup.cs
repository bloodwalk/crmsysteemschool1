using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace PluralsightDemo
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();



            //services.AddDbContext<PluralsightUserDbContext>( // replace "YourDbContext" with the class name of your DbContext
            //   options => options.UseMySql("Server=localhost;port=3306;Database=studentdatabase;User=root;Password=")); // replace with your Connection String

            //var connectionString = @"Data Source= Server = (localdb)\\MSSQLLocalDB; Database = testdatabase4.test; Trusted_Connection = True; MultipleActiveResultSets = true";
            //var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //services.AddDbContext<PluralsightUserDbContext>(opt => opt.UseSqlServer(connectionString,
            //    sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddDbContext<PluralsightUserDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Defaultconnection")));


            services.AddIdentity<PluralsightUser, IdentityRole>(options =>
                {
                    //options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider = "emailconf";

                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 4;

                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<PluralsightUserDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<PluralsightUser>>("emailconf")
                .AddPasswordValidator<DoesNotContainPasswordValidator<PluralsightUser>>();

            services.AddScoped<IUserClaimsPrincipalFactory<PluralsightUser>,
                PluralsightUserClaimsPrincipalFactory>();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(3));
            services.Configure<EmailConfirmationTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(2));

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Login}");
            });
        }
    }
}

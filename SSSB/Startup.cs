using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SSSB.Auth;
using SSSB.Auth.Model;
using SSSB.Data;
using SSSB.Data.Dtos.Auth;
using SSSB.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.Azure; //Namespace for CloudConfigurationManager
//using Microsoft.Azure.Storage;

namespace SSSB
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
            services.AddDbContext<SSSBRestContext>();

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddTransient<IProductCategoriesRepository, ProductCategoriesRepository>();
            services.AddTransient<IAdvertisementsRepository, AdvertisementsRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();




            services.AddTransient<ITokenManager, TokenManager>();
            services.AddIdentity<SSSBUser, IdentityRole>()
                .AddEntityFrameworkStores<SSSBRestContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ValidAudience = Configuration["JWT:ValidAudience"];
                options.TokenValidationParameters.ValidIssuer = Configuration["JWT:ValidIssuer"];
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));
            });
            services.AddTransient<DatabaseSeeder, DatabaseSeeder>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.SameUser, policy => policy.Requirements.Add(new SameUserRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, SameUserAuthorizationHandler>();
            //string connectionString = Configuration.GetConnectionString("SS-SB_db");
            //services.AddDbContextPool<SSSBRestContext>(options =>
            //options.UseSqlServer(connectionString));

            //services.AddDbContext<SSSBRestContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
















            //services.AddDbContext<SSSBRestContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:SS-SB_db"]));

            //services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SSSB", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSSB v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sparky.WebApp.Api.Contexts;
using Sparky.WebApp.Api.Services;
using AspNetCore.Firebase.Authentication;
using AspNetCore.Firebase.Authentication.Extensions;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;

namespace Sparky.WebApp.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddDbContext<SparkyDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionStrings:SparkyDb"]));

            services.AddScoped(typeof(IChargingStationService), typeof(ChargingStationService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IPowerbankService), typeof(PowerbankService));
            services.AddScoped(typeof(ILoanService), typeof(LoanService));

            services.AddFirebaseAuthentication(Configuration["FirebaseAuth:Issuer"], Configuration["FirebaseAuth:Audience"]);

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            //var context = services.BuildServiceProvider().GetService<SparkyDbContext>();
            //context.Database.Migrate();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseCors("AllowOrigin");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

        }
    }
}

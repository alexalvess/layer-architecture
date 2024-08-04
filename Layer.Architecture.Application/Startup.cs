using AutoMapper;
using Layer.Architecture.Application.Models;
using Layer.Architecture.Domain.Entities;
using Layer.Architecture.Domain.Interfaces;
using Layer.Architecture.Infra.Data.Context;
using Layer.Architecture.Infra.Data.Repository;
using Layer.Architecture.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Layer.Architecture.Application
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
            services.AddControllers();

            services.AddDbContext<MySqlContext>(options =>
            {
                var server = Configuration["database:mysql:server"];
                var port = Configuration["database:mysql:port"];
                var database = Configuration["database:mysql:database"];
                var username = Configuration["database:mysql:username"];
                var password = Configuration["database:mysql:password"];
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 21)); // Especifique a versão do servidor MySQL

                options.UseMySql($"Server={server};Port={port};Database={database};Uid={username};Pwd={password}", serverVersion, opt =>
                {
                    opt.CommandTimeout(180);
                    opt.EnableRetryOnFailure(5);
                });
            });

            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IBaseService<User>, BaseService<User>>();

            services.AddSingleton(new MapperConfiguration(config =>
            {
                config.CreateMap<CreateUserModel, User>();
                config.CreateMap<UpdateUserModel, User>();
                config.CreateMap<User, UserModel>();
            }).CreateMapper());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

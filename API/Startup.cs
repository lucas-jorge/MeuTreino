using System;
using System.Linq;
using System.Text;
using API.Entities;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using MySql.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API
{
    public class Startup
    {
        string SwaggerVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddCors();

            // Verifica o provedor de banco de dados configurado no appsettings.json
            var databaseProvider = Configuration["DatabaseProvider"];

            #region Define o tipo de banco de dados
            switch (databaseProvider)
            {
                case "":
                case "InMemory":
                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
                    break;
                case "Sqlite":
                    string DbPath = System.IO.Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Database.db");
                    services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={DbPath}"));
                    break;
                case "MySQL":
                    var connectionString = Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString));
                    break;
            }
            #endregion
            services.AddScoped<AppDbContext, AppDbContext>();

            #region Autentication
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0); // Garantir que nada quebrará em versões futuras

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("7f72cd430cb04f3a9e2c03039c03ac09")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            #region Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(SwaggerVersion, new OpenApiInfo { Title = "API", Version = SwaggerVersion });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                                    {
                                        new OpenApiSecurityScheme
                                        {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                        },
                                        new string[] { }
                                        }
                                    });
            });
            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Content-Disposition")
               .SetIsOriginAllowed(origin => true)
               .AllowCredentials());

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{SwaggerVersion}/swagger.json", "API");
                c.RoutePrefix = "api"; // Define o prefixo da UI do Swagger como /api
            });


            #region Verifica se precisa aplicar alguma atualização no banco de dados. Caso positivo, aplica
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

                // Verifica se o contexto é relacional antes de tentar executar migrações
                if (context.Database.IsMySql() || context.Database.IsSqlite())
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
            }
            #endregion
        }

    }
}

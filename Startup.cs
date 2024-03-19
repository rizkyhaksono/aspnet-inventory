using System.Text;
using InventoryItems.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace InventoryItems
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
            var connectionString =
                Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found."
                );
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            services.AddControllers();

            var secretKey = Configuration["JwtSettings:SecretKey"];
            var issuer = Configuration["JwtSettings:Issuer"];

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey)
                        )
                    };
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                });

            services.AddSwaggerGen();

            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

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

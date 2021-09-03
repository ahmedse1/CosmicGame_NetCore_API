using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace API
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
            services.AddControllers();
            services.AddMvc();
            var Issuer = Configuration["AuthToken:Issuer"].ToString();
            var Audience = Configuration["AuthToken:Audience"].ToString();
            
            var vSenderEmail = Configuration["EmailConfiguration:From"].ToString();
            var vSenderEmailPassword = Configuration["EmailConfiguration:Password"].ToString();
            var vSenderEmailSSL = Configuration["EmailConfiguration:SSL"].ToString();
            var vSenderEmailSMTP = Configuration["EmailConfiguration:SmtpServer"].ToString();
            var vSenderEmailPort = Configuration["EmailConfiguration:Port"].ToString();
            GlobalVars.SenderEmail = vSenderEmail;
            GlobalVars.SenderEmailPassword = vSenderEmailPassword;
            GlobalVars.SenderEmailSSL = Convert.ToBoolean(vSenderEmailSSL);
            GlobalVars.SenderEmailSMTP = vSenderEmailSMTP;
            GlobalVars.SenderEmailPort = vSenderEmailPort;

            var key = Encoding.ASCII.GetBytes(GlobalVars.JwtKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                     .SetIsOriginAllowed(_ => true)
                    .AllowCredentials();
            }));
            services.AddSingleton<IConfiguration>(Configuration);
            services.ConfigureDIExtension(Configuration);
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}

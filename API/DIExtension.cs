using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Interface;
using Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.Interface;
using Repository.Implementation;
using Microsoft.AspNetCore.Http;
using API.Controllers;

namespace API
{
    public static class DIExtension
    {
        public static void ConfigureDIExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<JWTAuthentication>();
            services.AddTransient<IDbContext, DbContext>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<ILoginRepo, LoginRepo>();
            services.AddTransient<ICommonRepo, CommonRepo>();
            services.AddTransient<IChartHolderService, ChartHolderService>();
            services.AddTransient<IChartHolderRepo, ChartHolderRepo>();
            services.AddTransient<IAstroChartService, AstroChartService>();
            services.AddTransient<IAstroChartRepo, AstroChartRepo>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IBookRepo, BookRepo>();



        }
    }
}

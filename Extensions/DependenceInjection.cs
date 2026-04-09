using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Application.Services;
using Infra.Repositories;

namespace CodeElevate.Extensions
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           
            services.AddScoped<IProductService, ProductService>();

            
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
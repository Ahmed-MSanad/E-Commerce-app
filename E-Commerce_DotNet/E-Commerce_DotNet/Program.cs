using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Services.Abstraction;
using Services.Implementation;

namespace E_Commerce_DotNet
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce", Version = "v1" });
            });

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDB"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddAutoMapper(cfg =>
            {
                //cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
                cfg.AddMaps(typeof(ServiceManager).Assembly);
            });


            var app = builder.Build();

            await SeedStoreDbAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce v1"));
            }

            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            });

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        public static async Task SeedStoreDbAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbInitializer.InitializeStoreDbAsync();
        }
    }
}

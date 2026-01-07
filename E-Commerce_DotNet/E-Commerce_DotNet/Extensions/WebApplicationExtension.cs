using Domain.Contracts;

namespace E_Commerce_DotNet.Extensions
{
    public static class WebApplicationExtension
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbInitializer.InitializeStoreDbAsync();

            await dbInitializer.InitializeIdentityStoreDbAsync();

            return app;
        }
    }
}

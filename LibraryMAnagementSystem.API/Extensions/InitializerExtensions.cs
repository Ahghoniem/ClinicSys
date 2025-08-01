namespace LibraryMAnagementSystem.API.Extensions
{

    public static class InitializerExtensions
    {
        //public static async Task<WebApplication> initStoreContextAsync(this WebApplication webApplication)
        //{
        //    using var scope = webApplication.Services.CreateAsyncScope();
        //    var services = scope.ServiceProvider;

        //    var storeContextInit = services.GetRequiredService<IStoreDbInitializer>();
        //    var storeIdentityContextInit = services.GetRequiredService<IStoreIdentityDbIntializer>();

        //    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        //    try
        //    {
        //        await storeContextInit.InitializeAsync();
        //        await storeContextInit.SeedAsync();


        //        await storeIdentityContextInit.InitializeAsync();
        //        await storeIdentityContextInit.SeedAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        var logger = loggerFactory.CreateLogger<Program>();
        //        logger.LogError(ex, "an Error occurred when applying the Migration Or the Data Seeding");
        //    }
        //    finally
        //    {
        //        await scope.DisposeAsync();
        //    }

        //    return webApplication;
        //}
    }

}

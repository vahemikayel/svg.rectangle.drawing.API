using SVG.API;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args)
            .Build()
            .Run();
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                optional: true,
                reloadOnChange: true);
            config.AddEnvironmentVariables();
        });

        //builder.Services.AddControllers();
        //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        builder.ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

        return builder;
    }
}
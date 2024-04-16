using UserCardsAPI.Managers;
using UserCardsAPI.Models.DB.Entity;

public class Program
{
    private static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<DBUserCardsContext>();
        builder.Services.AddCors();

        var app = builder.Build();

        var DBConfiguration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
            .AddJsonFile("DBConfiguration.json", optional: true, reloadOnChange: true)

            .Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseAuthorization();
        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=CardsController}/{action=Index}");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=HoldersController}/{action=Index}");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=LargeDataController}/{action=Index}");

        var config = Config.GetInstance();
        config.SetConnectionString(DBConfiguration);

        app.Run();
    }
}
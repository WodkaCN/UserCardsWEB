using Microsoft.Extensions.Configuration;
using UserCardsDB_DeployTool.Managers;
using UserCardsDB_DeployTool.Models.DB.Entity;

#nullable disable

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("::Инициализация...");
        Console.WriteLine("::Чтение конфигурационного файла");

        var AppConfiguration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Environment.CurrentDirectory, "Configuration"))
            .AddJsonFile("AppConfiguration.json", optional: true, reloadOnChange: true)
            .Build();

        var containerDB = AppConfiguration.GetSection("UserCardsDB").GetSection("ConnectionString").Get<string>();
        var fakesNum = AppConfiguration.GetSection("FakesNumber").Get<int>();

        Console.WriteLine("::Создание экземпляра базы данных");
        using (var db = new DBUserCardsContext(containerDB))
        {
            var generator = new FakeGenerator();

            Console.WriteLine("::Генерация синтетических данных...");
            var fakesList = generator.GenerateFakeData(fakesNum);

            foreach (var fake in fakesList)
            {
                db.CardHolders.Add(fake.CardHolder);
                db.CardsInfo.AddRange(fake.CardsInfo);
                db.SaveChanges();
            }
            
            Console.WriteLine("::База успешно создана");
        }

        Environment.Exit(0);
    }
}
#nullable disable

namespace UserCardsAPI.Managers
{
    public class Config
    {
        public Config() { }

        private static Config _instance;
        private static readonly object _instanceLock = new object();

        public String ConnectionString { get; private set; }

        public String BasePath { get; private set; }

        public void SetConnectionString(IConfiguration configuration)
        {
            ConnectionString = configuration.GetSection("UserCardsDB").GetSection("ConnectionString").Get<string>();
        }

        public static Config GetInstance()
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    _instance ??= new Config();
                }
            }

            return _instance;
        }
    }
}

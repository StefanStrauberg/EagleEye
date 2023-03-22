namespace EagleEye.Infrastructure.DatabaseConfig
{
    internal interface IMongoDBConnection
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

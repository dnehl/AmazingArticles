namespace AmazingArticles.Infrastructure.Persistence.Configurations
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ArticlesConnectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string ArticlesConnectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string TableName { get; set; }
    }
}

namespace Docket.Server.Data
{
    public interface IDocketDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string DocketCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

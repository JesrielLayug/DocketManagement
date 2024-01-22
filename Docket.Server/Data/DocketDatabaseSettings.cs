namespace Docket.Server.Data
{
    public class DocketDatabaseSettings : IDocketDatabaseSettings
    {
        public string UsersCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get ; set; } = string.Empty;
        public string DatabaseName { get; set ; } = string.Empty;
        public string DocketCollectionName { get ; set ; } = string.Empty;
        public string DocketRateCollectionName { get; set; } = string.Empty;
        public string DocketFavoriteCollectionName { get; set; } = string.Empty;
    }
}

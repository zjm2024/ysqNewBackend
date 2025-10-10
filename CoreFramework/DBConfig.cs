
namespace CoreFramework
{
    public static class DBConfig
    {
        public static int DBConnectionTimeOut { get; set; }
        public static string DbName { get; set; }
        public static EProviderType ProviderType { get; set; }
    }

    public enum EProviderType
    {
        None = 0, SQL = 1, Access = 2, MySQL = 3, Oracle = 4
    }
}

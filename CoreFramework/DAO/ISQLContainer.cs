
namespace CoreFramework.DAO
{
    public interface ISQLContainer
    {
        string CommandText { get;set; }
        void AddParameter(object parameter);
        object[] Parameters { get;set; }
    }
}

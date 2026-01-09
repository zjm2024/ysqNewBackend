using System.Collections.Generic;

namespace CoreFramework.DAO
{
    public interface ISelectFilter:ISQLContainer
    {

        ISelectFilter Select(params string[] column);
        
        ISelectFilter Select(List<string> columnList);

        ISelectFilter Top(object rowCount);

        ISelectFilter From(string tableName, string aliasName);

        ISelectFilter From(string tableName);

        //ISelectFilter From(SelectFilter filter_ASTable);

        ISelectFilter From(ISelectFilter filter_ASTable);

        //ISelectFilter From(SelectFilter filter_ASTable, string aliasName);

        ISelectFilter From(ISelectFilter filter_ASTable, string aliasName);
        
        ISelectFilter Join(params string[] joinStrings);
        
        ISelectFilter Join(List<string> joinList);
        
        ISelectFilter Where(string condtion);
        
        ISelectFilter WhereAnd(string condtion);
        
        ISelectFilter WhereOR(string condtion);
        
        ISelectFilter Having(string condtion);
        
        ISelectFilter GroupBy(params string[] columns);

        ISelectFilter OrderBy(params string[] columnsADsc);
        
        ISelectFilter Union(SelectFilter filter);
        
        ISelectFilter Union(string commandText);

        void AddParameter(string name, object value);

    }
}

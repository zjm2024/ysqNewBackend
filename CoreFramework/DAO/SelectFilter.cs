using System.Collections.Generic;


namespace CoreFramework.DAO
{
    public class SelectFilter : ISelectFilter,ISQLContainer
    {
        #region Filed
        protected string _select = " * ";
        protected string _top = string.Empty;
        protected string _from = string.Empty;
        protected string _join = string.Empty;
        protected string _where = string.Empty;
        protected string _having = string.Empty;
        protected string _groupby = string.Empty;
        protected string _orderby = string.Empty;
        protected string _sqlText = string.Empty;
        protected string _union = string.Empty;
        protected string _commandText = string.Empty;
        protected List<object> _dbparameters = new List<object>();
        #endregion

        #region SQL Methods
        public ISelectFilter From(string tableName, string aliasName)
        {
            _from = string.Format(" {0} {1} ", tableName, aliasName);
            return this;
        }

        public ISelectFilter From(string tableName)
        {
            return From(tableName, "");
        }

        //public ISelectFilter From(SelectFilter filter_ASTable)
        //{
        //    return From("(" + filter_ASTable.CommandText + ")", "");
        //}

        public ISelectFilter From(ISelectFilter filter_ASTable)
        {
            return From("(" + filter_ASTable.CommandText + ")", "");
        }

        //public ISelectFilter From(SelectFilter filter_ASTable, string aliasName)
        //{
        //    _dbparameters = new List<object>(filter_ASTable.Parameters);
        //    return From("(" + filter_ASTable.CommandText + ")", aliasName);
        //}

        public ISelectFilter From(ISelectFilter filter_ASTable, string aliasName)
        {
            _dbparameters = new List<object>(filter_ASTable.Parameters);
            return From("(" + filter_ASTable.CommandText + ")", aliasName);
        }

       

        public ISelectFilter Select(params string[] column)
        {
            _select = string.Join(",", column);
            return this;
        }

        public ISelectFilter Select(List<string> columnList)
        {
            return Select(columnList.ToArray());
        }

        public ISelectFilter Top(object rowCount)
        {
            _top = " top " + rowCount.ToString();
            return this;
        }

        public ISelectFilter Join(params string[] joinStrings)
        {
            _join = string.Join(" ", joinStrings);
            return this;
        }

        public ISelectFilter Join(List<string> joinList)
        {
            return Join(joinList.ToArray());
        }

        public ISelectFilter Where(string condtion)
        {
            if (condtion.Trim().Length > 0)
            {
                _where = " where " + condtion;
            }
            return this;
        }

        public ISelectFilter WhereAnd(string condtion)
        {
            if (condtion.Length > 0)
            {
                if (_where.Trim().Length > 0)
                {
                    _where = string.Format("{0} and {1} ", _where, condtion);
                }
                else
                {
                    Where(condtion);
                }
            }
            return this;
        }

        public ISelectFilter WhereOR(string condtion)
        {

            if (_where.Trim().Length > 0)
            {
                _where = string.Format("({0}) or ({1}) ", _where, condtion);
            }
            else
            {
                Where(condtion);
            }

            return this;
        }

        public ISelectFilter Having(string condtion)
        {
            _having = " having " + condtion;
            return this;
        }

        public ISelectFilter GroupBy(params string[] columns)
        {
            _groupby = " group by  " + string.Join(",", columns);
            return this;
        }

        public ISelectFilter OrderBy(params string[] columnsADsc)
        {
            _orderby = " order by " + string.Join(",", columnsADsc);
            return this;
        }

        public ISelectFilter Union(SelectFilter filter)
        {
            return Union(filter.CommandText);
        }

        public ISelectFilter Union(string commandText)
        {
            _union = " union " + commandText;
            return this;
        }

        #endregion

        #region CommonMethod

        public void AddParameter(string name, object value)
        {
            _dbparameters.Add(DbHelper.CreateParameter(name, value));
        }

        public void AddParameter(object Parameter)
        {
            _dbparameters.Add(Parameter);
        }
        #endregion


        #region Property

        public string CommandText
        {
            get
            {
                if (_commandText.Length == 0)
                {
                    _commandText = string.Format(" select {0} {1} from {2} {3} {4} {5} {6} {7} {8}"
                         , _top
                         , _select
                         , _from
                         , _join
                         , _where
                         , _having
                         , _groupby
                         , _orderby
                         , _union
                         );
                    return _commandText;
                }
                else
                {
                    return _commandText;
                }

            }
            set
            {
                _commandText = value;
            }
        }

        public object[] Parameters
        {
            get { return _dbparameters.ToArray(); }
            set { _dbparameters = new List<object>(value); }
        }
        #endregion
    }
}

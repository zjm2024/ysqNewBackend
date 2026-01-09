using System.Collections.Generic;

namespace CoreFramework.DAO
{
    public class SQLContainer:ISQLContainer
    {
        protected string _commandText = string.Empty;
        protected List<object> _parameters = new List<object>();

        public string CommandText
        {
            get { return _commandText; }
            set { _commandText = value; }
        }

        public void AddParameter(object parameter)
        {
            _parameters.Add(parameter);
        }

        public object[] Parameters
        {
            get { return _parameters.ToArray(); }
            set { _parameters = new List<object>(value); }
        }
    }
}

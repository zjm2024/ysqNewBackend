using System;
using System.Collections.Generic;

namespace CoreFramework.VO
{
    public interface ICommonVO : ICloneable
    {
        Dictionary<string, object> OriginData
        {
            get;
            set;
        }

        Dictionary<string, object> ChangeData
        {
            get;
        }

        object GetValue(Type type, string columnName);
        object GetOrginValue(Type type, string columnName);

        void SetValue(string columnName, object value);

        List<string> PropertyList
        {
            get;
        }
    }
}

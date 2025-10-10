using System;
using System.Collections.Generic;
using CoreFramework.DAO;
namespace CoreFramework.VO
{
    [Serializable]
    public abstract class CommonVO : ICommonVO, ICloneable
    {
        protected Dictionary<string, object> originData = new Dictionary<string, object>();
        protected Dictionary<string, object> changeData = new Dictionary<string, object>();


        /// <summary>
        /// while value is DBNull then return the defalut
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object GetDefaultValue(Type type, object value)
        {
            if (value is DBNull || value == null)
            {
                if (type == typeof(String))
                {
                    value = string.Empty;
                }
                else if (type == typeof(Int32))
                {
                    value = 0;
                }
                else if (type == typeof(Int16))
                {
                    value = 0;
                }
                else if (type == typeof(Int64))
                {
                    value = 0;
                }
                else if (type == typeof(Boolean))
                {
                    value = false;
                }
                else if (type == typeof(DateTime))
                {
                    value = new DateTime(1900, 1, 1);
                }
                else if (type == typeof(Decimal))
                {
                    value = Decimal.Zero;
                }
                else if (type == typeof(Byte))
                {
                    value = 0;
                }
                else if (type == typeof(UInt64))
                {
                    value = false;
                }

                return value;
            }
            else
            {
                if(DBConfig.ProviderType == EProviderType.MySQL)
                {
                    if(value.GetType() == typeof(MySql.Data.Types.MySqlDateTime))
                    {
                        value = DateTime.Parse(value.ToString());
                    }
                    else if (value.GetType() == typeof(UInt64))
                    {
                        value = value.ToString() == "1";
                    }
                    else if(value.GetType() == typeof(Int64))
                    {
                        value = Convert.ToInt32(value);
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// Give value by columnname
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object GetValue(Type type, string columnName)
        {
            if (changeData.ContainsKey(columnName))
            {
                return GetDefaultValue(type, changeData[columnName]);
            }
            else if (originData.ContainsKey(columnName))
            {
                return GetDefaultValue(type, originData[columnName]);
            }
            else
            {
                return GetDefaultValue(type, DBNull.Value);
                //return DBNull.Value;
            }
        }

        public object GetOrginValue(Type type, string columnName)
        {
            if (originData.ContainsKey(columnName))
            {
                return GetDefaultValue(type, originData[columnName]);
            }
            else if (changeData.ContainsKey(columnName))
            {
                return GetDefaultValue(type, changeData[columnName]);
            }
            else
            {
                return GetDefaultValue(type, DBNull.Value);
                //return DBNull.Value;
            }
        }

        /// <summary>
        /// Modifly column value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        public void SetValue(string columnName, object value)
        {
            if (value is DateTime)
            {
                value = DbHelper.FixDateTime((DateTime)value);

            }
            ////if (originData.ContainsKey(columnName))
            ////{
            ////    if (originData[columnName] != value)
            ////    {
            ////        changeData[columnName] = value;
            ////    }
            ////}
            ////else
            ////{

            changeData[columnName] = value;

            //}
        }

        public void RestoreAll()
        {
            changeData.Clear();
        }

        public void Restore(string columnName)
        {
            if (changeData.ContainsKey(columnName))
            {
                changeData.Remove(columnName);
            }
        }

        public object this[string index]
        {
            get { return GetValue(typeof(object), index); }
            set { SetValue(index, value.GetType()); }
        }

        /// <summary>
        /// Is column value Change
        /// </summary>
        public bool IsChange(string columnName)
        {
            if (changeData.ContainsKey(columnName))
            {
                return true;
            }
            return false;
        }

        #region IVO Members

        /// <summary>
        /// Origin DataRow value
        /// </summary>
        Dictionary<string, object> ICommonVO.OriginData
        {
            get { return originData; }
            set { originData = value; }
        }

        /// <summary>
        /// the Modiflyed Value
        /// </summary>
        Dictionary<string, object> ICommonVO.ChangeData
        {
            get { return changeData; }
        }

        /// <summary>
        /// VO property list
        /// </summary>
        List<string> ICommonVO.PropertyList
        {
            get { return null; }
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return null;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace CoreFramework.VO
{
    public class VOHelper
    {
        public delegate void BindHandler(object vo, object datarow);

        public delegate void BeforeBindHandler(object vo, object datarow, out VOBindAction action);

        public event BindHandler OnBind;

        public event BeforeBindHandler OnBeforeBind;


        /// <summary>
        /// Get VO property list
        /// </summary>
        /// <param name="voType"></param>
        /// <returns></returns>
        public static List<string> GetVOPropertyList(Type voType)
        {
            return GetVOPropertyList(voType, true);

        }

        /// <summary>
        /// Get VO property list
        /// </summary>
        /// <param name="voType">Type of the vo.</param>
        /// <param name="isMathCase">if set to <c>true</c> [is math case].</param>
        /// <returns></returns>
        public static List<string> GetVOPropertyList(Type voType, bool isMathCase)
        {
            PropertyInfo[] pros = voType.GetProperties();
            List<string> propertyList = new List<string>(pros.Length);
            foreach (PropertyInfo pro in pros)
            {
                if (isMathCase)
                    propertyList.Add(pro.Name);
                else
                    propertyList.Add(pro.Name.ToLower());
            }
            return propertyList;

        }


        public List<T> Bind<T>(IDataReader reader) where T : ICommonVO
        {
            
            ICommonVO tmp = (ICommonVO)Activator.CreateInstance(typeof(T));
            List<T> volist = new List<T>();
            List<string> existColumn = null;
            while (reader.Read())
            {
                VOBindAction action = VOBindAction.Normal;
                ICommonVO vo = tmp.Clone() as ICommonVO;//(ICommonVO)Activator.CreateInstance(_voType);
                if (existColumn == null)
                {
                    int fieldsCount = reader.FieldCount;
                    existColumn = new List<string>();
                    for (int i = 0; i < fieldsCount; i++)
                    {
                        string columnname = reader.GetName(i);
                        if (vo.PropertyList.Contains(columnname))
                        {
                            existColumn.Add(columnname);
                        }
                    }
                }
                if (OnBeforeBind != null)
                {
                    OnBeforeBind(vo, reader, out action);
                }
                if (action == VOBindAction.Skip)
                {
                    continue;
                }
                else if (action == VOBindAction.StopBind)
                {
                    break;
                }
                else
                {
                    foreach (string s in existColumn)
                    {
                        vo.OriginData[s] = reader[s];
                    }
                    if (OnBind != null)
                    {
                        OnBind(vo, reader);
                    }
                    volist.Add((T)vo);
                }
            }
            OnBind = null;
            OnBeforeBind = null;
            return volist;
        }

        public List<T> Bind<T>(DataTable dataTable)
        {
            VOBindAction action = VOBindAction.Normal;
            ICommonVO tmp = (ICommonVO)Activator.CreateInstance(typeof(T));
            DataColumnCollection dcc = dataTable.Columns;
            List<T> volist = new List<T>(dataTable.Rows.Count);
            List<string> existColumn = new List<string>();
            foreach (DataColumn dc in dcc)
            {
                if (tmp.PropertyList.Contains(dc.ColumnName))
                {
                    existColumn.Add(dc.ColumnName);
                }
            }
            foreach (DataRow dr in dataTable.Rows)
            {
                ICommonVO vo = tmp.Clone() as ICommonVO;//(ICommonVO)Activator.CreateInstance(_voType);
                if (OnBeforeBind != null)
                {
                    OnBeforeBind(vo, dr, out action);
                }
                if (action == VOBindAction.Skip)
                {
                    continue;
                }
                else if (action == VOBindAction.StopBind)
                {
                    break;
                }
                else
                {
                    foreach (string s in existColumn)
                    {
                        vo.OriginData[s] = dr[s];
                    }
                    if (OnBind != null)
                    {
                        OnBind(vo, dr);
                    }
                    volist.Add((T)vo);
                }
            }
            OnBind = null;
            OnBeforeBind = null;
            return volist;
        }

        public List<T> BindToEntityVO<T>(IDataReader reader)
        {
            return BindToEntityVO<T>(reader, true);
        }

        public List<T> BindToEntityVO<T>(IDataReader reader, bool isMathCase)
        {
            
            List<string> propertyList = VOHelper.GetVOPropertyList(typeof(T), isMathCase);
            int fieldCount = reader.FieldCount;
            //DataTable dt = reader.GetSchemaTable();
            List<string> existColumn = new List<string>();
            for (int i = 0; i < fieldCount; i++)
            {
                string columnname = reader.GetName(i);//dr["ColumnName"].ToString();
                if (!isMathCase)
                    columnname = columnname.ToLower();
                if (propertyList.Contains(columnname))
                {
                    existColumn.Add(columnname);
                }
            }
            List<T> volist = new List<T>();
            while (reader.Read())
            {
                VOBindAction action = VOBindAction.Normal;
                object vo = Activator.CreateInstance(typeof(T));
                if (OnBeforeBind != null)
                {
                    OnBeforeBind(vo, reader, out action);
                }
                if (action == VOBindAction.Skip)
                {
                    continue;
                }
                else if (action == VOBindAction.StopBind)
                {
                    break;
                }
                else
                {
                    foreach (string s in existColumn)
                    {
                        if (!(reader[s] is DBNull))
                        {
                            Type proptype = vo.GetType().GetProperty(s, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.IgnoreCase).PropertyType;

                            vo.GetType().GetProperty(s, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.IgnoreCase).SetValue(vo, Convert.ChangeType(reader[s], proptype), null);
                        }
                    }
                    if (OnBind != null)
                    {
                        OnBind(vo, reader);
                    }
                    volist.Add((T)vo);
                }

            }
            OnBind = null;
            OnBeforeBind = null;
            return volist;
        }


        public static object DeepClone(Object source)
        {
            //object vo = Activator.CreateInstance(source.GetType());

            //PropertyInfo[] pros = source.GetType().GetProperties();
            //foreach (PropertyInfo pro in pros)
            //{
            //    string name = pro.Name;
            //    object value = pro.GetValue(source, null);
            //    pro.SetValue(vo, value, null);
            //}

            //return vo;
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, source);
            memoryStream.Position = 0;
            return formatter.Deserialize(memoryStream);
        }

        /// <summary>
        /// 复制一个VO用于直接 insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="VO">要复制的VO</param>
        /// <param name="ignoreColumns">不复制的Column名，区分大小写</param>
        /// <returns></returns>
        public static T CreateInsertVO<T>(T VO, params string[] ignoreColumns) where T : ICommonVO
        {
            List<string> igList = new List<string>(ignoreColumns);
            ICommonVO vo = (ICommonVO)Activator.CreateInstance(typeof(T));
            foreach (KeyValuePair<string, object> kv in (VO as ICommonVO).OriginData)
            {
                if (!igList.Contains(kv.Key))
                {
                    vo.OriginData.Add(kv.Key, kv.Value);
                    vo.ChangeData.Add(kv.Key, kv.Value);
                }
            }
            return (T)vo;
        }

        public static void ModiflyVOToInsertVO(ICommonVO vo, params string[] ignoreColumns)
        {
            List<string> igList = new List<string>(ignoreColumns);

            foreach (KeyValuePair<string, object> kv in vo.OriginData)
            {
                if (!igList.Contains(kv.Key))
                {
                    if (!vo.ChangeData.ContainsKey(kv.Key))
                    {
                        vo.ChangeData.Add(kv.Key, kv.Value);
                    }
                }
            }
        }

        public static T ConvertVO<T>(ICommonVO fromVO, bool isIgnoreChange) where T : ICommonVO
        {
            ICommonVO vo = (ICommonVO)Activator.CreateInstance(typeof(T));
            foreach (string key in vo.PropertyList)
            {
                if (fromVO.OriginData.ContainsKey(key))
                {
                    vo.OriginData.Add(key, fromVO.OriginData[key]);

                }
                if (!isIgnoreChange && vo.ChangeData.ContainsKey(key))
                {
                    vo.ChangeData.Add(key, fromVO.ChangeData[key]);
                }
            }
            return (T)vo;
        }

        public static T Compare<T>(ICommonVO oldVO, ICommonVO newVO)
        {
            foreach (string key in oldVO.PropertyList)
            {
                object oldvalue = oldVO.GetValue(typeof(object), key);
                object newvalue = newVO.GetValue(typeof(object), key);
                if (!oldvalue.Equals(newvalue))
                {
                    oldVO.ChangeData.Add(key, newVO.OriginData[key]);
                }
            }
            return (T)oldVO;
        }

        public static string Compare(CommonVO oldVO, CommonVO newVO, Dictionary<string, string> dic)
        {
            string result = "";
            List<string> columnnames = GetVOPropertyList(oldVO.GetType());
            for (int j = 0; j < columnnames.Count; j++)
            {
                string columnname = columnnames[j];
                if (dic.ContainsKey(columnname))
                {
                    object oldvalue = oldVO.GetValue(typeof(object), columnname);
                    object newvalue = newVO.GetValue(typeof(object), columnname);
                    if (!oldvalue.Equals(newvalue))
                    {
                        if (oldvalue.ToString().Contains(":00:00"))
                        {
                            string oldval = GetDate(oldvalue.ToString().Substring(0, oldvalue.ToString().IndexOf(' ')));
                            string newval = GetDate(newvalue.ToString().Substring(0, newvalue.ToString().IndexOf(' ')));
                            result += dic[columnname] + "：" + oldval + "->" + newval + ";";
                        }
                        else
                            result += dic[columnname] + "：" + oldvalue.ToString() + "->" + newvalue.ToString() + ";";
                    }
                }
            }
            return result;
        }

        public static string GetDate(string date)
        {
            string[] dateArr = date.Split('/');
            dateArr[0] = dateArr[0].Substring(2,2);
            if (dateArr[1].Length == 1)
                dateArr[1] = "0" + dateArr[1];
            if (dateArr[2].Length == 1)
                dateArr[2] = "0" + dateArr[2];

            return dateArr[0] + '-' + dateArr[1] + '-' + dateArr[2];
        }
    }

    public enum VOBindAction : int
    {
        StopBind,
        Skip,
        Normal
    }
}

using System;
using System.Collections.Generic;
using CoreFramework.VO;
namespace CoreFramework.DAO
{
    public class InsertFilter
    {
        protected string _tableName = string.Empty;
        protected List<string> _propList;
        protected Type _voType;
        protected string _pkId = string.Empty;
        private bool _isNoUseParameter = false;
        public bool IsNoUseParameter { get { return _isNoUseParameter; } set { _isNoUseParameter = value; } }
        public InsertFilter(string tableName, Type voType, string pkId)
        {
            _tableName = tableName;
            _propList = VOHelper.GetVOPropertyList(voType);
            _voType = voType;
            _pkId = pkId;
        }

        public InsertFilter(string tableName, Type voType)
        {
            _tableName = tableName;
            _propList = VOHelper.GetVOPropertyList(voType);
            _voType = voType;
            _pkId = _propList[0];
        }

        public ISQLContainer Insert<T>(T vo) where T : ICommonVO
        {
            List<T> lst = new List<T>();
            lst.Add(vo);
            ISQLContainer sc = Insert(lst);
            return sc;
        }

        protected ISQLContainer Insert<T>(List<T> voList, int start, int end) where T : ICommonVO
        {
            ISQLContainer _container = new SQLContainer();
            List<object> pars = new List<object>();
            List<string> sqls = new List<string>();
            if (end > voList.Count)
            {
                end = voList.Count;
            }
            for (int i = start; i < end; i++)
            {
                List<string> tmp = new List<string>();
                foreach (string key in voList[i].ChangeData.Keys)
                {
                    if (_propList.Contains(key) && key != _pkId)
                    {
                        tmp.Add(key.Trim());
                    }
                }
                if (tmp.Count > 0)
                {
                    //MySQL不支持[]

                    string columns = DBConfig.ProviderType == EProviderType.MySQL ? "" + string.Join(",", tmp.ToArray()) + "" : "[" + string.Join("],[", tmp.ToArray()) + "]";
                    List<string> parString = new List<string>();
                    foreach (string s in tmp)
                    {
                        if (voList[i].ChangeData[s] == null || voList[i].ChangeData[s].ToString() == "System.DBNull" || voList[i].ChangeData[s].ToString().Length == 0)
                        {
                            parString.Add(" null ");
                        }
                        else
                        {
                            parString.Add("@" + i + s);
                            _container.AddParameter(DbHelper.CreateParameter(i + s, voList[i].ChangeData[s]));
                        }
                    }
                    string sql = string.Format("insert into {0}({1}) values({2})", _tableName, columns, string.Join(",", parString.ToArray()));
                    sqls.Add(sql);

                }
            }
            _container.CommandText = string.Join(";\n", sqls.ToArray());
            return _container;
        }
        protected ISQLContainer InsertNoParameter<T>(List<T> voList, int start, int end) where T : ICommonVO
        {
            ISQLContainer _container = new SQLContainer();
            List<object> pars = new List<object>();
            List<string> sqls = new List<string>();
            if (end > voList.Count)
            {
                end = voList.Count;
            }
            for (int i = start; i < end; i++)
            {
                List<string> tmp = new List<string>();
                foreach (string key in voList[i].ChangeData.Keys)
                {
                    if (_propList.Contains(key) && key != _pkId)
                    {
                        tmp.Add(key.Trim());
                    }
                }
                if (tmp.Count > 0)
                {
                    string columns = DBConfig.ProviderType == EProviderType.MySQL ? "" + string.Join(",", tmp.ToArray()) + "" : "[" + string.Join("],[", tmp.ToArray()) + "]";
                    List<string> parString = new List<string>();
                    foreach (string s in tmp)
                    {
                        if (voList[i].ChangeData[s] == null || voList[i].ChangeData[s].ToString() == "System.DBNull" || voList[i].ChangeData[s].ToString().Length == 0)
                        {
                            parString.Add(" null ");
                        }
                        else
                        {
                            //parString.Add("@" + i + s);
                            //_container.AddParameter(DbHelper.CreateParameter(i + s, voList[i].ChangeData[s]));
                            parString.Add("'" + voList[i].ChangeData[s].ToString() + "'");
                        }
                    }
                    string sql = string.Format("insert into {0}({1}) values({2})", _tableName, columns, string.Join(",", parString.ToArray()));
                    sqls.Add(sql);
                }
            }
            _container.CommandText = string.Join(";\n", sqls.ToArray());
            return _container;
        }

        public ISQLContainer Insert<T>(List<T> voList) where T : ICommonVO
        {
            if (!this.IsNoUseParameter)
            {
                return Insert(voList, 0, voList.Count);
            }
            else
            {
                return InsertNoParameter(voList, 0, voList.Count);
            }
            //return Insert(voList, 0, voList.Count);
        }

        public ISQLContainer[] Insert<T>(List<T> voList, int splitCount) where T : ICommonVO
        {
            List<ISQLContainer> insertList = new List<ISQLContainer>();

            for (int i = 0; i < voList.Count; i = i + splitCount)
            {
                InsertFilter filter = new InsertFilter(this._tableName, _voType, _pkId); // 加上指定的pk属性 Alex Lin 2009-09-16
                insertList.Add(filter.Insert(voList, i, i + splitCount));
            }
            return insertList.ToArray();
        }
    }
}

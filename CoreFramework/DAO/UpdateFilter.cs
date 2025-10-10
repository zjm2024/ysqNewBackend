using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Text.RegularExpressions;

namespace CoreFramework.DAO
{
    public class UpdateFilter
    {
        string _tableName = string.Empty;
        protected List<string> _propList;
        protected Type _voType;
        protected string _pkId = string.Empty;

        public UpdateFilter(string tableName, Type voType, string pkId)
        {
            _tableName = tableName;
            _propList = VOHelper.GetVOPropertyList(voType);
            _voType = voType;
            _pkId = pkId;
        }


        public UpdateFilter(string tableName, Type voType)
        {
            _tableName = tableName;
            _propList = VOHelper.GetVOPropertyList(voType);
            _voType = voType;
            _pkId = _propList[0];
        }


        public ISQLContainer Update<T>(T vo, string condtion, params object[] dbParameters) where T : ICommonVO
        {
            List<T> lst = new List<T>();
            lst.Add(vo);
            ISQLContainer sc = UpdateByParams(lst, condtion);
            foreach (object obj in dbParameters)
            {
                sc.AddParameter(obj);
            }
            return sc;
        }

        public ISQLContainer UpdateByParams<T>(List<T> voList, string condtion, params string[] columnList) where T : ICommonVO
        {
            List<string> lst = new List<string>(columnList);
            return UpdateByParams<T>(voList, condtion, lst);
        }

        public ISQLContainer[] UpdateByParams<T>(List<T> voList, string condtion, int splitCount, params string[] columnList) where T : ICommonVO
        {
            List<string> lst = new List<string>(columnList);
            return UpdateByParams<T>(voList, condtion, lst, splitCount);
        }

        protected ISQLContainer UpdateByParams<T>(List<T> voList, string condtion, List<string> columnList, int start, int end) where T : ICommonVO
        {
            ISQLContainer _container = new SQLContainer();
            List<string> sqls = new List<string>();
            if (end > voList.Count)
            {
                end = voList.Count;
            }
            for (int rowcount = start; rowcount < end; rowcount++)
            {
                ICommonVO vo = voList[rowcount];
                //remove pkid;
                object originPKid = null;
                //if (vo.OriginData.ContainsValue(_propList[0]))
                //{
                if (vo.ChangeData.ContainsKey(_pkId))
                {
                    if (!vo.OriginData.ContainsKey(_pkId) || vo.OriginData[_pkId] == null)
                    {
                        vo.OriginData.Add(_pkId, vo.ChangeData[_pkId]);
                    }
                    originPKid = vo.ChangeData[_pkId];
                    vo.ChangeData.Remove(_pkId);
                }
                //}
                //update Verion
                object origindate = null;
                if (_propList.Contains("UpdatedAt"))
                {

                    if (vo.ChangeData.ContainsKey("UpdatedAt") && vo.ChangeData["UpdatedAt"] != null)
                    {
                        origindate = vo.GetValue(null, "UpdatedAt");
                        vo.ChangeData["UpdatedAt"] = DbHelper.FixDateTime(DateTime.Now);
                    }
                }

                //Set the Update field
                List<string> tmp = new List<string>();


                foreach (KeyValuePair<string, object> k in vo.ChangeData)
                {
                    if (_propList.Contains(k.Key))
                    {
                        if (k.Value == null || k.Value.ToString() == "System.DBNull" || k.Value.ToString().Length == 0)
                        {
                            //MySQL 不支持[]
                            if (DBConfig.ProviderType == EProviderType.MySQL)
                            {
                                tmp.Add(string.Format("{0} = null ", k.Key));
                            }
                            else
                            {
                                tmp.Add(string.Format("[{0}] = null ", k.Key));
                            }
                        }
                        else
                        {

                            _container.AddParameter(DbHelper.CreateParameter(rowcount + k.Key, k.Value));
                            //MySQL 不支持[]
                            if (DBConfig.ProviderType == EProviderType.MySQL)
                            {
                                tmp.Add(string.Format("{0}=@{1}{0}", k.Key, rowcount));
                            }
                            else
                            {
                                tmp.Add(string.Format("[{0}]=@{1}{0}", k.Key, rowcount));
                            }
                        }

                    }
                }

                string parameters = string.Join(",", tmp.ToArray());

                if (parameters.Length == 0)
                {
                    throw new Exception("A VO's Value had not been  Changed!!");
                }

                //define pkid condtion
                string newconditon = condtion + " ";
                foreach (string column in columnList)
                {
                    object v = voList[rowcount].GetOrginValue(typeof(object), column);
                    if (v.ToString() == DBNull.Value.GetType().ToString() || v == DBNull.Value)
                    {
                        string par = column + "[ =<>]{0,}@" + column;
                        newconditon = Regex.Replace(newconditon, par, column + " is null");
                    }
                    else
                    {
                        newconditon = newconditon.Replace("@" + column + " ", "@condtion" + rowcount + column + " ");
                        _container.AddParameter(DbHelper.CreateParameter("condtion" + rowcount + column, vo.GetOrginValue(typeof(object), column)));
                    }

                    //newconditon = newconditon.Replace("@" + column + " ", "@condtion" + rowcount + column + " ");
                    //_container.AddParameter(DbHelper.CreateParameter("condtion" + rowcount + column, vo.GetOrginValue(typeof(object), column)));
                }
                //string condtion = string.Format("{0}=@{1}{0}", idName, rowcount);
                //_container.AddParameter(DbHelper.CreateParameter(rowcount + idName, vo.GetValue(null, idName)));

                //append the  version condtion
                if (_propList.Contains("UpdatedAt"))
                {
                    if (vo.ChangeData.ContainsKey("UpdatedAt") && vo.ChangeData["UpdatedAt"] != null)
                    {
                        ////用于last verison控制
                        //newconditon = newconditon + " and ( datediff(s, UpdatedAt,@" + rowcount + "originUpdatedAt)=0  or UpdatedAt is null )";
                        //_container.AddParameter(DbHelper.CreateParameter(rowcount + "originUpdatedAt ", origindate));
                    }
                }

                sqls.Add(string.Format("update {0} set {1} where {2} ", _tableName, parameters, newconditon));
                //add pkid
                if (originPKid != null)
                {
                    vo.SetValue(_pkId, originPKid);
                }
            }
            _container.CommandText = string.Join("\n", sqls.ToArray());

            return _container;
        }

        #region old code
        //public ISQLContainer UpdateByParams<T>(List<T> voList, string condtion, List<string> columnList) where T : ICommonVO
        //{
        //    ISQLContainer _container = new SQLContainer();
        //    List<string> sqls = new List<string>();
        //    int rowcount = 0;
        //    foreach (ICommonVO vo in voList)
        //    {
        //        //remove pkid;
        //        object originPKid = null;
        //        if (vo.ChangeData.ContainsKey(_propList[0]))
        //        {
        //            originPKid = vo.ChangeData[_propList[0]];
        //            vo.ChangeData.Remove(_propList[0]);
        //        }
        //        //update Verion
        //        object origindate = null;
        //        if (_propList.Contains("UpdatedAt"))
        //        {

        //            if (vo.ChangeData.ContainsKey("UpdatedAt") && vo.ChangeData["UpdatedAt"] != null)
        //            {
        //                origindate = vo.GetValue(null, "UpdatedAt");
        //                vo.ChangeData["UpdatedAt"] = DbHelper.FixDateTime(BOUtilities.GetNow());
        //            }
        //        }

        //        //Set the Update field
        //        List<string> tmp = new List<string>();


        //        foreach (KeyValuePair<string, object> k in vo.ChangeData)
        //        {
        //            if (_propList.Contains(k.Key))
        //            {
        //                _container.AddParameter(DbHelper.CreateParameter(rowcount + k.Key, k.Value));
        //                tmp.Add(string.Format("{0}=@{1}{0}", k.Key, rowcount));

        //            }
        //        }

        //        string parameters = string.Join(",", tmp.ToArray());

        //        if (parameters.Length == 0)
        //        {
        //            throw new Exception("A VO's Value had not been  Changed!!");
        //        }

        //        //define pkid condtion
        //        string newconditon = condtion + " ";
        //        foreach (string column in columnList)
        //        {
        //            newconditon = newconditon.Replace("@" + column + " ", "@condtion" + rowcount + column + " ");
        //            _container.AddParameter(DbHelper.CreateParameter("condtion" + rowcount + column, vo.GetOrginValue(typeof(object), column)));
        //        }
        //        //string condtion = string.Format("{0}=@{1}{0}", idName, rowcount);
        //        //_container.AddParameter(DbHelper.CreateParameter(rowcount + idName, vo.GetValue(null, idName)));

        //        //append the  version condtion
        //        if (_propList.Contains("UpdatedAt"))
        //        {
        //            if (vo.ChangeData.ContainsKey("UpdatedAt") && vo.ChangeData["UpdatedAt"] != null)
        //            {
        //                newconditon = newconditon + " and (UpdatedAt=@" + rowcount + "originUpdatedAt  or UpdatedAt is null )";
        //                _container.AddParameter(DbHelper.CreateParameter(rowcount + "originUpdatedAt ", origindate));
        //            }
        //        }

        //        sqls.Add(string.Format("update {0} set {1} where {2} ", _tableName, parameters, newconditon));
        //        rowcount++;
        //        //add pkid
        //        if (originPKid != null)
        //        {
        //            vo.SetValue(_propList[0], originPKid);
        //        }
        //    }
        //    _container.CommandText = string.Join("\n", sqls.ToArray());

        //    return _container;
        //}
        #endregion

        public ISQLContainer UpdateByParams<T>(List<T> voList, string condtion, List<string> columnList) where T : ICommonVO
        {
            return UpdateByParams(voList, condtion, columnList, 0, voList.Count);
        }

        public ISQLContainer[] UpdateByParams<T>(List<T> voList, string condtion, List<string> columnList, int splitCount) where T : ICommonVO
        {
            List<ISQLContainer> updateList = new List<ISQLContainer>();

            for (int i = 0; i < voList.Count; i = i + splitCount)
            {
                UpdateFilter filter = new UpdateFilter(this._tableName, _voType, _pkId);
                updateList.Add(filter.UpdateByParams(voList, condtion, columnList, i, i + splitCount));
            }
            return updateList.ToArray();
        }
    }
}

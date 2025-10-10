using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreFramework.DAO
{
    public abstract class CommonDAO : ICommonDAO
    {
        protected string _tableName = string.Empty;
        protected string _pkId = string.Empty;
        protected Type _voType;
        protected bool _isCheckActive = false;
        protected int _AccessRightKey = -1;
        protected string _OrderByFieldName = string.Empty;

        protected UserProfile CurrentUserProfile = new UserProfile();

        /// <summary>
        /// To Create a filter for current query criteria for this DAO.
        /// </summary>
        /// <returns></returns>
        public ISelectFilter CreateFiler()
        {
            return new SelectFilter().From(_tableName);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        protected List<T> FindByFilter<T>(ISelectFilter filter) where T : ICommonVO
        {
            if (_isCheckActive)
            {
                // To control whether filter inactive object or not.
                filter.WhereAnd(GetCheckActiveSQL());
            }
            String sql = filter.CommandText;
            if (_AccessRightKey > 0)
            {
                // To control access right for your query statement for integration with security module.
                sql = AddActionEnumSQL(sql);
            }
            return DbHelper.ExecuteVO<T>(sql, filter.Parameters);
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        protected List<T> FindByParams<T>(string condtion, params object[] dbParameters) where T : ICommonVO
        {
            ISelectFilter filter = CreateFiler().Where(condtion);
            filter.Parameters = dbParameters;
            return FindByFilter<T>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns>VO</returns>
        protected T FindById<T>(object id) where T : ICommonVO
        {
            object p = DbHelper.CreateParameter(_pkId, id);
            List<T> list = FindByParams<T>(DbHelper.GetParameterString(_pkId), p);
            return list.Count > 0 ? list[0] : default(T);
        }

        public int CountByParams(string condtion, params object[] dbParameters)
        {
            ISelectFilter filter = CreateFiler().Where(condtion);
            filter.Parameters = dbParameters;
            return CountByFilter(filter);
        }

        public int CountByFilter(ISelectFilter filter)
        {
            filter.Select("count(1)");
            if (_isCheckActive)
            {
                // To control whether filter inactive object or not.
                filter.WhereAnd(GetCheckActiveSQL());
            }
            String sql = filter.CommandText;
            return Convert.ToInt32(DbHelper.ExecuteScalar(sql, filter.Parameters));
        }

        /// <summary>
        /// To Insert a record to DB with VO.
        /// </summary>
        /// <param name="vo">value object</param>
        /// <returns>PK value</returns>
        public int Insert(ICommonVO vo)
        {
            StringBuilder sb = new StringBuilder();
            if (DBConfig.ProviderType == EProviderType.MySQL)
            {
                //MySQL的 Declare只能在存储过程，其它地方无需定义，直接使用
            }
            else
            {
                sb.Append("DECLARE @PKvalue INT ; \n");
            }
            InsertFilter filter = new InsertFilter(_tableName, _voType, _pkId);
            ISQLContainer container = filter.Insert(vo);
            sb.Append(container.CommandText + " ;\n");
            sb.Append(" SET @PKvalue=@@IDENTITY ;\n");
            sb.Append("SELECT @PKvalue as 'identity' ; \n");
            return Convert.ToInt32(DbHelper.ExecuteScalar(sb.ToString(), container.Parameters));
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        protected void InsertList<T>(List<T> voList) where T : ICommonVO
        {
            if (voList.Count < 1)
            {
                return;
            }
            else
            {
                InsertFilter filter = new InsertFilter(_tableName, _voType, _pkId);
                ISQLContainer container = filter.Insert<T>(voList);
                int recordCount = DbHelper.ExecuteNonQuery(container.CommandText, container.Parameters);
                if (recordCount < 1)
                {
                    throw new Exception();
                }
            }
        }
        /// <summary>
        /// To insert records to DB with VO list.
        /// and do not use container.Parameter , somewhere it will have so many parameter that cause problem in DbHelper , such as TimeRecordTransferLogDAO do log time transfer log detail
        /// </summary>
        /// <param name="voList">VO list</param>
        protected void InsertListNoParameter<T>(List<T> voList) where T : ICommonVO
        {
            if (voList.Count < 1)
            {
                return;
            }
            else
            {
                InsertFilter filter = new InsertFilter(_tableName, _voType, _pkId);
                filter.IsNoUseParameter = true;
                ISQLContainer container = filter.Insert<T>(voList);
                int recordCount = DbHelper.ExecuteNonQuery(container.CommandText);
                if (recordCount < 1)
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        protected void InsertList<T>(List<T> voList, int countInEveryRun) where T : ICommonVO
        {
            if (voList.Count < 1)
            {
                return;
            }
            if (voList.Count == 1)
            {
                Insert(voList[0]);
                return;
            }
            else
            {
                int recordCount = 0;
                InsertFilter filter = new InsertFilter(_tableName, _voType, _pkId);
                ISQLContainer[] containers = filter.Insert<T>(voList, countInEveryRun);
                foreach (ISQLContainer c in containers)
                {
                    recordCount += DbHelper.ExecuteNonQuery(c.CommandText, c.Parameters);
                }
                if (recordCount < voList.Count)
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// To Update the specified record to DB with VO by specified key(PK).
        /// </summary>
        /// <param name="vo">value object</param>
        public void UpdateById(ICommonVO vo)
        {
            object pkValue = DBNull.Value;
            if (vo.OriginData.ContainsKey(_pkId))
            {
                pkValue = vo.OriginData[_pkId];
            }
            else if (vo.ChangeData.ContainsKey(_pkId))
            {
                pkValue = vo.ChangeData[_pkId];
            }
            else
            {
                throw new Exception("PKID has not set a value!!");
            }
            pkValue = DbHelper.CreateParameter("OriginPKId", pkValue);
            //MySQL 不支持[]
            string condtion = DBConfig.ProviderType == EProviderType.MySQL ? string.Format("{0}=@OriginPKId", _pkId) : string.Format("[{0}]=@OriginPKId", _pkId);
            UpdateByParams(vo, condtion, pkValue);

        }



        /// <summary>
        /// To Update record(s) to DB with VO, The udpate condtion can be key(PK) or some parameters.
        /// </summary>
        /// <param name="vo">value object</param>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        public void UpdateByParams(ICommonVO vo, string condtion, params object[] dbParameters)
        {
            UpdateFilter filter = new UpdateFilter(_tableName, _voType, _pkId);
            ISQLContainer container = filter.Update(vo, condtion, dbParameters);
            int recordCount = DbHelper.ExecuteNonQuery(container.CommandText, container.Parameters);
            if (recordCount < 1)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        protected void UpdateListById<T>(List<T> voList) where T : ICommonVO
        {
            UpdateListByParams<T>(voList, DbHelper.GetParameterString(_pkId), _pkId);
        }

        protected void UpdateListByParams<T>(List<T> volist, string condition, params string[] columnList) where T : ICommonVO
        {
            List<string> lst = new List<string>(columnList);
            UpdateListByParams(volist, condition, lst);
        }

        protected void UpdateListByParams<T>(List<T> volist, string condition, List<string> columnList) where T : ICommonVO
        {
            UpdateFilter filter = new UpdateFilter(_tableName, _voType, _pkId);
            ISQLContainer container = filter.UpdateByParams<T>(volist, condition, columnList);
            int recordCount = DbHelper.ExecuteNonQuery(container.CommandText, container.Parameters);
            //if (recordCount < volist.Count)
            //{
            //    throw new InvalidVersionException(); //Exception(NOT_MATCH_VERSION);
            //}
        }

        protected void UpdateListByParams<T>(List<T> volist, string condition, List<string> columnList, int countInEveryRun) where T : ICommonVO
        {
            UpdateFilter filter = new UpdateFilter(_tableName, _voType, _pkId);
            ISQLContainer[] containers = filter.UpdateByParams(volist, condition, columnList, countInEveryRun);
            foreach (ISQLContainer c in containers)
            {
                DbHelper.ExecuteNonQuery(c.CommandText, c.Parameters);
            }
        }

        protected void UpdateListByParams<T>(List<T> volist, string condition, int countInEveryRun, params string[] columnList) where T : ICommonVO
        {
            UpdateFilter filter = new UpdateFilter(_tableName, _voType, _pkId);
            ISQLContainer[] containers = filter.UpdateByParams(volist, condition, countInEveryRun, columnList);
            foreach (ISQLContainer c in containers)
            {
                DbHelper.ExecuteNonQuery(c.CommandText, c.Parameters);
            }
        }


        /// <summary>
        /// To Delete a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK</param>
        public void DeleteById(object id)
        {
            DeleteByParams(DbHelper.GetParameterString(_pkId), DbHelper.CreateParameter(_pkId, id));
        }

        /// <summary>
        /// To Delete record(s) to the table with VO, The delete condtion can be key(PK) or some parameters.
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        public void DeleteByParams(string condtion, params object[] dbParameters)
        {
            if (condtion.Trim().Length > 0)
            {
                condtion = " where " + condtion;
            }
            string sql = string.Format("delete from {0} {1}", _tableName, condtion);
            DbHelper.ExecuteNonQuery(sql, dbParameters);
        }

        /// <summary>
        /// To Share a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK</param>
        public void ShareById(object id)
        {
            ShareByParams(DbHelper.GetParameterString(_pkId), DbHelper.CreateParameter(_pkId, id));
        }
        public void ShareByParams(string condtion, params object[] dbParameters)
        {
            if (condtion.Trim().Length > 0)
            {
                condtion = " where " + condtion;
            }
            string sql = string.Format("update {0} set isshare=1 {1}", _tableName, condtion);
            DbHelper.ExecuteNonQuery(sql, dbParameters);
        }

        protected void DeleteListByParams<T>(List<T> voList, string condition, int start, int end, params string[] columnList) where T : ICommonVO
        {
            if (voList.Count == 0)
            {
                return;
            }
            if (end > voList.Count)
            {
                end = voList.Count;
            }
            List<string> sqlList = new List<string>();
            //List<string> realList = new List<string>();
            List<object> parameters = new List<object>();
            for (int i = start; i < end; i++)
            {
                string newcondition = condition + " ";
                //string realcondition = condition + " ";
                foreach (string column in columnList)
                {

                    //realcondition = realcondition.Replace("@" + column , voList[i].GetValue(typeof(object), column).ToString()+" ");

                    object v = voList[i].GetOrginValue(typeof(object), column);
                    if (v.ToString() == DBNull.Value.GetType().ToString() || v == DBNull.Value)
                    {
                        string par = column + "[ =<>]{0,}@" + column;
                        newcondition = Regex.Replace(newcondition, par, column + " is null");
                    }
                    else
                    {
                        newcondition = newcondition.Replace("@" + column + " ", "@condition" + i + column + " ");
                        parameters.Add(DbHelper.CreateParameter("@condition" + i + column, voList[i].GetOrginValue(typeof(object), column)));
                    }

                }
                string tmpsql = string.Format("delete from {0} where {1}", _tableName, newcondition);
                sqlList.Add(tmpsql);
                //tmpsql = string.Format("delete from {0} where {1}", _tableName, realcondition);
                //realList.Add(tmpsql);
            }
            string sql = string.Join("\n", sqlList.ToArray());
            //string realsql = string.Join("\n", realList.ToArray());
            DbHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        protected void DeleteListByParams<T>(List<T> voList, string condition, params string[] columnList) where T : ICommonVO
        {
            DeleteListByParams(voList, condition, 0, voList.Count, columnList);
        }

        protected void DeleteListByParams<T>(List<T> voList, string condition, int splitCount, params string[] columnList) where T : ICommonVO
        {
            for (int i = 0; i < voList.Count; i = i + splitCount)
            {
                DeleteListByParams(voList, condition, i, i + splitCount, columnList);
            }
        }


        /// <summary>
        /// To control access right for your query statement for integration with security module.
        /// </summary>
        /// <param name="accessRightKey">
        /// The value of enum ActionEnum, e.g ActionEnum.Read, ActionEnum.FullControl, ActionEnum.Null,
        /// refer to HTP.Core.Framework.Security.VO.ActionEnum
        /// </param>
        /// <param name="orderByFieldName">The field name of order by</param>
        public void SetAccessRight(object accessRightKey, string orderByFieldName)
        {
            _AccessRightKey = (int)accessRightKey;
            _OrderByFieldName = orderByFieldName;
        }

        #region Property
        /// <summary>
        /// To control whether filter inactive object or not.
        /// </summary>
        public bool IsCheckActive
        {
            get { return _isCheckActive; }
            set { _isCheckActive = value; }
        }
        #endregion


        #region protected Method
        protected string GetCheckActiveSQL()
        {
            string sql = string.Empty;
            ICommonVO vo = (ICommonVO)Activator.CreateInstance(_voType);
            if (vo.PropertyList.Contains("EffectiveStartAt") && vo.PropertyList.Contains("EffectiveEndAt"))
            {
                sql = " ( getUTCDate() between EffectiveStartAt and EffectiveEndAt or (EffectiveStartAt <getUTCDate() and (year(EffectiveEndAt) <=1900 or EffectiveEndAt is null ) ))";
            }
            return sql;
        }

        protected string AddActionEnumSQL(string sql)
        {
            ICommonVO vo = (ICommonVO)Activator.CreateInstance(_voType);
            if (vo.PropertyList.Contains("EntityId") && vo.PropertyList.Contains("TypeId"))
            {
                //IUserGroupDAO accessDAO = SecurityDAOFactory.CreateUserGroupDAO();
                if (string.IsNullOrEmpty(_OrderByFieldName))
                {
                    //sql = accessDAO.ApplySecurityToSql(sql, SecurityContext.CurrentUser.UserProfile.UserId, _AccessRightKey);
                }
                else
                {
                    //sql = accessDAO.ApplySecurityToSql(sql, _OrderByFieldName, SecurityContext.CurrentUser.UserProfile.UserId, _AccessRightKey);
                }
            }
            return sql;
        }

        #region Alex Lin 2009-01-15

        protected DateTime GetNow()
        {
            return DateTime.Now;
        }

        #endregion

        #endregion
                
    }
}

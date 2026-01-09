using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class ConfigDAO:CommonDAO,IConfigDAO
    {
		public ConfigDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_config";
			base._pkId = "ConfigId";
			base._voType = typeof(ConfigVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ConfigVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ConfigVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ConfigVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ConfigVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ConfigVO FindById(object id)
        {
            return base.FindById<ConfigVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ConfigVO> voList)
        {
            base.InsertList<ConfigVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ConfigVO> voList, int countInEveryRun)
        {
            base.InsertList<ConfigVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ConfigVO> voList)
        {
            base.UpdateListById<ConfigVO>(voList);
        }
        
        public void UpdateListByParams(List<ConfigVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ConfigVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ConfigVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ConfigVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ConfigVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ConfigVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ConfigVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ConfigVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ConfigVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ConfigVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ConfigVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ConfigVO>(strSQL, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            int totalCount = 0;
            try
            {
                totalCount = Convert.ToInt32(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = -1;
            }
            return totalCount;
        }
	}
}
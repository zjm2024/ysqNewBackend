using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class HelpDocDAO:CommonDAO,IHelpDocDAO
    {
		public HelpDocDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_helpdoc";
			base._pkId = "HelpDocId";
			base._voType = typeof(HelpDocVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<HelpDocVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<HelpDocVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<HelpDocVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<HelpDocVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public HelpDocVO FindById(object id)
        {
            return base.FindById<HelpDocVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<HelpDocVO> voList)
        {
            base.InsertList<HelpDocVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<HelpDocVO> voList, int countInEveryRun)
        {
            base.InsertList<HelpDocVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<HelpDocVO> voList)
        {
            base.UpdateListById<HelpDocVO>(voList);
        }
        
        public void UpdateListByParams(List<HelpDocVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<HelpDocVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HelpDocVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<HelpDocVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<HelpDocVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<HelpDocVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HelpDocVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<HelpDocVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<HelpDocVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<HelpDocVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<HelpDocVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<HelpDocVO>(strSQL, parameters);
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
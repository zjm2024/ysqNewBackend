using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class HelpDocViewDAO:CommonDAO,IHelpDocViewDAO
    {
		public HelpDocViewDAO(UserProfile userProfile)
		{
			base._tableName="v_helpdocview";
			base._pkId = "HelpDocId";
			base._voType = typeof(HelpDocViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<HelpDocViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<HelpDocViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<HelpDocViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<HelpDocViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public HelpDocViewVO FindById(object id)
        {
            return base.FindById<HelpDocViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<HelpDocViewVO> voList)
        {
            base.InsertList<HelpDocViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<HelpDocViewVO> voList, int countInEveryRun)
        {
            base.InsertList<HelpDocViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<HelpDocViewVO> voList)
        {
            base.UpdateListById<HelpDocViewVO>(voList);
        }
        
        public void UpdateListByParams(List<HelpDocViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<HelpDocViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HelpDocViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<HelpDocViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<HelpDocViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<HelpDocViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HelpDocViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<HelpDocViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<HelpDocViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<HelpDocViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<HelpDocViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<HelpDocViewVO>(strSQL, parameters);
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
using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class ProjectCommissionViewDAO:CommonDAO,IProjectCommissionViewDAO
    {
		public ProjectCommissionViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_platformcommissionview";
			base._pkId = "commissionId";
			base._voType = typeof(PlatformCommissionViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<PlatformCommissionViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<PlatformCommissionViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<PlatformCommissionViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<PlatformCommissionViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public PlatformCommissionViewVO FindById(object id)
        {
            return base.FindById<PlatformCommissionViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<PlatformCommissionViewVO> voList)
        {
            base.InsertList<PlatformCommissionViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<PlatformCommissionViewVO> voList, int countInEveryRun)
        {
            base.InsertList<PlatformCommissionViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<PlatformCommissionViewVO> voList)
        {
            base.UpdateListById<PlatformCommissionViewVO>(voList);
        }
        
        public void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<PlatformCommissionViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<PlatformCommissionViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<PlatformCommissionViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<PlatformCommissionViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<PlatformCommissionViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<PlatformCommissionViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<PlatformCommissionViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<PlatformCommissionViewVO>(strSQL, parameters);
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
using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class MarkServicesViewDAO:CommonDAO,IMarkServicesViewDAO
    {
		public MarkServicesViewDAO(UserProfile userProfile)
		{
			base._tableName="v_markservicesview";
			base._pkId = "MarkCustomerId";
			base._voType = typeof(MarkServicesViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<MarkServicesViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<MarkServicesViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<MarkServicesViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<MarkServicesViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public MarkServicesViewVO FindById(object id)
        {
            return base.FindById<MarkServicesViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<MarkServicesViewVO> voList)
        {
            base.InsertList<MarkServicesViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<MarkServicesViewVO> voList, int countInEveryRun)
        {
            base.InsertList<MarkServicesViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<MarkServicesViewVO> voList)
        {
            base.UpdateListById<MarkServicesViewVO>(voList);
        }
        
        public void UpdateListByParams(List<MarkServicesViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<MarkServicesViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<MarkServicesViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<MarkServicesViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<MarkServicesViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<MarkServicesViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<MarkServicesViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<MarkServicesViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<MarkServicesViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<MarkServicesViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<MarkServicesViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<MarkServicesViewVO>(strSQL, parameters);
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
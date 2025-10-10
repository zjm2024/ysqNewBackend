using System;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial class TenderInfoViewDAO:CommonDAO,ITenderInfoViewDAO
    {
		public TenderInfoViewDAO(UserProfile userProfile)
		{
			base._tableName="v_tenderinfoview";
			base._pkId = "TenderInfoId";
			base._voType = typeof(TenderInfoViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<TenderInfoViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<TenderInfoViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<TenderInfoViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<TenderInfoViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public TenderInfoViewVO FindById(object id)
        {
            return base.FindById<TenderInfoViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<TenderInfoViewVO> voList)
        {
            base.InsertList<TenderInfoViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<TenderInfoViewVO> voList, int countInEveryRun)
        {
            base.InsertList<TenderInfoViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<TenderInfoViewVO> voList)
        {
            base.UpdateListById<TenderInfoViewVO>(voList);
        }
        
        public void UpdateListByParams(List<TenderInfoViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<TenderInfoViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TenderInfoViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<TenderInfoViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<TenderInfoViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<TenderInfoViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TenderInfoViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<TenderInfoViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<TenderInfoViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<TenderInfoViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<TenderInfoViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<TenderInfoViewVO>(strSQL, parameters);
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
using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class BusinessCardViewDAO : CommonDAO, IBusinessCardViewDAO
    {
		public BusinessCardViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_bcbusinessview";
			base._pkId = "BusinessID";
			base._voType = typeof(BusinessCardViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BusinessCardViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BusinessCardViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BusinessCardViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BusinessCardViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BusinessCardViewVO FindById(object id)
        {
            return base.FindById<BusinessCardViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BusinessCardViewVO> voList)
        {
            base.InsertList<BusinessCardViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BusinessCardViewVO> voList, int countInEveryRun)
        {
            base.InsertList<BusinessCardViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BusinessCardViewVO> voList)
        {
            base.UpdateListById<BusinessCardViewVO>(voList);
        }
        
        public void UpdateListByParams(List<BusinessCardViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BusinessCardViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessCardViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BusinessCardViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BusinessCardViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BusinessCardViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessCardViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BusinessCardViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BusinessCardViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BusinessCardViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BusinessCardViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessCardViewVO>(strSQL, parameters);
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
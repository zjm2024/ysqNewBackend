using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class BusinessReviewDetailDAO:CommonDAO,IBusinessReviewDetailDAO
    {
		public BusinessReviewDetailDAO(UserProfile userProfile)
		{
			base._tableName="t_bns_businessreviewdetail";
			base._pkId = "BusinessReviewDetailId";
			base._voType = typeof(BusinessReviewDetailVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BusinessReviewDetailVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BusinessReviewDetailVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BusinessReviewDetailVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BusinessReviewDetailVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BusinessReviewDetailVO FindById(object id)
        {
            return base.FindById<BusinessReviewDetailVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BusinessReviewDetailVO> voList)
        {
            base.InsertList<BusinessReviewDetailVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BusinessReviewDetailVO> voList, int countInEveryRun)
        {
            base.InsertList<BusinessReviewDetailVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BusinessReviewDetailVO> voList)
        {
            base.UpdateListById<BusinessReviewDetailVO>(voList);
        }
        
        public void UpdateListByParams(List<BusinessReviewDetailVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BusinessReviewDetailVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessReviewDetailVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BusinessReviewDetailVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BusinessReviewDetailVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BusinessReviewDetailVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessReviewDetailVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BusinessReviewDetailVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BusinessReviewDetailVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BusinessReviewDetailVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BusinessReviewDetailVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessReviewDetailVO>(strSQL, parameters);
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
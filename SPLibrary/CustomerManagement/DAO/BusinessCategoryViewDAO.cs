using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class BusinessCategoryViewDAO:CommonDAO,IBusinessCategoryViewDAO
    {
		public BusinessCategoryViewDAO(UserProfile userProfile)
		{
			base._tableName="v_businesscategoryview";
			base._pkId = "BusinessCategoryId";
			base._voType = typeof(BusinessCategoryViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BusinessCategoryViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BusinessCategoryViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BusinessCategoryViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BusinessCategoryViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BusinessCategoryViewVO FindById(object id)
        {
            return base.FindById<BusinessCategoryViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BusinessCategoryViewVO> voList)
        {
            base.InsertList<BusinessCategoryViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BusinessCategoryViewVO> voList, int countInEveryRun)
        {
            base.InsertList<BusinessCategoryViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BusinessCategoryViewVO> voList)
        {
            base.UpdateListById<BusinessCategoryViewVO>(voList);
        }
        
        public void UpdateListByParams(List<BusinessCategoryViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BusinessCategoryViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessCategoryViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BusinessCategoryViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BusinessCategoryViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BusinessCategoryViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessCategoryViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BusinessCategoryViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BusinessCategoryViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BusinessCategoryViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BusinessCategoryViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessCategoryViewVO>(strSQL, parameters);
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
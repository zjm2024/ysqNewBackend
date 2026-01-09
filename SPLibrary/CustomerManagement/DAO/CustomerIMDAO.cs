using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CustomerIMDAO:CommonDAO,ICustomerIMDAO
    {
		public CustomerIMDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_customerim";
			base._pkId = "CustomerIMId";
			base._voType = typeof(CustomerIMVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CustomerIMVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CustomerIMVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CustomerIMVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CustomerIMVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CustomerIMVO FindById(object id)
        {
            return base.FindById<CustomerIMVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CustomerIMVO> voList)
        {
            base.InsertList<CustomerIMVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CustomerIMVO> voList, int countInEveryRun)
        {
            base.InsertList<CustomerIMVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CustomerIMVO> voList)
        {
            base.UpdateListById<CustomerIMVO>(voList);
        }
        
        public void UpdateListByParams(List<CustomerIMVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CustomerIMVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerIMVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CustomerIMVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CustomerIMVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CustomerIMVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerIMVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CustomerIMVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CustomerIMVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CustomerIMVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CustomerIMVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CustomerIMVO>(strSQL, parameters);
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
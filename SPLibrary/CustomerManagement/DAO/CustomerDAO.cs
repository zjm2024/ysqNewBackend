using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CustomerDAO:CommonDAO,ICustomerDAO
    {
		public CustomerDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_customer";
			base._pkId = "CustomerId";
			base._voType = typeof(CustomerVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CustomerVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CustomerVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CustomerVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CustomerVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CustomerVO FindById(object id)
        {
            return base.FindById<CustomerVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CustomerVO> voList)
        {
            base.InsertList<CustomerVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CustomerVO> voList, int countInEveryRun)
        {
            base.InsertList<CustomerVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CustomerVO> voList)
        {
            base.UpdateListById<CustomerVO>(voList);
        }
        
        public void UpdateListByParams(List<CustomerVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CustomerVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CustomerVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CustomerVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CustomerVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CustomerVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CustomerVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CustomerVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CustomerVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CustomerVO>(strSQL, parameters);
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

        public string ViewAgencyPhone(int customerId,int agencyCustomerId)
        {
            string strSQL = "";
            strSQL = " call sp_view_agency_phone(" + customerId + "," + agencyCustomerId + "); \n";

            string phone = "";
            try
            {
                phone = Convert.ToString(DbHelper.ExecuteScalar(strSQL));
            }
            catch
            {
                phone = "-1";
            }
            return phone;
        }

        public string ViewBusinessPhone(int customerId, int businessCustomerId)
        {
            string strSQL = "";
            strSQL = " call sp_view_business_phone(" + customerId + "," + businessCustomerId + "); \n";

            string phone = "";
            try
            {
                phone = Convert.ToString(DbHelper.ExecuteScalar(strSQL));
            }
            catch
            {
                phone = "-1";
            }
            return phone;
        }
    }
}
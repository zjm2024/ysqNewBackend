using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class BusinessViewDAO:CommonDAO,IBusinessViewDAO
    {
		public BusinessViewDAO(UserProfile userProfile)
		{
			base._tableName="v_businessview";
			base._pkId = "BusinessId";
			base._voType = typeof(BusinessViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BusinessViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BusinessViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BusinessViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BusinessViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BusinessViewVO FindById(object id)
        {
            return base.FindById<BusinessViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BusinessViewVO> voList)
        {
            base.InsertList<BusinessViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BusinessViewVO> voList, int countInEveryRun)
        {
            base.InsertList<BusinessViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BusinessViewVO> voList)
        {
            base.UpdateListById<BusinessViewVO>(voList);
        }
        
        public void UpdateListByParams(List<BusinessViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BusinessViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BusinessViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BusinessViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BusinessViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BusinessViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BusinessViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BusinessViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BusinessViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessViewVO>(strSQL, parameters);
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

        public List<AgencyViewVO> FindAllMyAgencyByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM (select distinct  c.CustomerId as BusinessCustomerId,a.* From T_CSC_Customer c \n";
            strSQL += " inner join T_PRO_Requirement r on c.CustomerId = r.CustomerId \n";
            strSQL += " inner join T_BNS_Project p on r.RequirementId = p.RequirementId \n";
            strSQL += " inner join V_AgencyView a on p.CustomerId = a.CustomerId) t	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AgencyViewVO>(strSQL, parameters);
        }

        public int FindMyAgencyTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM (select distinct  c.CustomerId as BusinessCustomerId,a.* From T_CSC_Customer c \n";
            strSQL += " inner join T_PRO_Requirement r on c.CustomerId = r.CustomerId \n";
            strSQL += " inner join T_BNS_Project p on r.RequirementId = p.RequirementId \n";
            strSQL += " inner join V_AgencyView a on p.CustomerId = a.CustomerId) t	 \n";
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
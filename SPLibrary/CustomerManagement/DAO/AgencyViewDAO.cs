using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class AgencyViewDAO:CommonDAO,IAgencyViewDAO
    {
		public AgencyViewDAO(UserProfile userProfile)
		{
			base._tableName="v_agencyview";
			base._pkId = "AgencyId";
			base._voType = typeof(AgencyViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AgencyViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AgencyViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AgencyViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AgencyViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AgencyViewVO FindById(object id)
        {
            return base.FindById<AgencyViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AgencyViewVO> voList)
        {
            base.InsertList<AgencyViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AgencyViewVO> voList, int countInEveryRun)
        {
            base.InsertList<AgencyViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AgencyViewVO> voList)
        {
            base.UpdateListById<AgencyViewVO>(voList);
        }
        
        public void UpdateListByParams(List<AgencyViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AgencyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AgencyViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AgencyViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AgencyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AgencyViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AgencyViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AgencyViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AgencyViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AgencyViewVO>(strSQL, parameters);
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

        public List<BusinessViewVO> FindAllMyBusinessByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM (select distinct  c.CustomerId as AgencyCustomerId,b.* From T_CSC_Customer c \n";
            strSQL += " inner join T_BNS_Project p on c.CustomerId = p.CustomerId \n";
            strSQL += " inner join T_PRO_Requirement r on p.Requirementid = r.RequirementId \n";
            strSQL += " inner join V_BusinessView b on r.CustomerId = b.CustomerId) t	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessViewVO>(strSQL, parameters);
        }

        public int FindMyBusinessTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM (select distinct  c.CustomerId as Agency,b.* From T_CSC_Customer c \n";
            strSQL += " inner join T_BNS_Project p on c.CustomerId = p.CustomerId \n";
            strSQL += " inner join T_PRO_Requirement r on p.Requirementid = r.RequirementId \n";
            strSQL += " inner join V_BusinessView b on r.CustomerId = b.CustomerId) t	 \n";
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

        public List<AgencyViewVO> FindMatchAgencyByPageIndex(int requireId,string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL = " call sp_match_agency(" + requireId + "," + start + "," + end + ",'" + sortcolname + "','" + asc + "','" + conditionStr + "'); \n";
            

            return DbHelper.ExecuteVO<AgencyViewVO>(strSQL, parameters);
        }

        public int FindMatchAgencyTotalCount(int requireId, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL = " call sp_match_agency_count(" + requireId + ",'" + condition + "'); \n";

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
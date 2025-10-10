using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class AgencyCategoryDAO:CommonDAO,IAgencyCategoryDAO
    {
		public AgencyCategoryDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_agencycategory";
			base._pkId = "AgencyCategoryId";
			base._voType = typeof(AgencyCategoryVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AgencyCategoryVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AgencyCategoryVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AgencyCategoryVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AgencyCategoryVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AgencyCategoryVO FindById(object id)
        {
            return base.FindById<AgencyCategoryVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AgencyCategoryVO> voList)
        {
            base.InsertList<AgencyCategoryVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AgencyCategoryVO> voList, int countInEveryRun)
        {
            base.InsertList<AgencyCategoryVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AgencyCategoryVO> voList)
        {
            base.UpdateListById<AgencyCategoryVO>(voList);
        }
        
        public void UpdateListByParams(List<AgencyCategoryVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AgencyCategoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyCategoryVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AgencyCategoryVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AgencyCategoryVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AgencyCategoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyCategoryVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AgencyCategoryVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AgencyCategoryVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AgencyCategoryVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AgencyCategoryVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AgencyCategoryVO>(strSQL, parameters);
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

        public string FindAgencyCategoryIds(int agencyId)
        {
            string strSQL = "";
            strSQL += " select CategoryIds from v_agencycategorys	 \n";
            strSQL += " Where \n";
            strSQL += " AgencyId = " + agencyId;
            string str = "";
            try
            {
                str = DbHelper.ExecuteScalar(strSQL).ToString();
                if(str.Length > 0)
                {
                    str = str.Substring(1, str.Length - 1);
                    str = str.Substring(0, str.Length - 1);
                }
            }
            catch
            {
                str = "0";
            }
            return str;
        }
	}
}
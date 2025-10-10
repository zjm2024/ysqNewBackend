using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class AgencyIdCardDAO:CommonDAO,IAgencyIdCardDAO
    {
		public AgencyIdCardDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_agencyidcard";
			base._pkId = "AgencyIDCardId";
			base._voType = typeof(AgencyIdCardVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AgencyIdCardVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AgencyIdCardVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AgencyIdCardVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AgencyIdCardVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AgencyIdCardVO FindById(object id)
        {
            return base.FindById<AgencyIdCardVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AgencyIdCardVO> voList)
        {
            base.InsertList<AgencyIdCardVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AgencyIdCardVO> voList, int countInEveryRun)
        {
            base.InsertList<AgencyIdCardVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AgencyIdCardVO> voList)
        {
            base.UpdateListById<AgencyIdCardVO>(voList);
        }
        
        public void UpdateListByParams(List<AgencyIdCardVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AgencyIdCardVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyIdCardVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AgencyIdCardVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AgencyIdCardVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AgencyIdCardVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyIdCardVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AgencyIdCardVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AgencyIdCardVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AgencyIdCardVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AgencyIdCardVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AgencyIdCardVO>(strSQL, parameters);
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
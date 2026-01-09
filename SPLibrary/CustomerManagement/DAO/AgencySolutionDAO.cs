using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class AgencySolutionDAO:CommonDAO,IAgencySolutionDAO
    {
		public AgencySolutionDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_agencysolution";
			base._pkId = "AgencySolutionId";
			base._voType = typeof(AgencySolutionVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AgencySolutionVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AgencySolutionVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AgencySolutionVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AgencySolutionVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AgencySolutionVO FindById(object id)
        {
            return base.FindById<AgencySolutionVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AgencySolutionVO> voList)
        {
            base.InsertList<AgencySolutionVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AgencySolutionVO> voList, int countInEveryRun)
        {
            base.InsertList<AgencySolutionVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AgencySolutionVO> voList)
        {
            base.UpdateListById<AgencySolutionVO>(voList);
        }
        
        public void UpdateListByParams(List<AgencySolutionVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AgencySolutionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencySolutionVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AgencySolutionVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AgencySolutionVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AgencySolutionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencySolutionVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AgencySolutionVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AgencySolutionVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AgencySolutionVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AgencySolutionVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AgencySolutionVO>(strSQL, parameters);
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
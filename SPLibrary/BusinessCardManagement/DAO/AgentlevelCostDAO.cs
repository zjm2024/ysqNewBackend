using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class AgentlevelCostDAO : CommonDAO, IAgentlevelCostDAO
    {
		public AgentlevelCostDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_agentlevelcost";
			base._pkId = "AgentlevelCostID";
			base._voType = typeof(AgentlevelCostVO);
            base.CurrentUserProfile = userProfile;
        }
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AgentlevelCostVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AgentlevelCostVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AgentlevelCostVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AgentlevelCostVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AgentlevelCostVO FindById(object id)
        {
            return base.FindById<AgentlevelCostVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AgentlevelCostVO> voList)
        {
            base.InsertList<AgentlevelCostVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AgentlevelCostVO> voList, int countInEveryRun)
        {
            base.InsertList<AgentlevelCostVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AgentlevelCostVO> voList)
        {
            base.UpdateListById<AgentlevelCostVO>(voList);
        }
        
        public void UpdateListByParams(List<AgentlevelCostVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AgentlevelCostVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgentlevelCostVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AgentlevelCostVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AgentlevelCostVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AgentlevelCostVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgentlevelCostVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AgentlevelCostVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AgentlevelCostVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AgentlevelCostVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AgentlevelCostVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AgentlevelCostVO>(strSQL, parameters);
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
        public List<AgentlevelCostVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<AgentlevelCostVO>(strSQL, parameters);
        }
        public List<AgentlevelCostVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<AgentlevelCostVO>(strSQL, parameters);
        }
    }
}
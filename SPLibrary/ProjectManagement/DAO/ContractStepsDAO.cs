using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class ContractStepsDAO:CommonDAO,IContractStepsDAO
    {
		public ContractStepsDAO(UserProfile userProfile)
		{
			base._tableName="t_bns_contractsteps";
			base._pkId = "ContractStepsId";
			base._voType = typeof(ContractStepsVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ContractStepsVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ContractStepsVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ContractStepsVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ContractStepsVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ContractStepsVO FindById(object id)
        {
            return base.FindById<ContractStepsVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ContractStepsVO> voList)
        {
            base.InsertList<ContractStepsVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ContractStepsVO> voList, int countInEveryRun)
        {
            base.InsertList<ContractStepsVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ContractStepsVO> voList)
        {
            base.UpdateListById<ContractStepsVO>(voList);
        }
        
        public void UpdateListByParams(List<ContractStepsVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ContractStepsVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ContractStepsVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ContractStepsVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ContractStepsVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ContractStepsVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ContractStepsVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ContractStepsVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ContractStepsVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ContractStepsVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ContractStepsVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ContractStepsVO>(strSQL, parameters);
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
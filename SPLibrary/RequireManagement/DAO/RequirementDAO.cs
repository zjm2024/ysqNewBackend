using System;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial class RequirementDAO:CommonDAO,IRequirementDAO
    {
		public RequirementDAO(UserProfile userProfile)
		{
			base._tableName="t_pro_requirement";
			base._pkId = "RequirementId";
			base._voType = typeof(RequirementVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RequirementVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RequirementVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RequirementVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RequirementVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RequirementVO FindById(object id)
        {
            return base.FindById<RequirementVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RequirementVO> voList)
        {
            base.InsertList<RequirementVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RequirementVO> voList, int countInEveryRun)
        {
            base.InsertList<RequirementVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RequirementVO> voList)
        {
            base.UpdateListById<RequirementVO>(voList);
        }
        
        public void UpdateListByParams(List<RequirementVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RequirementVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequirementVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<RequirementVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<RequirementVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RequirementVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequirementVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RequirementVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<RequirementVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<RequirementVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<RequirementVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RequirementVO>(strSQL, parameters);
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
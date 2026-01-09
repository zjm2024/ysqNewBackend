using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class CompanyDAO:CommonDAO,ICompanyDAO
    {
		public CompanyDAO(UserProfile userProfile)
		{
			base._tableName="T_CSC_Company";
			base._pkId = "CompanyId";
			base._voType = typeof(CompanyVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CompanyVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CompanyVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CompanyVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CompanyVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CompanyVO FindById(object id)
        {
            return base.FindById<CompanyVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CompanyVO> voList)
        {
            base.InsertList<CompanyVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CompanyVO> voList, int countInEveryRun)
        {
            base.InsertList<CompanyVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CompanyVO> voList)
        {
            base.UpdateListById<CompanyVO>(voList);
        }
        
        public void UpdateListByParams(List<CompanyVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CompanyVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CompanyVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CompanyVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CompanyVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CompanyVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CompanyVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CompanyVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CompanyVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CompanyVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CompanyVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";
            
            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();
            
            return DbHelper.ExecuteVO<CompanyVO>(strSQL, parameters);
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
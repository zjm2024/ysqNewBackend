using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class DepartmentDAO : CommonDAO, IDepartmentDAO
    {
		public DepartmentDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_department";
			base._pkId = "DepartmentID";
			base._voType = typeof(DepartmentVO);
            base.CurrentUserProfile = userProfile;
        }
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<DepartmentVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<DepartmentVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<DepartmentVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<DepartmentVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public DepartmentVO FindById(object id)
        {
            return base.FindById<DepartmentVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<DepartmentVO> voList)
        {
            base.InsertList<DepartmentVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<DepartmentVO> voList, int countInEveryRun)
        {
            base.InsertList<DepartmentVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<DepartmentVO> voList)
        {
            base.UpdateListById<DepartmentVO>(voList);
        }
        
        public void UpdateListByParams(List<DepartmentVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<DepartmentVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<DepartmentVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<DepartmentVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<DepartmentVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<DepartmentVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<DepartmentVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<DepartmentVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<DepartmentVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<DepartmentVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<DepartmentVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<DepartmentVO>(strSQL, parameters);
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
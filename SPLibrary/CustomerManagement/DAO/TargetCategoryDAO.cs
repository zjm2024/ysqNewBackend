using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class TargetCategoryDAO:CommonDAO,ITargetCategoryDAO
    {
		public TargetCategoryDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_targetcategory";
			base._pkId = "TargetCategoryId";
			base._voType = typeof(TargetCategoryVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<TargetCategoryVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<TargetCategoryVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<TargetCategoryVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<TargetCategoryVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public TargetCategoryVO FindById(object id)
        {
            return base.FindById<TargetCategoryVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<TargetCategoryVO> voList)
        {
            base.InsertList<TargetCategoryVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<TargetCategoryVO> voList, int countInEveryRun)
        {
            base.InsertList<TargetCategoryVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<TargetCategoryVO> voList)
        {
            base.UpdateListById<TargetCategoryVO>(voList);
        }
        
        public void UpdateListByParams(List<TargetCategoryVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<TargetCategoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TargetCategoryVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<TargetCategoryVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<TargetCategoryVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<TargetCategoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TargetCategoryVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<TargetCategoryVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<TargetCategoryVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<TargetCategoryVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<TargetCategoryVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<TargetCategoryVO>(strSQL, parameters);
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
using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class ComplaintsDAO:CommonDAO,IComplaintsDAO
    {
		public ComplaintsDAO(UserProfile userProfile)
		{
			base._tableName="t_bns_complaints";
			base._pkId = "ComplaintsId";
			base._voType = typeof(ComplaintsVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ComplaintsVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ComplaintsVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ComplaintsVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ComplaintsVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ComplaintsVO FindById(object id)
        {
            return base.FindById<ComplaintsVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ComplaintsVO> voList)
        {
            base.InsertList<ComplaintsVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ComplaintsVO> voList, int countInEveryRun)
        {
            base.InsertList<ComplaintsVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ComplaintsVO> voList)
        {
            base.UpdateListById<ComplaintsVO>(voList);
        }
        
        public void UpdateListByParams(List<ComplaintsVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ComplaintsVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ComplaintsVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ComplaintsVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ComplaintsVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ComplaintsVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ComplaintsVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ComplaintsVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ComplaintsVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ComplaintsVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ComplaintsVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ComplaintsVO>(strSQL, parameters);
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
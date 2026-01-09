using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class JurisdictionDAO : CommonDAO, IJurisdictionDAO
    {
		public JurisdictionDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_Jurisdiction";
			base._pkId = "JurisdictionID";
			base._voType = typeof(JurisdictionVO);
            base.CurrentUserProfile = userProfile;
        }
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<JurisdictionVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<JurisdictionVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<JurisdictionVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<JurisdictionVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public JurisdictionVO FindById(object id)
        {
            return base.FindById<JurisdictionVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<JurisdictionVO> voList)
        {
            base.InsertList<JurisdictionVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<JurisdictionVO> voList, int countInEveryRun)
        {
            base.InsertList<JurisdictionVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<JurisdictionVO> voList)
        {
            base.UpdateListById<JurisdictionVO>(voList);
        }
        
        public void UpdateListByParams(List<JurisdictionVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<JurisdictionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<JurisdictionVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<JurisdictionVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<JurisdictionVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<JurisdictionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<JurisdictionVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<JurisdictionVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<JurisdictionVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<JurisdictionVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<JurisdictionVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<JurisdictionVO>(strSQL, parameters);
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
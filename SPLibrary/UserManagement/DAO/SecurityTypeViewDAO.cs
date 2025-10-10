using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class SecurityTypeViewDAO:CommonDAO,ISecurityTypeViewDAO
    {
		public SecurityTypeViewDAO(UserProfile userProfile)
		{
			base._tableName="V_SecurityType";
			base._pkId = "GroupTypeName";
			base._voType = typeof(SecurityTypeViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<SecurityTypeViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<SecurityTypeViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<SecurityTypeViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<SecurityTypeViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public SecurityTypeViewVO FindById(object id)
        {
            return base.FindById<SecurityTypeViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<SecurityTypeViewVO> voList)
        {
            base.InsertList<SecurityTypeViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<SecurityTypeViewVO> voList, int countInEveryRun)
        {
            base.InsertList<SecurityTypeViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<SecurityTypeViewVO> voList)
        {
            base.UpdateListById<SecurityTypeViewVO>(voList);
        }
        
        public void UpdateListByParams(List<SecurityTypeViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<SecurityTypeViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecurityTypeViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<SecurityTypeViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<SecurityTypeViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<SecurityTypeViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecurityTypeViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<SecurityTypeViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<SecurityTypeViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<SecurityTypeViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<SecurityTypeViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<SecurityTypeViewVO>(strSQL, parameters);
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
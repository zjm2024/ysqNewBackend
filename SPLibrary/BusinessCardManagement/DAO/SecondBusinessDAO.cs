using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class SecondBusinessDAO : CommonDAO, ISecondBusinessDAO
    {
		public SecondBusinessDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_secondbusiness";
			base._pkId = "SecondBusinessID";
			base._voType = typeof(SecondBusinessVO);
            base.CurrentUserProfile = userProfile;
        }
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<SecondBusinessVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<SecondBusinessVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<SecondBusinessVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<SecondBusinessVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public SecondBusinessVO FindById(object id)
        {
            return base.FindById<SecondBusinessVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<SecondBusinessVO> voList)
        {
            base.InsertList<SecondBusinessVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<SecondBusinessVO> voList, int countInEveryRun)
        {
            base.InsertList<SecondBusinessVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<SecondBusinessVO> voList)
        {
            base.UpdateListById<SecondBusinessVO>(voList);
        }
        
        public void UpdateListByParams(List<SecondBusinessVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<SecondBusinessVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecondBusinessVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<SecondBusinessVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<SecondBusinessVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<SecondBusinessVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecondBusinessVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<SecondBusinessVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<SecondBusinessVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<SecondBusinessVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<SecondBusinessVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<SecondBusinessVO>(strSQL, parameters);
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
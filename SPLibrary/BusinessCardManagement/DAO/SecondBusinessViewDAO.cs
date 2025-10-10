using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class SecondBusinessViewDAO : CommonDAO, ISecondBusinessViewDAO
    {
		public SecondBusinessViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_bcsecondbusinessview";
			base._pkId = "SecondBusinessID";
			base._voType = typeof(SecondBusinessViewVO);
            base.CurrentUserProfile = userProfile;
        }
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<SecondBusinessViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<SecondBusinessViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<SecondBusinessViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<SecondBusinessViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public SecondBusinessViewVO FindById(object id)
        {
            return base.FindById<SecondBusinessViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<SecondBusinessViewVO> voList)
        {
            base.InsertList<SecondBusinessViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<SecondBusinessViewVO> voList, int countInEveryRun)
        {
            base.InsertList<SecondBusinessViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<SecondBusinessViewVO> voList)
        {
            base.UpdateListById<SecondBusinessViewVO>(voList);
        }
        
        public void UpdateListByParams(List<SecondBusinessViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<SecondBusinessViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecondBusinessViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<SecondBusinessViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<SecondBusinessViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<SecondBusinessViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecondBusinessViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<SecondBusinessViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<SecondBusinessViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<SecondBusinessViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<SecondBusinessViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<SecondBusinessViewVO>(strSQL, parameters);
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
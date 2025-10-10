using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class HelpDAO : CommonDAO, IHelpDAO
    { 
        public HelpDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_help";
			base._pkId = "HelpID";
			base._voType = typeof(HelpVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<HelpVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<HelpVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<HelpVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<HelpVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public HelpVO FindById(object id)
        {
            return base.FindById<HelpVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<HelpVO> voList)
        {
            base.InsertList<HelpVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<HelpVO> voList, int countInEveryRun)
        {
            base.InsertList<HelpVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<HelpVO> voList)
        {
            base.UpdateListById<HelpVO>(voList);
        }
        
        public void UpdateListByParams(List<HelpVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<HelpVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HelpVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<HelpVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<HelpVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<HelpVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HelpVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<HelpVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<HelpVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<HelpVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<HelpVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<HelpVO>(strSQL, parameters);
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

        public int Update(string data, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " UPDATE  " + this._tableName + " 	 \n";
            strSQL += " SET  " + data + "\n";
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
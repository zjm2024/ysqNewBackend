using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardKeywordViewDAO : CommonDAO, ICardKeywordViewDAO
    { 
        public CardKeywordViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_cardkeywordview";
			base._pkId = "KeywordID";
			base._voType = typeof(CardKeywordViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardKeywordViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardKeywordViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardKeywordViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardKeywordViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardKeywordViewVO FindById(object id)
        {
            return base.FindById<CardKeywordViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardKeywordViewVO> voList)
        {
            base.InsertList<CardKeywordViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardKeywordViewVO> voList, int countInEveryRun)
        {
            base.InsertList<CardKeywordViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardKeywordViewVO> voList)
        {
            base.UpdateListById<CardKeywordViewVO>(voList);
        }
        
        public void UpdateListByParams(List<CardKeywordViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardKeywordViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardKeywordViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardKeywordViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardKeywordViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardKeywordViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardKeywordViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardKeywordViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardKeywordViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardKeywordViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardKeywordViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardKeywordViewVO>(strSQL, parameters);
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
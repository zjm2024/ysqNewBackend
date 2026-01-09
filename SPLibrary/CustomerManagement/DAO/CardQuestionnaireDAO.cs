using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardQuestionnaireDAO : CommonDAO, ICardQuestionnaireDAO
    { 
        public CardQuestionnaireDAO(UserProfile userProfile)
		{
			base._tableName= "t_card_questionnaire";
			base._pkId = "QuestionnaireID";
			base._voType = typeof(CardQuestionnaireVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardQuestionnaireVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardQuestionnaireVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardQuestionnaireVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardQuestionnaireVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardQuestionnaireVO FindById(object id)
        {
            return base.FindById<CardQuestionnaireVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardQuestionnaireVO> voList)
        {
            base.InsertList<CardQuestionnaireVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardQuestionnaireVO> voList, int countInEveryRun)
        {
            base.InsertList<CardQuestionnaireVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardQuestionnaireVO> voList)
        {
            base.UpdateListById<CardQuestionnaireVO>(voList);
        }
        
        public void UpdateListByParams(List<CardQuestionnaireVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardQuestionnaireVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardQuestionnaireVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardQuestionnaireVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardQuestionnaireVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardQuestionnaireVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardQuestionnaireVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardQuestionnaireVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardQuestionnaireVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardQuestionnaireVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardQuestionnaireVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardQuestionnaireVO>(strSQL, parameters);
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
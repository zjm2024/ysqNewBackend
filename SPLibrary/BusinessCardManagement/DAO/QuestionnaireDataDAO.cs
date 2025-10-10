using Baidu.Aip.Speech;
using CoreFramework.DAO;
using CoreFramework.VO;
using NPOI.SS.Formula.Functions;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class QuestionnaireDataDAO : CommonDAO, IQuestionnaireDataDAO
    {
		public QuestionnaireDataDAO(UserProfile userProfile)
		{
			base._tableName= "t_wjx_questionnairedata";
			base._pkId = "QuestionId";
			base._voType = typeof(QuestionnaireDataVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<QuestionnaireDataVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<QuestionnaireDataVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<QuestionnaireDataVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<QuestionnaireDataVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public QuestionnaireDataVO FindById(object id)
        {
            return base.FindById<QuestionnaireDataVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<QuestionnaireDataVO> voList)
        {
            base.InsertList<QuestionnaireDataVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<QuestionnaireDataVO> voList, int countInEveryRun)
        {
            base.InsertList<QuestionnaireDataVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<QuestionnaireDataVO> voList)
        {
            base.UpdateListById<QuestionnaireDataVO>(voList);
        }
        
        public void UpdateListByParams(List<QuestionnaireDataVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<QuestionnaireDataVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<QuestionnaireDataVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<QuestionnaireDataVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<QuestionnaireDataVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<QuestionnaireDataVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<QuestionnaireDataVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<QuestionnaireDataVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<QuestionnaireDataVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<QuestionnaireDataVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<QuestionnaireDataVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<QuestionnaireDataVO>(strSQL, parameters);
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
        public List<QuestionnaireDataVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<QuestionnaireDataVO>(strSQL, parameters);
        }
        public List<QuestionnaireDataVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<QuestionnaireDataVO>(strSQL, parameters);
        }

        public List<QuestionnaireDataVO> FindByFour(int limit)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " order by sort asc " ;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<QuestionnaireDataVO>(strSQL);
        }
    }
}
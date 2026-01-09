using Baidu.Aip.Speech;
using CoreFramework.DAO;
using CoreFramework.VO;
using NPOI.SS.Formula.Functions;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using System;
using System.Collections.Generic;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class AnswerSheetViewDAO : CommonDAO, IAnswerSheetViewDAO
    {
		public AnswerSheetViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_wjx_answersheetview";
			base._pkId = "AnswerSheetId";
			base._voType = typeof(AnswerSheetViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AnswerSheetViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AnswerSheetViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AnswerSheetViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AnswerSheetViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AnswerSheetViewVO FindById(object id)
        {
            return base.FindById<AnswerSheetViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AnswerSheetViewVO> voList)
        {
            base.InsertList<AnswerSheetViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AnswerSheetViewVO> voList, int countInEveryRun)
        {
            base.InsertList<AnswerSheetViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AnswerSheetViewVO> voList)
        {
            base.UpdateListById<AnswerSheetViewVO>(voList);
        }
        
        public void UpdateListByParams(List<AnswerSheetViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AnswerSheetViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AnswerSheetViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AnswerSheetViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AnswerSheetViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AnswerSheetViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AnswerSheetViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AnswerSheetViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AnswerSheetViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AnswerSheetViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AnswerSheetViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            try
            {
                string strSQL = "";

                strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
                strSQL += " Where \n";
                strSQL += conditionStr;
                strSQL += " order by " + sortcolname + " " + asc;
                strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

                return DbHelper.ExecuteVO<AnswerSheetViewVO>(strSQL, parameters);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new List<AnswerSheetViewVO>();
            }
           
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
        public List<AnswerSheetViewVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<AnswerSheetViewVO>(strSQL, parameters);
        }
        public List<AnswerSheetViewVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<AnswerSheetViewVO>(strSQL, parameters);
        }

        public List<AnswerSheetViewVO> FindByFour(int limit)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " order by sort asc " ;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<AnswerSheetViewVO>(strSQL);
        }
    }
}
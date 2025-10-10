using System;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class BCPartySignUpDAO : CommonDAO, IBCPartySignUpDAO
    { 
        public BCPartySignUpDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_partysignup";
			base._pkId = "PartySignUpID";
			base._voType = typeof(BCPartySignUpVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BCPartySignUpVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BCPartySignUpVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BCPartySignUpVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BCPartySignUpVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BCPartySignUpVO FindById(object id)
        {
            return base.FindById<BCPartySignUpVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BCPartySignUpVO> voList)
        {
            base.InsertList<BCPartySignUpVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BCPartySignUpVO> voList, int countInEveryRun)
        {
            base.InsertList<BCPartySignUpVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BCPartySignUpVO> voList)
        {
            base.UpdateListById<BCPartySignUpVO>(voList);
        }
        
        public void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BCPartySignUpVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BCPartySignUpVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BCPartySignUpVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BCPartySignUpVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BCPartySignUpVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BCPartySignUpVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BCPartySignUpVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BCPartySignUpVO>(strSQL, parameters);
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
            LogBO _log = new LogBO(typeof(BCPartySignUpDAO));
            string strErrorMsg = "strSQL:" + strSQL;
            _log.Error(strErrorMsg);

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

        public int FindTotalSum(string sum, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Sum(" + sum + ") FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            int totalCount = 0;
            try
            {
                totalCount = Convert.ToInt32(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = 0;
            }
            return totalCount;
        }
    }
}
using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardPartyOrderViewDAO : CommonDAO, ICardPartyOrderViewDAO
    { 
        public CardPartyOrderViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_cardpartyorderview";
			base._pkId = "PartyOrderID";
			base._voType = typeof(CardPartyOrderViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardPartyOrderViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardPartyOrderViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardPartyOrderViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardPartyOrderViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardPartyOrderViewVO FindById(object id)
        {
            return base.FindById<CardPartyOrderViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardPartyOrderViewVO> voList)
        {
            base.InsertList<CardPartyOrderViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardPartyOrderViewVO> voList, int countInEveryRun)
        {
            base.InsertList<CardPartyOrderViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardPartyOrderViewVO> voList)
        {
            base.UpdateListById<CardPartyOrderViewVO>(voList);
        }
        
        public void UpdateListByParams(List<CardPartyOrderViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardPartyOrderViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardPartyOrderViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardPartyOrderViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardPartyOrderViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardPartyOrderViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardPartyOrderViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardPartyOrderViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardPartyOrderViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardPartyOrderViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardPartyOrderViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardPartyOrderViewVO>(strSQL, parameters);
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

        public decimal FindSumCost(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select sum(Cost) FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            decimal totalCount = 0;
            try
            {
                totalCount = Convert.ToDecimal(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = 0;
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
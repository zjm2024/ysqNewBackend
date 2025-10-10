using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardPartyViewViewDAO : CommonDAO, ICardPartyViewViewDAO
    { 
        public CardPartyViewViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_cardpartyviewview";
			base._pkId = "PartyID";
			base._voType = typeof(CardPartyViewViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardPartyViewViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardPartyViewViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardPartyViewViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardPartyViewViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardPartyViewViewVO FindById(object id)
        {
            return base.FindById<CardPartyViewViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardPartyViewViewVO> voList)
        {
            base.InsertList<CardPartyViewViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardPartyViewViewVO> voList, int countInEveryRun)
        {
            base.InsertList<CardPartyViewViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardPartyViewViewVO> voList)
        {
            base.UpdateListById<CardPartyViewViewVO>(voList);
        }
        
        public void UpdateListByParams(List<CardPartyViewViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardPartyViewViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardPartyViewViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardPartyViewViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardPartyViewViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardPartyViewViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardPartyViewViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardPartyViewViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardPartyViewViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardPartyViewViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardPartyViewViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardPartyViewViewVO>(strSQL, parameters);
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
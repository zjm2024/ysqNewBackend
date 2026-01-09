using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardSubscriptionDAO : CommonDAO, ICardSubscriptionDAO
    { 
        public CardSubscriptionDAO(UserProfile userProfile)
		{
			base._tableName= "t_card_subscription";
			base._pkId = "SubscriptionID";
			base._voType = typeof(SubscriptionVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<SubscriptionVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<SubscriptionVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<SubscriptionVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<SubscriptionVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public SubscriptionVO FindById(object id)
        {
            return base.FindById<SubscriptionVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<SubscriptionVO> voList)
        {
            base.InsertList<SubscriptionVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<SubscriptionVO> voList, int countInEveryRun)
        {
            base.InsertList<SubscriptionVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<SubscriptionVO> voList)
        {
            base.UpdateListById<SubscriptionVO>(voList);
        }
        
        public void UpdateListByParams(List<SubscriptionVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<SubscriptionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SubscriptionVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<SubscriptionVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<SubscriptionVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<SubscriptionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SubscriptionVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<SubscriptionVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<SubscriptionVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<SubscriptionVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<SubscriptionVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<SubscriptionVO>(strSQL, parameters);
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
using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework.Logging.BO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class OrderGroupBuyViewDAO : CommonDAO, IOrderGroupBuyViewDAO
    { 
        public OrderGroupBuyViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_bcordergroupbuyview";
			base._pkId = "OrderID";
			base._voType = typeof(OrderGroupBuyViewVO);
            base.CurrentUserProfile = userProfile;
        }
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<OrderGroupBuyViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<OrderGroupBuyViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<OrderGroupBuyViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<OrderGroupBuyViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public OrderGroupBuyViewVO FindById(object id)
        {
            return base.FindById<OrderGroupBuyViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<OrderGroupBuyViewVO> voList)
        {
            base.InsertList<OrderGroupBuyViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<OrderGroupBuyViewVO> voList, int countInEveryRun)
        {
            base.InsertList<OrderGroupBuyViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<OrderGroupBuyViewVO> voList)
        {
            base.UpdateListById<OrderGroupBuyViewVO>(voList);
        }
        
        public void UpdateListByParams(List<OrderGroupBuyViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<OrderGroupBuyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<OrderGroupBuyViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<OrderGroupBuyViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<OrderGroupBuyViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<OrderGroupBuyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<OrderGroupBuyViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<OrderGroupBuyViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<OrderGroupBuyViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<OrderGroupBuyViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<OrderGroupBuyViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<OrderGroupBuyViewVO>(strSQL, parameters);
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
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(OrderGroupBuyViewDAO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);

                totalCount = -1;
            }
            return totalCount;
        }
        public List<OrderGroupBuyViewVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<OrderGroupBuyViewVO>(strSQL, parameters);
        }
        public List<OrderGroupBuyViewVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<OrderGroupBuyViewVO>(strSQL, parameters);
        }
    }
}
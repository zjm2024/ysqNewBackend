using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardAccessRecordsViewDAO : CommonDAO, ICardAccessRecordsViewDAO
    {
		public CardAccessRecordsViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_card_accessrecordsview";
			base._pkId = "AccessRecordsID";
			base._voType = typeof(CardAccessRecordsViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardAccessRecordsViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardAccessRecordsViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardAccessRecordsViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardAccessRecordsViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardAccessRecordsViewVO FindById(object id)
        {
            return base.FindById<CardAccessRecordsViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardAccessRecordsViewVO> voList)
        {
            base.InsertList<CardAccessRecordsViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardAccessRecordsViewVO> voList, int countInEveryRun)
        {
            base.InsertList<CardAccessRecordsViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardAccessRecordsViewVO> voList)
        {
            base.UpdateListById<CardAccessRecordsViewVO>(voList);
        }
        
        public void UpdateListByParams(List<CardAccessRecordsViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardAccessRecordsViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardAccessRecordsViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardAccessRecordsViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardAccessRecordsViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardAccessRecordsViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardAccessRecordsViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardAccessRecordsViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardAccessRecordsViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardAccessRecordsViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardAccessRecordsViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardAccessRecordsViewVO>(strSQL, parameters);
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

        public List<CardAccessRecordsViewVO> FindCardList(string condition, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            strSQL += " order by DefaultCard desc,CreatedAt asc";

            return DbHelper.ExecuteVO<CardAccessRecordsViewVO>(strSQL, parameters);
        }

        public int UpdateCustomerId(int CustomerId,string OpenID,int AppType, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " UPDATE " + this._tableName + " SET CustomerId="+ CustomerId + " \n";
            strSQL += " Where \n";
            strSQL += "OpenID='"+ OpenID + "' and CustomerId=0 and AppType="+ AppType;
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
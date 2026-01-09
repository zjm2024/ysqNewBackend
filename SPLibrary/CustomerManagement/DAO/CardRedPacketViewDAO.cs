using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardRedPacketViewDAO : CommonDAO, ICardRedPacketViewDAO
    {
        public CardRedPacketViewDAO(UserProfile userProfile)
        {
            base._tableName = "v_card_ad_redpacketview";
            base._pkId = "RedPacketId";
            base._voType = typeof(CardRedPacketViewVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardRedPacketViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardRedPacketViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardRedPacketViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardRedPacketViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardRedPacketViewVO FindById(object id)
        {
            return base.FindById<CardRedPacketViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardRedPacketViewVO> voList)
        {
            base.InsertList<CardRedPacketViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardRedPacketViewVO> voList, int countInEveryRun)
        {
            base.InsertList<CardRedPacketViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardRedPacketViewVO> voList)
        {
            base.UpdateListById<CardRedPacketViewVO>(voList);
        }

        public void UpdateListByParams(List<CardRedPacketViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardRedPacketViewVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<CardRedPacketViewVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<CardRedPacketViewVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<CardRedPacketViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardRedPacketViewVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<CardRedPacketViewVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardRedPacketViewVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<CardRedPacketViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<CardRedPacketViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<CardRedPacketViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardRedPacketViewVO>(strSQL, parameters);
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

        public List<CardRedPacketViewVO> FindCardList(string condition, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            strSQL += " order by RPCreateDate desc";

            return DbHelper.ExecuteVO<CardRedPacketViewVO>(strSQL, parameters);
        }
    }
}
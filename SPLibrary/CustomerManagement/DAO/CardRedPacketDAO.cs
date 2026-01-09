using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardRedPacketDAO : CommonDAO, ICardRedPacketDAO
    {
        public CardRedPacketDAO(UserProfile userProfile)
        {
            base._tableName = "t_card_ad_redpacket";
            base._pkId = "RedPacketId";
            base._voType = typeof(CardRedPacketVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardRedPacketVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardRedPacketVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardRedPacketVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardRedPacketVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardRedPacketVO FindById(object id)
        {
            return base.FindById<CardRedPacketVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardRedPacketVO> voList)
        {
            base.InsertList<CardRedPacketVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardRedPacketVO> voList, int countInEveryRun)
        {
            base.InsertList<CardRedPacketVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardRedPacketVO> voList)
        {
            base.UpdateListById<CardRedPacketVO>(voList);
        }

        public void UpdateListByParams(List<CardRedPacketVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardRedPacketVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<CardRedPacketVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<CardRedPacketVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<CardRedPacketVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardRedPacketVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<CardRedPacketVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardRedPacketVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<CardRedPacketVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<CardRedPacketVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<CardRedPacketVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardRedPacketVO>(strSQL, parameters);
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

        public List<CardRedPacketVO> FindCardList(string condition, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            strSQL += " order by DefaultCard desc,CreatedAt asc";

            return DbHelper.ExecuteVO<CardRedPacketVO>(strSQL, parameters);
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
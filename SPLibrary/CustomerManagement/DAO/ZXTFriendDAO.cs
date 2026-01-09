using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class ZXTFriendDAO : CommonDAO, IZXTFriendDAO
    {
        public ZXTFriendDAO(UserProfile userProfile)
        {
            base._tableName = "t_zxt_Friend";
            base._pkId = "FriendID";
            base._voType = typeof(ZXTFriendVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ZXTFriendVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ZXTFriendVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ZXTFriendVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ZXTFriendVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ZXTFriendVO FindById(object id)
        {
            return base.FindById<ZXTFriendVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ZXTFriendVO> voList)
        {
            base.InsertList<ZXTFriendVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ZXTFriendVO> voList, int countInEveryRun)
        {
            base.InsertList<ZXTFriendVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ZXTFriendVO> voList)
        {
            base.UpdateListById<ZXTFriendVO>(voList);
        }

        public void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ZXTFriendVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<ZXTFriendVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ZXTFriendVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ZXTFriendVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<ZXTFriendVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<ZXTFriendVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<ZXTFriendVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ZXTFriendVO>(strSQL, parameters);
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
    }
}

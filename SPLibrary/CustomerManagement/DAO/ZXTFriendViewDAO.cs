using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class ZXTFriendViewDAO : CommonDAO, IZXTFriendViewDAO
    {
        public ZXTFriendViewDAO(UserProfile userProfile)
        {
            base._tableName = "v_zxt_Friendview";
            base._pkId = "FriendID";
            base._voType = typeof(ZXTFriendViewVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ZXTFriendViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ZXTFriendViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ZXTFriendViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ZXTFriendViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ZXTFriendViewVO FindById(object id)
        {
            return base.FindById<ZXTFriendViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ZXTFriendViewVO> voList)
        {
            base.InsertList<ZXTFriendViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ZXTFriendViewVO> voList, int countInEveryRun)
        {
            base.InsertList<ZXTFriendViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ZXTFriendViewVO> voList)
        {
            base.UpdateListById<ZXTFriendViewVO>(voList);
        }

        public void UpdateListByParams(List<ZXTFriendViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ZXTFriendViewVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ZXTFriendViewVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<ZXTFriendViewVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<ZXTFriendViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ZXTFriendViewVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ZXTFriendViewVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ZXTFriendViewVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<ZXTFriendViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<ZXTFriendViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<ZXTFriendViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ZXTFriendViewVO>(strSQL, parameters);
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

using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class MessageDAO:CommonDAO,IMessageDAO
    {
		public MessageDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_message";
			base._pkId = "MessageId";
			base._voType = typeof(MessageVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<MessageVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<MessageVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<MessageVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<MessageVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public MessageVO FindById(object id)
        {
            return base.FindById<MessageVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<MessageVO> voList)
        {
            base.InsertList<MessageVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<MessageVO> voList, int countInEveryRun)
        {
            base.InsertList<MessageVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<MessageVO> voList)
        {
            base.UpdateListById<MessageVO>(voList);
        }
        
        public void UpdateListByParams(List<MessageVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<MessageVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<MessageVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<MessageVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<MessageVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<MessageVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<MessageVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<MessageVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<MessageVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<MessageVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<MessageVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<MessageVO>(strSQL, parameters);
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
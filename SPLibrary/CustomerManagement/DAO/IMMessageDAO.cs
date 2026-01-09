using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class IMMessageDAO:CommonDAO,IIMMessageDAO
    {
		public IMMessageDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_immessage";
			base._pkId = "IMMessageId";
			base._voType = typeof(IMMessageVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<IMMessageVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<IMMessageVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<IMMessageVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<IMMessageVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public IMMessageVO FindById(object id)
        {
            return base.FindById<IMMessageVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<IMMessageVO> voList)
        {
            base.InsertList<IMMessageVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<IMMessageVO> voList, int countInEveryRun)
        {
            base.InsertList<IMMessageVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<IMMessageVO> voList)
        {
            base.UpdateListById<IMMessageVO>(voList);
        }
        
        public void UpdateListByParams(List<IMMessageVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<IMMessageVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<IMMessageVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<IMMessageVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<IMMessageVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<IMMessageVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<IMMessageVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<IMMessageVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<IMMessageVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<IMMessageVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<IMMessageHistoryVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM ( \n";
            strSQL += "  select c.NickName as username,c.IMId as id,c.HeaderLogo as avatar,UNIX_TIMESTAMP(m.SendAt) as timestamp,m.Message as content from T_CSC_IMMessage m \n";
            strSQL += "  left join T_CSC_CustomerIM c on m.MessageFrom = c.CustomerIMId \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += "  ) t	 \n";            
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<IMMessageHistoryVO>(strSQL, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM ( \n";
            strSQL += "  select c.NickName as username,c.IMId as id,c.HeaderLogo as avatar,UNIX_TIMESTAMP(m.SendAt) as timestamp,m.Message as content from T_CSC_IMMessage m \n";
            strSQL += "  left join T_CSC_CustomerIM c on m.MessageFrom = c.CustomerIMId \n";
            strSQL += " Where \n";
            strSQL += condition;
            strSQL += "  ) t \n";
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
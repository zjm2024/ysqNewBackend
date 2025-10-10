using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using CoreFramework.VO;
using System.Data;

namespace SPLibrary.UserManagement.DAO
{
    public partial class TokenDAO:CommonDAO,ITokenDAO
    {
		public TokenDAO(UserProfile userProfile)
		{
			base._tableName="T_CSC_Token";
			base._pkId = "TokenId";
			base._voType = typeof(TokenVO);
			base.CurrentUserProfile = userProfile;			
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<TokenVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<TokenVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<TokenVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<TokenVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public TokenVO FindById(object id)
        {
            return base.FindById<TokenVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<TokenVO> voList)
        {
            base.InsertList<TokenVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<TokenVO> voList, int countInEveryRun)
        {
            base.InsertList<TokenVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<TokenVO> voList)
        {
            base.UpdateListById<TokenVO>(voList);
        }
        
        public void UpdateListByParams(List<TokenVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<TokenVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TokenVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<TokenVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<TokenVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<TokenVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TokenVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<TokenVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<TokenVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<TokenVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }        

		public List<TokenVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<TokenVO>(strSQL, parameters);
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
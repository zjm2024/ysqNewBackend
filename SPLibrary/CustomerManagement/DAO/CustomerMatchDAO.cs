using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CustomerMatchDAO:CommonDAO,ICustomerMatchDAO
    {
		public CustomerMatchDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_customermatch";
			base._pkId = "CustomerMatchId";
			base._voType = typeof(CustomerMatchVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CustomerMatchVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CustomerMatchVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CustomerMatchVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CustomerMatchVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CustomerMatchVO FindById(object id)
        {
            return base.FindById<CustomerMatchVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CustomerMatchVO> voList)
        {
            base.InsertList<CustomerMatchVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CustomerMatchVO> voList, int countInEveryRun)
        {
            base.InsertList<CustomerMatchVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CustomerMatchVO> voList)
        {
            base.UpdateListById<CustomerMatchVO>(voList);
        }
        
        public void UpdateListByParams(List<CustomerMatchVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CustomerMatchVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerMatchVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CustomerMatchVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CustomerMatchVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CustomerMatchVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerMatchVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CustomerMatchVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CustomerMatchVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CustomerMatchVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CustomerMatchVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CustomerMatchVO>(strSQL, parameters);
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
using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CustomerIMGroupUserDAO:CommonDAO,ICustomerIMGroupUserDAO
    {
		public CustomerIMGroupUserDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_customerimgroupuser";
			base._pkId = "CustomerIMGroupUserId";
			base._voType = typeof(CustomerIMGroupUserVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CustomerIMGroupUserVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CustomerIMGroupUserVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CustomerIMGroupUserVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CustomerIMGroupUserVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CustomerIMGroupUserVO FindById(object id)
        {
            return base.FindById<CustomerIMGroupUserVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CustomerIMGroupUserVO> voList)
        {
            base.InsertList<CustomerIMGroupUserVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CustomerIMGroupUserVO> voList, int countInEveryRun)
        {
            base.InsertList<CustomerIMGroupUserVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CustomerIMGroupUserVO> voList)
        {
            base.UpdateListById<CustomerIMGroupUserVO>(voList);
        }
        
        public void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CustomerIMGroupUserVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CustomerIMGroupUserVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CustomerIMGroupUserVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CustomerIMGroupUserVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CustomerIMGroupUserVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CustomerIMGroupUserVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CustomerIMGroupUserVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CustomerIMGroupUserVO>(strSQL, parameters);
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
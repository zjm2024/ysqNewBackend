using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class BusinessBalanceDAO : CommonDAO, IBusinessBalanceDAO
    {
		public BusinessBalanceDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_balance";
			base._pkId = "BalanceId";
			base._voType = typeof(BusinessBalanceVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BusinessBalanceVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BusinessBalanceVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BusinessBalanceVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BusinessBalanceVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BusinessBalanceVO FindById(object id)
        {
            return base.FindById<BusinessBalanceVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BusinessBalanceVO> voList)
        {
            base.InsertList<BusinessBalanceVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BusinessBalanceVO> voList, int countInEveryRun)
        {
            base.InsertList<BusinessBalanceVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BusinessBalanceVO> voList)
        {
            base.UpdateListById<BusinessBalanceVO>(voList);
        }
        
        public void UpdateListByParams(List<BusinessBalanceVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BusinessBalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessBalanceVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BusinessBalanceVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BusinessBalanceVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BusinessBalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessBalanceVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BusinessBalanceVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BusinessBalanceVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BusinessBalanceVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BusinessBalanceVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessBalanceVO>(strSQL, parameters);
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
        public List<BusinessBalanceVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<BusinessBalanceVO>(strSQL, parameters);
        }
        public List<BusinessBalanceVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<BusinessBalanceVO>(strSQL, parameters);
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

        public bool ReduceBalance(int BusinessID, decimal balance)
        {
            if (balance < 0)
                return false;

            int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance - " + balance + " where BusinessID = " + BusinessID + " and Balance >= " + balance);
            return result > 0;
        }

        public bool PlusBalance(int BusinessID, decimal balance)
        {
            if (balance < 0)
                return false;

            // 判断余额表是否存在信息
            if (this.FindByParams("BusinessID = " + BusinessID).Count < 1)
            {
                //添加余额表
                BusinessBalanceVO bVO = new BusinessBalanceVO();
                bVO.BusinessID = BusinessID;
                bVO.Balance = balance;
                return this.Insert(bVO) > 0;
            }
            else
            {
                int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance + " + balance + " where BusinessID = " + BusinessID);
                return result > 0;
            }
        }

        public bool SetBalance(int BusinessID, decimal balance)
        {
            // 判断余额表是否存在信息
            if (this.FindByParams("BusinessID = " + BusinessID).Count < 1)
            {
                //添加余额表
                BusinessBalanceVO bVO = new BusinessBalanceVO();
                bVO.BusinessID = BusinessID;
                bVO.Balance = balance;
                return this.Insert(bVO) > 0;
            }
            else
            {
                int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = " + balance + " where BusinessID = " + BusinessID);
                return result > 0;
            }
        }
    }
}
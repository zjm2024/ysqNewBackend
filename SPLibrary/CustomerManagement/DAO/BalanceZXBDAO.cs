using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.CustomerManagement.BO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;
using System.Data;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class BalanceZXBDAO : CommonDAO
    {
		public BalanceZXBDAO(UserProfile userProfile)
		{
			base._tableName= "t_csc_balance_zhongxiaobi";
			base._pkId = "BalanceId";
			base._voType = typeof(BalanceVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BalanceVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BalanceVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BalanceVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BalanceVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BalanceVO FindById(object id)
        {
            return base.FindById<BalanceVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BalanceVO> voList)
        {
            base.InsertList<BalanceVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BalanceVO> voList, int countInEveryRun)
        {
            base.InsertList<BalanceVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BalanceVO> voList)
        {
            base.UpdateListById<BalanceVO>(voList);
        }
        
        public void UpdateListByParams(List<BalanceVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BalanceVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BalanceVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BalanceVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BalanceVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BalanceVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BalanceVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BalanceVO>(strSQL, parameters);
        }

        public List<zxbRequireVO> FindAllByPageIndex_Require(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {//获取乐币奖励列表
            string strSQL = "";

            strSQL += " SELECT * FROM t_csc_zhongxiaobirequire \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<zxbRequireVO>(strSQL, parameters);
        }

        public List<zxbRequireVO> FindAllByPageIndex_Require(string conditionStr, params object[] parameters)
        {//获取乐币奖励列表
            string strSQL = "";

            strSQL += " SELECT * FROM t_csc_zhongxiaobirequire \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            return DbHelper.ExecuteVO<zxbRequireVO>(strSQL, parameters);
        }

        public int FindTotalCount_Require(string condition, params object[] parameters)
        {//获取乐币奖励列表数量
            string strSQL = "";
            strSQL += " Select Count(0) FROM t_csc_zhongxiaobirequire  \n";
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

        public bool ReduceBalance(int customerId, decimal balance, string str)
        {
           int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance - " + balance + " where CustomerId = " + customerId + " and Balance >= " + balance);
           if (result > 0)
           this.createhistory(customerId, -balance, str);
           return result > 0;
        }

        public bool AddrequireZXB(int customerId, decimal balance, string str,int type)
        {//发放奖励
            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            ZxbConfigVO zVO = _bo.FindZxbConfigById(type);
            if (zVO.Status == 0) {
                return false;
            }
            if (balance>0)
            {
                string sql = "INSERT INTO t_csc_zhongxiaobirequire (CustomerId ,Cost,Date,Purpose,type)VALUES (" + customerId + "," + balance + ", '" + System.DateTime.Now.ToString() + "', '" + str + "'," + type + ")";
                int result = DbHelper.ExecuteNonQuery(sql);
                if (result > 0)
                {
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    mBO.SendMessage("获得" + balance + "乐币奖励", "恭喜你获得了" + balance + "乐币奖励，快去 个人中心>我的奖励 处领取吧！", customerId, MessageType.SYS);
                }
                return result > 0;
            }
            return false;
        }
        public bool delRequireZXB(int ZXBrequireId)
        {//撤销奖励
            string strSQL = "SELECT * FROM t_csc_zhongxiaobirequire where ZXBrequireId=" + ZXBrequireId + " limit 0,1";
            DataTable dt = DbHelper.ExecuteDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                int customerId = Convert.ToInt32(dt.Rows[0]["CustomerId"].ToString());
                decimal balance = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());
                int Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                string sqlstr = "delete from t_csc_zhongxiaobirequire where ZXBrequireId = " + ZXBrequireId;
                if (Status == 0)
                {
                    int result = DbHelper.ExecuteNonQuery(sqlstr);
                    return result > 0;
                }
                else if(Status == 1)
                {
                    ReduceBalance(customerId, balance,"撤销");
                    int result = DbHelper.ExecuteNonQuery(sqlstr);
                    return result > 0;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public bool ReceiveRequireZXB(int ZXBrequireId)
        {//领取奖励
            string strSQL = "SELECT * FROM t_csc_zhongxiaobirequire where ZXBrequireId=" + ZXBrequireId+ " limit 0,1";
            DataTable dt=DbHelper.ExecuteDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                int customerId = Convert.ToInt32(dt.Rows[0]["CustomerId"].ToString());
                decimal balance = Convert.ToDecimal(dt.Rows[0]["Cost"].ToString());
                string str = dt.Rows[0]["Purpose"].ToString();
                int Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                if (Status == 0)
                {
                    DbHelper.ExecuteNonQuery(" Update t_csc_zhongxiaobirequire set Status = 1 where ZXBrequireId = " + ZXBrequireId);
                    PlusBalance(customerId, balance, str);
                    return true;
                }
                else
                {
                    return false;
                }
                
                
            }
            else {
                return false;
            }
        }
        public bool createhistory(int customerId, decimal balance, string str)
        {//添加乐币更改记录
            string sql = "INSERT INTO t_csc_zhongxiaobihistory (CustomerId ,Cost,Date,Purpose)VALUES ("+ customerId + ","+ balance + ", '" + System.DateTime.Now.ToString() + "', '"+ str + "')";
            int result = DbHelper.ExecuteNonQuery(sql);
            return result > 0;
        }

        public bool PlusBalance(int customerId, decimal balance, string str)
        {
            // 判断乐币表是否存在信息
            if (this.FindByParams("CustomerId = " + customerId).Count < 1)
            {
                //添加余额表
                BalanceVO bVO = new BalanceVO();
                bVO.CustomerId = customerId;
                bVO.Balance = balance;
                if(this.Insert(bVO) > 0)
                this.createhistory(customerId, balance, str);
                return this.Insert(bVO) > 0;
            }
            else
            {
                int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance + " + balance + " where CustomerId = " + customerId);
                if (result > 0)
                this.createhistory(customerId, balance, str);
                return result > 0;
            }
            
        }
        public decimal GetBalanceByCondition(string condition, params object[] parameters)
        {
            string strSQL = "SELECT sum(Cost) as TotalBalance FROM t_csc_zhongxiaobirequire where " + condition;

            decimal TotalBalance = 0;
            try
            {
                TotalBalance = Convert.ToDecimal(DbHelper.ExecuteScalar(strSQL));
            }
            catch
            {
                TotalBalance = 0;
            }
            return TotalBalance;

        }
        public decimal GetTotalBalance()
        {
            string strSQL = "SELECT sum(Cost) as TotalBalance FROM t_csc_zhongxiaobirequire";

            decimal TotalBalance = 0;
            try
            {
                TotalBalance = Convert.ToDecimal(DbHelper.ExecuteScalar(strSQL));
            }
            catch
            {
                TotalBalance = 0;
            }
            return TotalBalance;
          
        }

    }
}
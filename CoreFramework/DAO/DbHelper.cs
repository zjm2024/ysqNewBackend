using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using CoreFramework.VO;
using System.Transactions;
using MySql.Data.MySqlClient;
using System.Linq;

namespace CoreFramework.DAO
{
    public static class DbHelper
    {
        public delegate void DBAction(params object[] data);

        public static DateTime FixDateTime(DateTime t)
        {
            return t.AddMilliseconds(-t.Millisecond);
        }

        public static string GetParameterString(object parameterName)
        {
            return GetParameterString(parameterName, "=");
        }

        /// <summary>
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="operation">like = ,and so on</param>
        /// <returns></returns>
        public static string GetParameterString(object parameterName, string operation)
        {
            return string.Format(" {0} {1} @{0} ", parameterName, operation);
        }

        #region 私有工具方法：统一格式化参数日志
        /// <summary>
        /// 格式化 object[] 参数数组为可读字符串
        /// </summary>
        private static string FormatParamArrayLog(object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return "无参数";

            List<string> items = new List<string>();
            foreach (object p in parameters)
            {
                DbParameter tmp = p as DbParameter;
                if (tmp == null)
                {
                    items.Add($"[非DbParameter对象] 值：{p ?? "null"}");
                    continue;
                }
                // 三元加括号修复优先级问题
                string valStr = tmp.Value == null ? "null" : tmp.Value.ToString();
                items.Add($"{tmp.ParameterName}:{valStr}");
            }
            return string.Join("\r\n", items);
        }

        /// <summary>
        /// 格式化 DbParameterCollection 为可读字符串
        /// </summary>
        private static string FormatParamCollectionLog(DbParameterCollection paramCollection)
        {
            if (paramCollection == null || paramCollection.Count == 0)
                return "无参数";

            List<string> items = new List<string>();
            foreach (DbParameter tmp in paramCollection)
            {
                string valStr = tmp.Value == null ? "null" : tmp.Value.ToString();
                items.Add($"{tmp.ParameterName}:{valStr}");
            }
            return string.Join("\r\n", items);
        }
        #endregion

        public static int ExecuteNonQuery(string commandText, params object[] parameters)
        {
            int re = -1;
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (DbCommand cmd = db.GetSqlStringCommand(commandText))
                {
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            if (param != null) cmd.Parameters.Add(param);
                        }
                    }
                    re = db.ExecuteNonQuery(cmd);
                    cmd.Dispose();
                }
                return re;
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamArrayLog(parameters);
                string errMsg = $"ErrorSQL:{commandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }
        }

        public static int ExecuteNonQuery(ISelectFilter filter)
        {
            return ExecuteNonQuery(filter.CommandText, filter.Parameters);
        }

        public static int ExecuteNonQuery(DbCommand cmd)
        {
            int re = -1;
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (cmd)
                {
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    re = db.ExecuteNonQuery(cmd);
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamCollectionLog(cmd.Parameters);
                string errMsg = $"ErrorSQL:{cmd.CommandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return re;
        }

        public static object ExecuteScalar(string commandText, params object[] parameters)
        {
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (DbCommand cmd = db.GetSqlStringCommand(commandText))
                {
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    foreach (object p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }

                    object re = db.ExecuteScalar(cmd);
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    return re;
                }
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamArrayLog(parameters);
                string errMsg = $"ErrorSQL:{commandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }
        }

        public static object ExecuteScalar(ISelectFilter filter)
        {
            return ExecuteScalar(filter.CommandText, filter.Parameters);
        }

        public static object ExecuteScalar(DbCommand cmd)
        {
            object re = null;
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (cmd)
                {
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    re = db.ExecuteScalar(cmd);
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamCollectionLog(cmd.Parameters);
                string errMsg = $"ErrorSQL:{cmd.CommandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return re;
        }

        public static IDataReader ExecuteReader(string commandText, params object[] parameters)
        {
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (DbCommand cmd = db.GetSqlStringCommand(commandText))
                {
                    cmd.CommandText = commandText;
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    foreach (object p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    IDataReader re = db.ExecuteReader(cmd);
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    return re;
                }
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamArrayLog(parameters);
                string errMsg = $"ErrorSQL:{commandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }
        }

        public static IDataReader ExecuteReader(ISelectFilter filter)
        {
            return ExecuteReader(filter.CommandText, filter.Parameters);
        }

        public static IDataReader ExecuteReader(DbCommand cmd)
        {
            IDataReader re = null;
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (cmd)
                {
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    re = db.ExecuteReader(cmd);
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamCollectionLog(cmd.Parameters);
                string errMsg = $"ErrorSQL:{cmd.CommandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return re;
        }

        public static DataTable ExecuteDataTable(DbCommand cmd)
        {
            DataTable dt = null;
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (cmd)
                {
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    dt = db.ExecuteDataSet(cmd).Tables[0];
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamCollectionLog(cmd.Parameters);
                string errMsg = $"ErrorSQL:{cmd.CommandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return dt;
        }

        public static DataTable ExecuteDataTable(String commandText, params object[] parameters)
        {
            DataTable dt = null;
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                using (DbCommand cmd = db.GetSqlStringCommand(commandText))
                {
                    foreach (object p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    cmd.CommandTimeout = DBConfig.DBConnectionTimeOut;
                    dt = db.ExecuteDataSet(cmd).Tables[0];
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                string paramLog = FormatParamArrayLog(parameters);
                string errMsg = $"ErrorSQL:{commandText}\r\nParameters:\r\n{paramLog}";
                throw new Exception(errMsg, ex);
            }

            return dt;
        }

        public static List<T> ExecuteVO<T>(string commandText, params object[] parameters) where T : ICommonVO
        {
            List<T> voList = new List<T>();
            using (IDataReader dr = ExecuteReader(commandText, parameters))
            {
                VOHelper vo = new VOHelper();
                voList = vo.Bind<T>(dr);
            }
            return voList;
        }

        public static List<T> ExecuteVO<T>(Type voType, SelectFilter filter) where T : ICommonVO
        {
            return ExecuteVO<T>(filter.CommandText, filter.Parameters);
        }

        public static object CreateParameter(object key, object value)
        {
            switch (DBConfig.ProviderType)
            {
                case EProviderType.None:
                case EProviderType.SQL:
                default:
                    SqlParameter p = new SqlParameter();
                    p.IsNullable = true;
                    p.ParameterName = key.ToString();
                    p.Value = value;
                    if (p.Value != null && p.Value.GetType() == DBNull.Value.GetType())
                    {
                        p.DbType = DbType.Object;
                    }
                    return p;
                case EProviderType.MySQL:
                    MySqlParameter pMySQL = new MySqlParameter();
                    pMySQL.IsNullable = true;
                    pMySQL.ParameterName = key.ToString();
                    pMySQL.Value = value;
                    if (pMySQL.Value != null && pMySQL.Value.GetType() == DBNull.Value.GetType())
                    {
                        pMySQL.DbType = DbType.Object;
                    }
                    return pMySQL;
            }
        }

        public static object CreateLikeParameter(object key, object value)
        {
            return CreateParameter(key, "%" + value + "%");
        }

        private static void GetChild(DataRow dr, DataTable dt, string parentColumnName, string ColumnName, bool isDistinctByColumnNmae)
        {
            DataRow[] drs = dr.GetChildRows("child");
            string filter = string.Empty;
            foreach (DataRow tmp in drs)
            {
                if (isDistinctByColumnNmae)
                {
                    filter = string.Format("{0}={1} ", ColumnName, tmp[ColumnName]);
                }
                else
                {
                    filter = string.Format("{0}={1} and {2}={3}", ColumnName, tmp[ColumnName], parentColumnName, tmp[parentColumnName]);
                }
                if (dt.Select(filter).Length == 0)
                {
                    object[] v = tmp.ItemArray;
                    dt.Rows.Add(v);
                    GetChild(tmp, dt, parentColumnName, ColumnName, isDistinctByColumnNmae);
                }
            }
        }

        private static void GetChild(List<DataRow> drs, DataTable dt, string parentColumnName, string ColumnName, bool isDistinctByColumnNmae)
        {
            List<DataRow> lst = new List<DataRow>();
            string filter = string.Empty;
            foreach (DataRow tmp in drs)
            {
                if (isDistinctByColumnNmae)
                {
                    filter = string.Format("{0}={1} ", ColumnName, tmp[ColumnName]);
                }
                else
                {
                    filter = string.Format("{0}={1} and {2}={3}", ColumnName, tmp[ColumnName], parentColumnName, tmp[parentColumnName]);
                }
                if (dt.Select(filter).Length == 0)
                {
                    dt.Rows.Add(tmp.ItemArray);
                    DataRow[] childs = tmp.GetChildRows("child");
                    if (childs.Length > 0)
                    {
                        lst.AddRange(childs);
                    }
                }
            }
            if (lst.Count > 0)
            {
                GetChild(lst, dt, parentColumnName, ColumnName, isDistinctByColumnNmae);
            }
        }

        public static DataTable GetChildByConditon(string sql, string condition, string parentColumnName, string ColumnName)
        {
            return GetChildByConditon(sql, condition, parentColumnName, ColumnName, false, false);
        }

        public static DataTable GetChildByConditon(string sql, string condition, string parentColumnName, string ColumnName, bool isOrderByLevel, bool isDistinctByColumnNmae)
        {
            try
            {
                Database db = DataAccessUtility.CreateDBInstance();
                DataTable dt = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                DataTable rtn = dt.Clone();

                if (!string.IsNullOrEmpty(condition.Trim()))
                {
                    dt.ChildRelations.Add("child", dt.Columns[ColumnName], dt.Columns[parentColumnName], false);
                    DataRow[] drs = dt.Select(condition);
                    if (isOrderByLevel)
                    {
                        GetChild(new List<DataRow>(drs), rtn, parentColumnName, ColumnName, isDistinctByColumnNmae);
                    }
                    else
                    {
                        foreach (DataRow tmp in drs)
                        {
                            rtn.Rows.Add(tmp.ItemArray);
                            GetChild(tmp, rtn, parentColumnName, ColumnName, isDistinctByColumnNmae);
                        }
                    }
                    return rtn;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ErrorSQL:{sql}\r\nCondition:{condition}\r\nColumn Name:{ColumnName}", ex);
            }
        }

        #region transaction method

        public static int ExecuteTransaction(DBAction method, params object[] data)
        {
            int intResult = -1;
            TransactionOptions option = new TransactionOptions();
            option.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, option))
                {
                    method(data);
                    ts.Complete();
                    intResult = 1;
                }
            }
            catch (Exception ex)
            {
                intResult = -1;
                throw new Exception("DbHelper.ExecuteTransaction(DBAction method, params object[] data)\nException:" + ex.Message, ex);
            }

            return intResult;
        }
        #endregion
    }
}
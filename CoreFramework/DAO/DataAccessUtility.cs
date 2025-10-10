using System;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace CoreFramework.DAO
{
    /// <summary>
    /// Data access utility
    /// </summary>
    public class DataAccessUtility
    {


        #region Variable
        private static Database _db;
        private static string _DB_INSTANCE = DBConfig.DbName;

        public static string DB_INSTANCE
        {
            get { return DataAccessUtility._DB_INSTANCE; }
            set { DataAccessUtility._DB_INSTANCE = value; }
        }


        #endregion

        #region Construction function
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessUtility"/> class.
        /// </summary>
        public DataAccessUtility() { }
        #endregion


        #region  Method

        /// <summary>
        /// Get database instance
        /// </summary>
        private static Database CreateDatabaseInstance
        {
            get
            {
                if (Equals(_db, null))
                {
                    if (DBConfig.ProviderType == EProviderType.None)
                        _db = DatabaseFactory.CreateDatabase(_DB_INSTANCE);
                    else if (DBConfig.ProviderType == EProviderType.Access)
                    {
                        DbProviderFactory _dbProvider = DbProviderFactories.GetFactory("System.Data.OleDb");
                        _db = new GenericDatabase(_DB_INSTANCE, _dbProvider);
                    }
                    else if (DBConfig.ProviderType == EProviderType.SQL)
                    {
                        DbProviderFactory _dbProvider = DbProviderFactories.GetFactory("System.Data.SqlClient");
                        _db = new GenericDatabase(_DB_INSTANCE, _dbProvider);
                    }
                    else if (DBConfig.ProviderType == EProviderType.MySQL)
                    {
                        DbProviderFactory _dbProvider = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
                        _db = new GenericDatabase(_DB_INSTANCE, _dbProvider);
                    }

                }
                return _db;
            }
        }


        /// <summary>
        /// Create Database Instance        
        /// </summary>
        /// <returns>Enterprise Library Database Object</returns>
        /// <summary>
        /// Create Database Instance        
        /// </summary>
        /// <returns>Enterprise Library Database Object</returns>
        public static Database CreateDBInstance()
        {
            // Uncomment this code if singleton Database object generates problem of performance issues
            return CreateDatabaseInstance;
        }       
        
        /// <summary>
        /// Provide a method to excute the Transaction Handle
        /// </summary>
        /// <param name="method">which database method you need to excute in a transaction</param>
        /// <returns>1:transaction successfully commit; -1:transaction rollback </returns>
        public static int ExecuteTransaction(Action<Database> method)
        {
            int intResult = -1;
            TransactionOptions option = new TransactionOptions();
            option.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, option))
                {
                    //- Ensure there are one database object among the transaction excute
                    Database db = CreateDatabaseInstance;
                    method.Invoke(db);

                    ts.Complete();
                    intResult = 1;
                }
            }
            catch (Exception exMsg)
            {
                //LogBO _log = new LogBO(new DataAccessUtility().GetType());
                string strErrorMsg = exMsg.Message.ToString() + "\r\n  --" + exMsg.StackTrace;
                //_log.Error(strErrorMsg);
                throw (exMsg);
                //intResult = -1;
            }

            return intResult;
        }
        #endregion

    }





}

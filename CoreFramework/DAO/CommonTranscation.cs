/*Sample:
            CommonTranscation t = new CommonTranscation();
            t.TransactionContext += delegate()
            {
                updatemethod();
                insertmethod();

            };
            int result = t.Go();
  
            CommonTranscation t = new CommonTranscation();
            t. TranscationContextWithReturn+= delegate()
            {
	            int count =0;
                count += updatemethod();
                count += insertmethod();
                return count
            };
            int result = t.Go();
            object  count = t.TranscationReturnValue;

*/

using System;
using System.Transactions;

namespace CoreFramework.DAO
{
    public class CommonTranscation
    {
        protected IsolationLevel _Default = System.Transactions.IsolationLevel.ReadUncommitted;
        public delegate void TranscationHandler();
        public delegate object TranscationWithReturnHandler();
        protected object _rtnValue = null;
        protected Exception _ex;
        //public delegate void TranscationHandler2(params object[] args);
        /// <summary>
        /// 不带返回值
        /// </summary>
        public event TranscationHandler TransactionContext;

        /// <summary>
        /// 带返回值，返回值通过TranscationReturnValue 读取
        /// </summary>
        public event TranscationWithReturnHandler TranscationContextWithReturn;

        /// <summary>
        /// 可带参数
        /// </summary>
        //public event TranscationHandler2 TransactionContext2;

        public CommonTranscation()
        {
        }

        public CommonTranscation(IsolationLevel level)
        {
            _Default = level;
        }

        /// <summary>
        /// 执行事务，返1或-1，如果使用TranscationWithReturnHandler，则读取TranscationReturnValue获取事番中的返回值
        /// </summary>
        /// <returns></returns>
        public int Go()
        {
            int re = -1;
            if (TransactionContext != null)
            {
                try
                {
                    TransactionOptions option = new TransactionOptions();
                    option.IsolationLevel = _Default;
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, option))
                    {
                        TransactionContext.Invoke();
                        ts.Complete();
                        re = 1;
                    }
                    TransactionContext = null;
                }
                catch (Exception ex)
                {
                    //LogBO.Error(this.GetType(), ex, new StackTrace());
                    TransactionContext = null;
                    _ex = new Exception(ex.Message, ex);
                }
            }
            else if (TranscationContextWithReturn != null)
            {
                try
                {
                    TransactionOptions option = new TransactionOptions();
                    option.IsolationLevel = _Default;
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, option))
                    {
                        _rtnValue = TranscationContextWithReturn.Invoke();
                        ts.Complete();
                        re = 1;
                    }
                    TranscationContextWithReturn = null;
                }
                catch (Exception ex)
                {
                    //LogBO.Error(this.GetType(), ex, new StackTrace());
                    TranscationContextWithReturn = null;
                    _ex = new Exception(ex.Message, ex);
                }
            }
            return re;
        }

        public object TranscationReturnValue
        {
            get { return _rtnValue; }
        }

        public Exception Exception
        {
            get { return _ex; }
        }


    }
}

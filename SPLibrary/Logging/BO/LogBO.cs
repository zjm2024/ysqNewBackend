using System;
using System.Text;
using log4net;
using log4net.Config;
using System.Diagnostics;
using SPLibrary.CoreFramework.BO;

namespace SPLibrary.CoreFramework.Logging.BO
{
    public class LogBO : BaseBO,ILogBO
    {
        #region variables and methods for write log
         // properties.
        private ILog _logger;

        /// <summary>
        /// Initialize the class level properties.
        /// </summary>
        private  LogBO()
        {
           
        }

        /// <summary>
        /// Construct an instance GlobalServicesLogger object.
        /// </summary>
        /// <param name="type">Type used to identify the corresponding logger configuration.</param>
        public LogBO(Type type)
        {
            // load the logger configuration.
            XmlConfigurator.Configure();
            // set properties.
            _logger = LogManager.GetLogger(type);
        }

        /// <summary>
        /// Log the information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Info(object message)
        {
            // log the information specified.
            _logger.Info(message);
        }

        /// <summary>
        /// Log the information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="exception">Excpetion to log.</param>
        public void Info(object message, Exception exception)
        {
            // log the information specified.
            _logger.Info(message, exception);
        }

        /// <summary>
        /// Log the debug information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Debug(object message)
        {
            // log the debug information specified.
            _logger.Info(message);
        }

        /// <summary>
        /// Log the debug information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="exception">Excpetion to log.</param>
        public void Debug(object message, Exception exception)
        {
            // log the debug information specified.
            _logger.Info(message, exception);
        }

        /// <summary>
        /// Log the error information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Error(object message)
        {
            _logger.Error(message);
        }

        /// <summary>
        /// Log the error information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="exception">Excpetion to log.</param>
        public void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        } 
       

        /// <summary>
        /// Log the fatal information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Fatal(object message)
        {
            // log the fatal information specified.
            _logger.Info(message);
        }

        /// <summary>
        /// Log the fatal information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="exception">Excpetion to log.</param>
        public void Fatal(object message, Exception exception)
        {
            // log the information specified.
            _logger.Fatal(message, exception);
        }

        /// <summary>
        /// Log the warning information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Warn(object message)
        {
            // log the warning information specified.
            _logger.Warn(message);
        }

        /// <summary>
        /// Log the warning information specified.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="exception">Excpetion to log.</param>
        public void Warn(object message, Exception exception)
        {
            // log the warning information specified.
            _logger.Warn(message, exception);
        }


        /// <summary>
        /// Static Error
        /// </summary>
        /// <param name="type">Type used to identify the corresponding logger configuration.</param>
        /// <param name="exception">Excpetion to log.</param>
        /// <param name="list">Other to log.</param>


        /// <summary>
        /// Static Error
        /// </summary>
        /// <param name="type">Type used to identify the corresponding logger configuration.</param>
        /// <param name="exception">Excpetion to log.</param>
        /// <param name="stackTrace">this Stack Trace</param>
        /// <param name="list">Other to log.</param>
        public static void Error(Type type, Exception exception,StackTrace stackTrace, params object[] list)
        {
            // load the logger configuration.
            XmlConfigurator.Configure();
            // set properties.
            ILog iLog = LogManager.GetLogger(type);

            // Get Method List
            StringBuilder sbMethodList = GetMethodList(stackTrace);

            // Set Error Message
            StringBuilder stbError = new StringBuilder();
            stbError.Append("\r\n");
            stbError.Append("### Source Methods List -------------------------------------------------------------------------------------------------------------");
            stbError.Append("\r\n");
            stbError.Append(sbMethodList.ToString());
            stbError.Append("### Message -------------------------------------------------------------------------------------------------------------------------");
            stbError.Append("\r\n");
            stbError.Append(exception.Message);
            stbError.Append("\r\n");
            stbError.Append("### StackTrace ----------------------------------------------------------------------------------------------------------------------");
            stbError.Append("\r\n");
            stbError.Append(exception.StackTrace);

            if (list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    stbError.Append("\r\n");
                    stbError.AppendFormat("### Other Message No.{0}----------------------------------------------------------------------------------------------------------------------", i + 1);
                    stbError.Append("\r\n");
                    stbError.Append(list[i].ToString());
                }
            }

            // Set Blank Row
            stbError.Append("\r\n");

            // log the error information specified.
            iLog.Error(stbError.ToString());

        }

        /// <summary>
        /// Get Method List by StackTrace
        /// </summary>
        /// <param name="stackTrace">this StackTrace</param>
        /// <returns>Method List</returns>
        private static StringBuilder GetMethodList(StackTrace stackTrace)
        {
            StringBuilder sbMethodList = new StringBuilder();
            foreach (StackFrame sf in stackTrace.GetFrames())
            {
                sbMethodList.Append(sf.GetMethod().ToString() + "\r\n");
            }
            return sbMethodList;
        }
              

        #endregion
    }
}

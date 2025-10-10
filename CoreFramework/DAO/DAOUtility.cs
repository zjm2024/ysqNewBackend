
namespace CoreFramework.DAO
{
    public class DAOUtility
    {
        #region Variable
        /// <summary>
        /// SQL default escape char.
        /// </summary>
        public const char SQL_ESCAPE_CHAR = '\\';
        #endregion

        #region Constructor function
        /// <summary>
        /// Initializes a new instance of the <see cref="DAOUtility"/> class.
        /// </summary>
        public DAOUtility()
        { }
        #endregion

        #region Handling sql

        #region GetSQLUnicodeString

        /// <summary>
        /// Gets the SQL unicode STR.
        /// </summary>
        /// <param name="queryStr">The query STR.</param>
        /// <returns></returns>
        public static string GetSQLUnicodeStr(string queryStr)
        {
            string sReturn = (queryStr == null) ? "" : queryStr;
            sReturn = "N'" + sReturn + "'";
            return sReturn;
        }

        #endregion

        #region AddSqlQuote
        /// <summary>
        /// For the string value added Sql single Quotes.
        /// </summary>
        /// <param name="queryStr">To the string</param>
        /// <returns>Return after the string.</returns>
        public static string AddSqlQuote(string queryStr)
        {
            return AddSqlQuote(queryStr, false);
        }

        /// <summary>
        /// For the string value added Sql single Quotes.
        /// </summary>
        /// <param name="queryStr">To the string.</param>
        /// <param name="replaceValueQuote">Whether the replacement string value of a single-quotes for the two single quotes.</param>
        /// <returns>Return after the string.</returns>
        public static string AddSqlQuote(string queryStr, bool replaceValueQuote)
        {
            string sReturn = (queryStr == null) ? "" : queryStr;
            sReturn = (replaceValueQuote) ? ReplaceSQLStrForQuote(sReturn) : sReturn;
            sReturn = "'" + sReturn + "'";
            return sReturn;
        }
        #endregion

        #region AddSqlUnicodeQuote
        /// <summary>
        /// For the string value added Sql Unicode single Quotes.
        /// </summary>
        /// <param name="queryStr">To replace the string.</param>
        /// <returns>Return after the string(返回处理后的字符串)</returns>
        public static string AddSqlUnicodeQuote(string queryStr)
        {
            return AddSqlUnicodeQuote(queryStr, false);
        }

        /// <summary>
        /// For the string value added Sql Unicode single Quotes.
        /// </summary>
        /// <param name="queryStr">To replace the string.</param>
        /// <param name="replaceValueQuote">Whether the replacement string value of a single-quotes for the two single quotes.</param>
        /// <returns>Return after the string.</returns>
        public static string AddSqlUnicodeQuote(string queryStr, bool replaceValueQuote)
        {
            string sReturn = (queryStr == null) ? "" : queryStr;
            sReturn = (replaceValueQuote) ? ReplaceSQLStrForQuote(sReturn) : sReturn;
            sReturn = "N'" + sReturn + "'";
            return sReturn;
        }
        #endregion

        #region ReplaceSQLStrForQuote
        /// <summary>
        /// Replacement of the SQL strings for a single quoted two single quotes..
        /// </summary>
        /// <param name="queryStr">To replace the string.</param>
        /// <returns>Return after the string.</returns>
        public static string ReplaceSQLStrForQuote(string queryStr)
        {
            string sReturn = (queryStr == null) ? "" : queryStr;
            sReturn = sReturn.Replace("'", "''");
            return sReturn;
        }
        #endregion

        #region ReplaceSQLStrForLike
        /// <summary>
        /// Replacement of the SQL string "Like" string.
        /// </summary>
        /// <param name="queryStr">To replace the string.</param>
        /// <param name="escapeChar">SQL escapeChar.</param>
        /// <returns>Return after the string.</returns>
        public static string ReplaceSQLStrForLike(string queryStr, char escapeChar)
        {
            string sReturn = (queryStr == null) ? "" : queryStr;
            sReturn = sReturn.Replace(escapeChar.ToString(), string.Format("{0}{0}", escapeChar));
            sReturn = sReturn.Replace("%", escapeChar + "%");
            sReturn = sReturn.Replace("_", escapeChar + "_");
            sReturn = sReturn.Replace("[", escapeChar + "[");
            sReturn = sReturn.Replace("]", escapeChar + "]");
            sReturn = sReturn.Replace("^", escapeChar + "^");
            sReturn = sReturn.Replace("'", escapeChar + "''");
            return sReturn;
        }
        /// <summary>
        /// Replacement of the SQL string "Like" string.
        /// </summary>
        /// <param name="queryStr">To replace the string</param>
        /// <returns>Return after the string</returns>
        public static string ReplaceSQLStrForLike(string queryStr)
        {
            return ReplaceSQLStrForLike(queryStr, SQL_ESCAPE_CHAR);
        }
        #endregion

        #endregion


    }
}

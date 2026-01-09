using CoreFramework;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.WebConfigInfo
{
    class ParameterObject
    {
    }
    public class Filter
    {
        public string groupOp { get; set; }
        public Rules[] rules { get; set; }
        public Filter[] filter { get; set; }
        public string Result()
        {
            StringBuilder sb = new StringBuilder();
            if (filter != null)
            {
                for (int i = 0; i < filter.Length; i++)
                {
                    sb.Append("(" + filter[i].Result()).Append(") ");
                    //if (i < rules.Length - 1)
                    //{
                    //    sb.Append(groupOp).Append(" ");
                    //}
                }
            }
            if (rules.Length > 0 && filter != null && filter.Length > 0)
            {
                sb.Append(" ").Append(groupOp).Append(" ");
            }
            for (int i = 0; i < rules.Length; i++)
            {
                sb.Append(rules[i].Result()).Append(" ");
                if (i < rules.Length - 1)
                {
                    sb.Append(groupOp).Append(" ");
                }
            }
            return sb.ToString();
        }
    }

    public class Rules
    {
        private Dictionary<string, string> ops;
        public string field { get; set; }
        public string op { get; set; }
        public string data { get; set; }
        public Rules()
        {
            ops = new Dictionary<string, string>();
            ops.Add("eq", " = ");
            ops.Add("ieq", " = ");
            ops.Add("ne", " <> ");
            ops.Add("lt", " < ");
            ops.Add("le", " <= ");
            ops.Add("gt", " > ");
            ops.Add("ge", " >= ");
            ops.Add("bw", " LIKE ");
            ops.Add("bn", " NOT LIKE ");
            ops.Add("in", " IN ");
            ops.Add("ni", " NOT IN ");
            ops.Add("ew", " LIKE ");
            ops.Add("en", " NOT LIKE ");
            ops.Add("cn", " LIKE ");
            ops.Add("nc", " NOT LIKE ");
            ops.Add("bet", " and ");
            
            ops.Add("none", "");
        }
        public string Result()
        {
            //if (filter != null)
            //{
            //    return "(" + filter.Result() + ")";
            //}
            //else
            //{
            StringBuilder sb = new StringBuilder();

            if ((op.Equals("eq") || op.Equals("ne")) && string.IsNullOrEmpty(data))
            {
                data = Utilities.ReplaceSQLStrForQuote(data);
                sb.Append("(").Append(field).Append(ops[op]).Append("N'").Append(data).Append("' or ").Append(field).Append(" is ");
                if (op.Equals("ne"))
                {
                    sb.Append(" not ");
                }
                sb.Append(" null ) ");
            }
            else if (op.Equals("none"))
            {
                return " 1=1 ";
            }
            else if (op.Equals("ieq"))
            {
                sb.Append(field).Append(ops[op]).Append(data);
            }
            else if (op.Equals("le") || op.Equals("ge"))
            {
                sb.Append(field).Append(ops[op]).Append("'").Append(data).Append("'");
            }
            else
            {
                sb.Append(field).Append(ops[op]);

                if (op.Equals("in") || op.Equals("ni"))
                {
                    data = Utilities.ReplaceSQLStrForQuote(data);
                    string[] array = data.Split(',');
                    string temp = "";
                    foreach (string str in array)
                    {
                        temp += "N'" + str + "',";
                    }
                    sb.Append("(").Append(temp.Substring(0, temp.Length - 1)).Append(")");
                }
                else if (op.Equals("bw") || op.Equals("bn"))
                {
                    data = Utilities.ReplaceSQLStrForQuote(data);
                    sb.Append("N'").Append(data).Append("%'");
                }
                else if (op.Equals("ew") || op.Equals("en"))
                {
                    data = Utilities.ReplaceSQLStrForQuote(data);
                    sb.Append("N'%").Append(data).Append("'");
                }
                else if (op.Equals("cn") || op.Equals("nc"))
                {
                    data = Utilities.ReplaceSQLStrForLike(data);
                    if (DBConfig.ProviderType == EProviderType.MySQL)
                        sb.Append("N'%").Append(data).Append("%' ESCAPE '" + string.Format("{0}{0}", Utilities.SQL_ESCAPE_CHAR) + "'");
                    else
                        sb.Append("N'%").Append(data).Append("%' ESCAPE '" + Utilities.SQL_ESCAPE_CHAR + "'");
                }
                else if (op.Equals("bet"))
                {
                    data = Utilities.ReplaceSQLStrForQuote(data);
                    string[] array = data.Split('|');
                    sb.Append(" between ").Append(array[0]).Append(" and ").Append(array[1]);
                }
                else
                {
                    //if (!data.Contains("Encrypt"))
                    //{
                    sb.Append("N'").Append(data).Append("'");
                    //}
                    //else
                    //{
                    // sb.Append("N'").Append(BOUtilities.EncryptString(data.Replace("Encrypt('", "").Replace("')", ""))).Append("'");
                    //}
                }
            }

            return sb.ToString();
            //}
        }
    }

    public class Paging
    {
        //第几页
        public int PageIndex { get; set; }
        //每页行数
        public int PageCount { get; set; }
        //排序字段
        public string SortName { get; set; }
        //排序方式 asc desc
        public string SortType { get; set; }
    }
}

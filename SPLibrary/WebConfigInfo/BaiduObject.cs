using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.WebConfigInfo
{
    class BaiduObject
    {
    }

    public class Baidu_business_card_Result
    {
        public CardDataVO SuccessResult { get; set; }
        public bool Result { get; set; }

        public BaiduErrorMsg ErrorResult { get; set; }
    }
    /// <summary>
    /// 百度名片识别返回数据
    /// </summary>
    public class business_card
    {
        public string errno { get; set; }
        public string log_id { get; set; }
        public int words_result_num { get; set; }
        public words_result words_result { get; set; }
    }

    public class words_result
    {
        public string[] ADDR { get; set; }
        public string[] FAX { get; set; }
        public string[] MOBILE { get; set; }
        public string[] NAME { get; set; }
        public string[] PC { get; set; }
        public string[] URL { get; set; }
        public string[] TEL { get; set; }
        public string[] TITLE { get; set; }
        public string[] COMPANY { get; set; }
        public string[] EMAIL { get; set; }
    }

    /// <summary>
    /// 百度接口错误访问的情况
    /// </summary>
    public class BaiduErrorMsg
    {
        /// <summary>
        /// 错误编号
        /// </summary>
        public int error_code { get; set; }
        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string error_msg { get; set; }
    }
}

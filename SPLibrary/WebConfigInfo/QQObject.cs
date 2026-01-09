using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.WebConfigInfo
{
    class QQObject
    {
    }
    public class User_info
    {
        public string OpenID { get; set; }//用户唯一appid

        public string Name { get; set; }

        public string img_qq50 { get; set; } //QQ图像40*40

        public string img_qq100 { get; set; }

        public string city { get; set; }

        public int year { get; set; }

    }

    public class QQOpen
    {
        public string client_id { get; set; }

        public string openid { get; set; }
    }

    public class QQInfo
    {
        public string nickname { get; set; }

        public string figureurl_qq_1 { get; set; }
        public string figureurl_qq_2 { get; set; }
        public string city { get; set; }
        public int year { get; set; }
    }

}

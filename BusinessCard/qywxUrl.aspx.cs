using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tencent;

namespace BusinessCard
{
    public partial class qywxUrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            /**url中的签名**/
            String signature = Request.QueryString["msg_signature"];
            /**url中的时间戳*/
            String timestamp = Request.QueryString["timestamp"];
            /** url中的随机字符串 **/
            String nonce = Request.QueryString["nonce"];
            /** 创建套件时验证回调url有效性时传入**/
            String echostr = Request.QueryString["echostr"];

            string CorpID = "ww05d52e294e33edd5";
            string Token = "hEi74AmmoqTpxaPxcXUICpLb";
            string EncodingAESKey = "WCf6E2tkbMnuDyg4cCsgIyrIUXfYhTU5u5eHnkFpDLh";

            /**url中的签名**/
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(Token, EncodingAESKey, CorpID);
            string sEchoStr = "";
            int r = wxcpt.VerifyURL(signature, timestamp, nonce, echostr, ref sEchoStr);
            if (r == 0)
            {
                Response.Write(sEchoStr);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CustomerManagement.BO;

namespace WebUI
{
    public partial class SendPassCodeMsg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string phone = string.IsNullOrEmpty(Request.QueryString["phone"]) ? "0" : Request.QueryString["phone"];
            if (phone != "0")
            {
                MessageTool uBO = new MessageTool();
                if (string.IsNullOrEmpty(phone))
                {
                    Response.Write("{ \"Flag\" : 0, \"Message\" : \"手机号码不能为空!\", \"Result\" : null }");
                }
                else
                {
                    Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
                    int tmp = ra.Next(1000001, 9999999);
                    string code = tmp.ToString().Substring(1, 6);

                    string msgContent = "尊敬的客户：您的验证码为：" + code + "，请尽快校验。温馨提示：请妥善保管，不要随意泄露给他人。【众销乐-资源共享众包销售平台】";

                    string result = MessageTool.SendMobileMsg(msgContent, phone);
                    Session["code"] = code;
                    Session["phone"] = phone;

                    if (result == "1")
                        Response.Write("{ \"Flag\" : 1, \"Message\" : \"发送成功!\", \"SessionID\" : \""+ Session.SessionID + "\" }");
                    else
                        Response.Write("{ \"Flag\" : 0, \"Message\" : \"发送失败!\", \"SessionID\" : \"" + Session.SessionID + "\" }");
                }
            }
            else
            {
                Response.Write("{ \"Flag\" : 0, \"Message\" : \"手机号码不能为空!\", \"SessionID\" : \"" + Session.SessionID + "\" }");
            }
        }
    }
}



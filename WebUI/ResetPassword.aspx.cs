using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;

namespace WebUI
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string phone = string.IsNullOrEmpty(Request.QueryString["phone"]) ? "0" : Request.QueryString["phone"];
            string newPassword = string.IsNullOrEmpty(Request.QueryString["newPassword"]) ? "0" : Request.QueryString["newPassword"];
            string code = string.IsNullOrEmpty(Request.QueryString["code"]) ? "0" : Request.QueryString["code"];
            /*
            if (Session["code"] == null)
            {
                Response.Write("{ \"Flag\" : 0, \"Message\" : \"请先发送验证码!\", \"Result\" : null }");
                return;
            }
            else
            {
                if (Session["code"].ToString() != code)
                {
                    Response.Write("{ \"Flag\" : 0, \"Message\" : \"验证码错误!\", \"Result\" : null }");
                    return;
                }
            }

            if (Session["phone"] == null)
            {
                Response.Write("{ \"Flag\" : 0, \"Message\" : \"手机号码不得为空!\", \"Result\" : null }");
                return;
            }
            else
            {
                if (Session["phone"].ToString() != phone)
                {
                    Response.Write("{ \"Flag\" : 0, \"Message\" : \"手机号码错误，请重新发送验证码!\", \"Result\" : null }");
                    return;
                }
            }
            */
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindByParams("Phone = '" + phone + "'");
            if (cVO == null)
            {
                Response.Write("{ \"Flag\" : 0, \"Message\" : \"手机号码不存在!\", \"Result\" : null }");
            }
            else
            {
                if (newPassword != "0")
                {
                    bool result = uBO.ChangeCustomerPassword(cVO.CustomerId, Utilities.GetMD5(newPassword));
                    if (result)
                        Response.Write("{ \"Flag\" : 1, \"Message\" : \"修改成功!\", \"Result\" : null }");
                    else
                        Response.Write("{ \"Flag\" : 0, \"Message\" : \"修改失败!\", \"Result\" : null }");
                }
                else
                {
                    Response.Write("{ \"Flag\" : 0, \"Message\" : \"修改失败!\", \"Result\" : null }");
                }
            }
        }
    }
}
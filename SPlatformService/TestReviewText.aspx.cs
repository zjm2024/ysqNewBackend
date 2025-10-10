using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService
{
    public partial class TestReviewText : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "审核测试";
            }
            base.ValidPageRight("审核测试", "Read");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(TextBox1.Text))
            {
                txtTitle.Text = "有政治敏感或违法关键词，请重新填写!";
                txtTitle.ForeColor = Color.Red;//字体颜色
            }
            else
            {
                txtTitle.Text = "审核通过!";
                txtTitle.ForeColor = Color.Green;//字体颜色
            }
        }
    }
}
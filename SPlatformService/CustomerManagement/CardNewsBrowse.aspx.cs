using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CustomerManagement.BO;
using CoreFramework.VO;

namespace SPlatformService.UserManagement
{
    public partial class CardNewsBrowse : BasePage
    {
        public int Number { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "今日话题";
            }

            CardBO cBO = new CardBO(new CustomerProfile());

            Number=cBO.FindFormListView().Count;

            base.ValidPageRight("今日话题", "Read");  
        }
    }
}
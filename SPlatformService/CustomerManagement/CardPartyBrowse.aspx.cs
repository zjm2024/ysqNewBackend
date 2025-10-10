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

namespace SPlatformService.CustomerManagement
{
    public partial class CardPartyBrowse : BasePage
    {
        public static int SignupCount { get; set; } // 当天报名人数
        public static int SignupSum { get; set; } // 当天报名次数
        public static int YesterDaySignupCount { get; set; } // 昨天报名人数
        public static int YesterDaySignupSum { get; set; } // 昨天报名次数
        public static int d7DaySignupCount { get; set; } // 7天内报名人数
        public static int d7DaySignupSum { get; set; } // 7天内报名次数
        public static int d30DaySignupCount { get; set; } // 30天内报名人数
        public static int d30DaySignupSum { get; set; } // 30天内报名次数
        public static DateTime UpTime { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                
            }

            if (UpTime > DateTime.Now)
            {

            }
            string systemCustomerId = string.IsNullOrEmpty(Request.QueryString["isLuckDraw"]) ? "false" : Request.QueryString["isLuckDraw"];

            if (systemCustomerId == "true")
            {
                base.ValidPageRight("抽奖活动", "Read");
                (this.Master as Shared.MasterPage).PageNameText = "抽奖活动";

                if(UpTime.Date != DateTime.Now.Date)
                {
                    CardBO cBO = new CardBO(new CustomerProfile());
                    SignupCount = cBO.FindCardPartSignInNumAllByPageIndex("DATEDIFF(CreatedAt,NOW())=0 and isAutoAdd=0 and LuckDrawNumber<>'' GROUP BY CustomerId", 1, 99999, "CreatedAt", "desc").Count;
                    SignupSum = cBO.FindCardPartSignInNumTotalCount("DATEDIFF(CreatedAt,NOW())=0 and isAutoAdd=0 and LuckDrawNumber<>''");

                    YesterDaySignupCount = cBO.FindCardPartSignInNumAllByPageIndex("DATEDIFF(CreatedAt,NOW())=-1 and isAutoAdd=0 and LuckDrawNumber<>'' GROUP BY CustomerId", 1, 99999, "CreatedAt", "desc").Count;
                    YesterDaySignupSum = cBO.FindCardPartSignInNumTotalCount("DATEDIFF(CreatedAt,NOW())=-1 and isAutoAdd=0 and LuckDrawNumber<>''");

                    d7DaySignupCount = cBO.FindCardPartSignInNumAllByPageIndex("DATEDIFF(CreatedAt,NOW())<=-1 and DATEDIFF(CreatedAt,NOW())>=-7 and isAutoAdd=0 and LuckDrawNumber<>'' GROUP BY CustomerId", 1, 99999, "CreatedAt", "desc").Count;
                    d7DaySignupSum = cBO.FindCardPartSignInNumTotalCount("DATEDIFF(CreatedAt,NOW())<=-1 and DATEDIFF(CreatedAt,NOW())>=-7 and isAutoAdd=0 and LuckDrawNumber<>''");

                    d30DaySignupCount = cBO.FindCardPartSignInNumAllByPageIndex("isAutoAdd=0 and LuckDrawNumber<>'' GROUP BY CustomerId", 1, 99999, "CreatedAt", "desc").Count;
                    d30DaySignupSum = cBO.FindCardPartSignInNumTotalCount("isAutoAdd=0 and LuckDrawNumber<>''");
                    UpTime = DateTime.Now;
                }
            }
            else
            {
                base.ValidPageRight("活动信息", "Read");
                (this.Master as Shared.MasterPage).PageNameText = "活动信息";
            }

            this.isLuckDraw.Value = systemCustomerId;

            StringBuilder sb = new StringBuilder();
            sb.Append("var isLuckDraw='").Append(isLuckDraw.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardPartyBrowse", sb.ToString());
        }
    }
}
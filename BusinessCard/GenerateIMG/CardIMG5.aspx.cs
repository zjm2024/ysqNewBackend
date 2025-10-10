using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPlatformService.CustomerManagement;
using SPLibrary.CustomerManagement.BO;

namespace BusinessCard.GenerateIMG
{
    public partial class CardIMG5 : System.Web.UI.Page
    {
        public BCPartyVO PartyViewVO{ set; get; }
        public List<BCPartyContactsVO> ContactsViewVO{ set; get; }
        public List<BCPartyCostVO> BCPartyCostVO { set; get; }
        public PersonalViewVO PersonalVO { set; get; }
        public Int64 PartyID_index { set; get; }
        public int fontSize { set; get; }
        public int SignUpCount { set; get; }
        public string Title { set; get; }
        public string Cost { set; get; }
        public string StartTime{
            get {
                DateTime dt = PartyViewVO.StartTime;

                if (PartyViewVO.isStartTime==0&& PartyViewVO.Type==2)
                {
                    return "不限时间";
                }
                string str = "";
                int Hour = dt.Hour;

                if (Hour < 6)
                    str = "凌晨";
                else if (Hour >= 6 && Hour<12)
                    str = "早上";
                else if (Hour >= 12 && Hour < 14)
                    str = "中午";
                else if (Hour >= 14 && Hour < 18)
                    str = "下午";
                else if (Hour >= 18 && Hour <= 24)
                    str = "晚上";

                //if (Hour > 12)
                //    Hour -= 12;
                //return string.Format("{0:yyyy年MM月dd日 dddd}", dt) + str + Hour + "点";

                
                return string.Format("{0:f}", dt);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int PartyID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PartyID"]) ? "0" : Request.QueryString["PartyID"]);
            Int64 style = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["style"]) ? "0" : Request.QueryString["style"]);
            Int64 CustomerId = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            int AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "0" : Request.QueryString["AppType"]);
            LogBO _log = new LogBO(typeof(CardBO));
            PartyID_index = style;
            try
            {
                if (PartyID > 0)
                {
                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

                    /*
                    LogBO _log = new LogBO(typeof(CardBO));
                    string strErrorMsg = "CardIMG:" + cBO.appid + "\r\n  " + cBO.secret + "\r\n  "+ Request.QueryString["AppType"];
                    _log.Error(strErrorMsg);
                    */

                    BCPartyVO cVO = cBO.FindPartyById(PartyID);
                    if (cVO != null)
                    {
                        PartyViewVO = cVO;
                        SignUpCount = cBO.FindSignUpByPartyID(cVO.PartyID).Count;
                        ContactsViewVO = cBO.FindPartyContactsByPartyId(cVO.PartyID);
                        if (ContactsViewVO == null)
                        {
                            ContactsViewVO = new List<BCPartyContactsVO>();
                        }
                        List<BCPartyCostVO> CostVO = cBO.FindCostByPartyID(cVO.PartyID);
                        if (CostVO.Count > 0)
                        {
                            BCPartyCostVO = CostVO;
                            decimal min = CostVO[0].Cost;
                            decimal max = CostVO[0].Cost;
                            foreach (BCPartyCostVO item in CostVO)
                            {
                                decimal sCost = item.Cost;

                                if (item.isDiscount == 1)
                                {
                                    sCost = item.DiscountCost;
                                }

                                if (sCost > max)
                                {
                                    max = sCost;
                                }
                                if (sCost < min)
                                {
                                    min = sCost;
                                }
                            }

                            if (max == min)
                            {
                                Cost = max + "元";
                            }
                            else
                            {
                                Cost = min + "-" + max + "元";
                            }
                        }
                        else
                        {
                            BCPartyCostVO = new List<BCPartyCostVO>();
                            Cost = "";
                        }
                    }
                    else
                    {
                        PartyViewVO = new BCPartyVO();
                        ContactsViewVO = new List<BCPartyContactsVO>();
                    }
                    if (ContactsViewVO.Count > 0)
                    {
                        PersonalVO = cBO.FindPersonalViewById(ContactsViewVO[0].PersonalID);
                    }

                    //如果样式是15.则把名片改为发起人的名片
                    if (style == 15 || style == 16 || style == 17 || style == 18 || style == 19 || style == 20 || style == 23 || style == 24 || style == 25 || style == 26 || style == 27 || style == 28 || style == 29 || style == 30 || style == 31 || style == 32 || style == 33 || style == 39)
                    {
                        PersonalVO = cBO.FindPersonalViewById(PartyViewVO.PersonalID);
                    }

                    MatchCollection var = Regex.Matches(PartyViewVO.Title, @"[a-zA-Z]*");

                    MatchCollection var2 = Regex.Matches(PartyViewVO.Title, @"[\u4e00-\u9fa5]+");

                    int TitleLength = var.Count + var2.Count * 2;

                    if (TitleLength <= 20)
                    {
                        fontSize = 1;
                    }
                    else if (TitleLength > 20 && TitleLength <= 40)
                    {
                        fontSize = 2;
                    }
                    else if (TitleLength > 40 && TitleLength <= 60)
                    {
                        fontSize = 3;
                    }
                    else if (TitleLength > 60 && TitleLength <= 80)
                    {
                        fontSize = 4;
                    }
                    else
                    {
                        fontSize = 5;
                    }

                    //样式16，标题变成两行
                    if (style == 16)
                    {
                        string title = "<p>";
                        for (int i = 0; i < PartyViewVO.Title.Length; i++)
                        {
                            title += PartyViewVO.Title[i];
                            if (i == PartyViewVO.Title.Length / 2)
                            {
                                title += "</p><p>";
                            }
                        }
                        PartyViewVO.Title = title + "</p>";
                    }

                    if (PartyViewVO.Content != "")
                    {
                        PartyViewVO.Content = PartyViewVO.Content.Replace("\n", "</br>");
                    }
                    _log.Error("CustomerId=" + CustomerId);

                    if (CustomerId > 0)
                    {
                        PartyViewVO.QRCodeImg = cBO.GetCardPartyQRByMessage(PartyID, CustomerId, AppType);
                    }
                    //PartyViewVO.QRCodeImg = PartyViewVO.QRCodeImg.Replace("https", "http");
                    //if(PersonalVO.Headimg!="")
                    //   PersonalVO.Headimg = PersonalVO.Headimg.Replace("https", "http");
                    _log.Error("111111111==" + PersonalVO.BusinessName);
                    if (PersonalVO.BusinessName != "")
                    {
                        _log.Error("222222222222");
                        string testStr = PersonalVO.BusinessName;
                        string newStr = "<text>";
                        _log.Error("33333333333");
                        for (int counter = 0; counter < testStr.Length; counter++)
                        {
                            newStr += testStr[counter] + "</text>";
                            if (counter != testStr.Length - 1)
                            {
                                newStr += "<text>";
                            }
                        }

                        PersonalVO.BusinessName = newStr;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }

        }
    }
}
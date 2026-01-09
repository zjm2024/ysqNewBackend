using CoreFramework.VO;
using SPlatformService.Controllers;
using SPlatformService.ThirdLogin;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class PartyShow : System.Web.UI.Page
    {
        public CardPartyViewVO PartyViewVO { set; get; }
        public CardDataVO CardDataVO { set; get; }
        public CardDataVO ContactsCardDataVO { set; get; }
        public List<CardPartySignUpVO> SignUpList = new List<CardPartySignUpVO>();
        public List<CardPartySignUpViewVO> SignUpViewVO = new List<CardPartySignUpViewVO>();
        public int fontSize { set; get; }
        public string Cost { set; get; }
        public ViewBag ViewBag;
        public string TimeDown;
        public string StartTime
        {
            get
            {
                DateTime dt = PartyViewVO.StartTime;
                return string.Format("{0:f}", dt);
            }
        }
        public int InviterCID = 0;
        public int CustomerId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int PartyID = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["PartyID"]) ? "0" : HttpContext.Current.Request.QueryString["PartyID"]);
            InviterCID = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["InviterCID"]) ? "0" : HttpContext.Current.Request.QueryString["InviterCID"]);
            string userAgent = Request.UserAgent;
            if (userAgent.ToLower().Contains("micromessenger"))
            {
                //如果是微信内置浏览器就微信登录
                wxlogin.login();
                CustomerId = wxlogin.CustomerId;
                new CardController().AddAccessrecords("ReadParty", PartyID, CustomerId, "");
            }
            else
            {
                new CardController().AddAccessrecords("ReadParty", PartyID, 0, "");
            }


            if (PartyID > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    SignUpViewVO = cBO.FindSignUpViewById(wxlogin.CustomerId, PartyID);
                }

                CardPartyViewVO cVO = cBO.FindPartyViewById(PartyID);
                if (cVO != null)
                {
                    PartyViewVO = cVO;
                }
                else
                {
                    PartyViewVO = new CardPartyViewVO();
                }

                List<CardDataVO> ListcVO = cBO.FindCardByCustomerId(PartyViewVO.CustomerId);
                if (ListcVO.Count > 0)
                {
                    CardDataVO = ListcVO[0];
                }

                List<CardPartyContactsViewVO> ContactsViewVO = cBO.FindPartyContactsByPartyId(PartyViewVO.PartyID);
                ContactsCardDataVO = new CardDataVO();
                if (ContactsViewVO.Count > 0)
                {
                    ContactsCardDataVO.Name = ContactsViewVO[0].Name;
                    ContactsCardDataVO.Phone = ContactsViewVO[0].Phone;
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
                if (PartyViewVO.Content != "")
                {
                    PartyViewVO.Content = PartyViewVO.Content.Replace("\n", "</br>");
                }
                if (PartyViewVO.Title != "")
                {
                    PartyViewVO.Title = PartyViewVO.Title.Replace("\n", "");
                }
                if (PartyViewVO.Details != "")
                {
                    PartyViewVO.Details = PartyViewVO.Details.Replace("\n", "</br>");
                }

                PartyViewVO.QRCodeImg = PartyViewVO.QRCodeImg.Replace("https", "http");

                TimeSpan s = PartyViewVO.SignUpTime - DateTime.Now;
                double totalSeconds = Math.Floor(s.TotalSeconds);

                //天数
                var days = Math.Floor(totalSeconds / (60 * 60 * 24));
                //取模（余数）
                var modulo = totalSeconds % (60 * 60 * 24);
                //小时数
                var hours = Math.Floor(modulo / (60 * 60));
                modulo = modulo % (60 * 60);
                //分钟
                var minutes = Math.Floor(modulo / 60);
                //秒
                var seconds = Math.Floor(modulo % 60);

                TimeDown = days + "天" + hours + "小时" + minutes + "分钟" + seconds + "秒";

                if (PartyViewVO.isDisplayCost == 1)
                {
                    //获取费用
                    string getCost = "";
                    List<CardPartyCostVO> CardPartyCost = cBO.FindCostByPartyID(PartyID);
                    if (CardPartyCost.Count > 0)
                    {
                        var min = CardPartyCost[0].Cost;
                        var max = CardPartyCost[0].Cost;
                        for (var i = 1; i < CardPartyCost.Count; i++)
                        {
                            var Cost = CardPartyCost[i].Cost;
                            if (Cost < min)
                            {
                                min = Cost;
                            }
                        }
                        for (var i = 1; i < CardPartyCost.Count; i++)
                        {
                            var Cost = CardPartyCost[i].Cost;
                            if (Cost > max)
                            {
                                max = Cost;
                            }
                        }
                        if (max == min)
                        {
                            getCost = "￥" + max;
                        }
                        else
                        {
                            getCost = "￥" + Convert.ToInt32(min) + " - " + Convert.ToInt32(max);
                        }
                    }
                    if (getCost == "")
                    {
                        getCost = "免费";
                    }
                    Cost = getCost;
                }

                if (PartyViewVO.isDisplaySignup == 1)
                {
                    SignUpList = cBO.FindSignUpByPartyID(PartyID);
                    SignUpList.Reverse();
                }
            }
            else
            {
                PartyViewVO = new CardPartyViewVO();
            }



            GetWX();
        }
        /// <summary>
        /// 获取微信分享接口参数
        /// </summary>
        public void GetWX()
        {
            WX_JSSDK jssdk = new WX_JSSDK();
            ViewBag = jssdk.getSignPackage();
        }
        public string formatMsgTime(DateTime dt)
        {//时间转换
            DateTime dateTime = dt;
            var year = dateTime.Year;
            var month = dateTime.Month;
            var day = dateTime.Day;
            var hour = dateTime.Hour;
            var minute = dateTime.Minute;
            var second = dateTime.Second;
            var now = DateTime.Now;

            var timeSpanStr = "";

            TimeSpan s = now - dateTime;
            var milliseconds = Math.Floor(s.TotalSeconds);

            if (milliseconds <= 60 * 1)
            {
                timeSpanStr = "刚刚";
            }
            else if (60 * 1 < milliseconds && milliseconds <= 60 * 60)
            {
                timeSpanStr = (int)(milliseconds / 60) + "分钟前";
            }
            else if (60 * 60 * 1 < milliseconds && milliseconds <= 60 * 60 * 24)
            {
                timeSpanStr = (int)(milliseconds / (60 * 60)) + "小时前";
            }
            else if (60 * 60 * 24 < milliseconds && milliseconds <= 60 * 60 * 24 * 15)
            {
                timeSpanStr = (int)(milliseconds / (60 * 60 * 24)) + "天前";
            }
            else if (milliseconds > 60 * 60 * 24 * 15 && year == now.Year)
            {
                timeSpanStr = month + "月" + day + "日";
            }
            else
            {
                timeSpanStr = year + "年" + month + "月" + day + "日";
            }
            return timeSpanStr;
        }
    }
}
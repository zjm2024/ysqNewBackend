using CoreFramework.VO;
using SPlatformService.TokenMange;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class PartyBuying_ok : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try { 
            int CustomerId = Convert.ToInt32(HttpContext.Current.Session["#Session#CustomerId"]);

            CardBO cBO = new CardBO(new CustomerProfile());
            int PartyID = Convert.ToInt32(string.IsNullOrEmpty(Request.Form["PartyID"]) ? "0" : Request.Form["PartyID"]);
            int InviterCID = Convert.ToInt32(string.IsNullOrEmpty(Request.Form["InviterCID"]) ? "0" : Request.Form["InviterCID"]);
            CardPartyVO cpvo = cBO.FindPartyById(PartyID);
            if (cpvo == null)
            {
                Alert("参数错误！！！");
            }

            int Number = 1;
            List<CardPartySignUpViewVO> cpsuVO = cBO.FindSignUpViewByPartyID(PartyID);
            int _NumberSum = cpsuVO.Sum(p => p.Number);

            if (cpvo.limitPeopleNum != 0 && cpvo.limitPeopleNum < _NumberSum + Number) { Alert("报名人数已满！！！");}

            if (cpvo.SignUpTime < DateTime.Now&& cpvo.Type==1)
            {
                Alert("报名已截止！");
            }

                if (cpvo.isEndTime != 0&&cpvo.SignUpTime < DateTime.Now && cpvo.Type == 2)
                {
                    Alert("报名已截止！");
                }

                if (cpvo.Status == 0)
            {
                Alert("该活动已被主办方删除，请勿报名！");
            }

            
            int CardID = 0;
            string name = "";
            string Headimg = "";
            string phone = "";
            string SignUpForm = "";
            string Position = "";
            string CorporateName = "";
            string Address = "";
            decimal latitude = 0;
            decimal longitude = 0;
            string WeChat = "";

            List<CardPartySignUpFormVO> fVO = cBO.FindSignUpFormByPartyID(PartyID);
            for (int i=0;i< fVO.Count;i++)
            {
                if (fVO[i].Status > 0)
                {
                    fVO[i].value = string.IsNullOrEmpty(Request.Form["input"+ fVO[i].SignUpFormID]) ? "" : Request.Form["input"+ fVO[i].SignUpFormID];
                    if (fVO[i].Name == "姓名")
                        name = fVO[i].value;
                    if (fVO[i].Name == "手机")
                        phone = fVO[i].value;
                    if (fVO[i].Name == "职位")
                        Position = fVO[i].value;
                    if (fVO[i].Name == "工作单位")
                        CorporateName = fVO[i].value;
                    if (fVO[i].Name == "微信")
                        WeChat = fVO[i].value;
                    if (fVO[i].Name == "单位地址")
                    {
                        Address = fVO[i].value;
                        WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(fVO[i].value);
                        if (Geocoder != null)
                        {
                            latitude = Geocoder.result.location.lat;
                            longitude = Geocoder.result.location.lng;
                        }
                    }
                    SignUpForm += "<SignUpForm>" + "<Name>" + fVO[i].Name + "</Name>" + "<Value>" + fVO[i].value + "</Value>" + "</SignUpForm>";
                }
            }

            List<CardDataVO> uVO = cBO.FindCardByCustomerId(CustomerId);
            if (uVO.Count > 0)
            {
                Headimg = uVO[0].Headimg;
                CardID = uVO[0].CardID;


                if (uVO[0].Phone == "" && phone != "")
                {
                    uVO[0].Phone = phone;
                }
                if (uVO[0].Position == "" && Position != "")
                {
                    uVO[0].Position = Position;
                }
                if (uVO[0].CorporateName == "" && CorporateName != "")
                {
                    uVO[0].CorporateName = CorporateName;
                }
                if (uVO[0].WeChat == "" && WeChat != "")
                {
                    uVO[0].WeChat = WeChat;
                }
                if (uVO[0].Address == "" && Address != "")
                {
                    uVO[0].Address = Address;
                }

                cBO.Update(uVO[0]);
            }
            else
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO = uBO.FindCustomenById(CustomerId);
                Headimg = CustomerVO.HeaderLogo;

                CardDataVO CardDataVO = new CardDataVO();
                CardDataVO.Name = name;
                CardDataVO.Phone = phone;
                CardDataVO.Position = Position;
                CardDataVO.CorporateName = CorporateName;
                CardDataVO.Headimg = Headimg;
                CardDataVO.Address = Address;
                CardDataVO.WeChat = WeChat;
                CardDataVO.latitude = latitude;
                CardDataVO.longitude = longitude;

                CardDataVO.CreatedAt = DateTime.Now;
                CardDataVO.Status = 1;//0:禁用，1:启用
                CardDataVO.CustomerId = CustomerId;
                CardDataVO.isParty = 1;

                CardID = cBO.AddCard(CardDataVO);
            }

            List<CardPartyCostVO> cVO = cBO.FindCostByPartyID(PartyID);
            if (cVO.Count > 0)
            {
                int PartyCostID = Convert.ToInt32(string.IsNullOrEmpty(Request.Form["Cost"]) ? "0" : Request.Form["Cost"]);
                if (PartyCostID <= 0)
                {
                    Alert("请选择购买类型!");
                        return;
                }
                CardPartyCostVO CostVO = cBO.FindCostById(PartyCostID);
                if (CostVO == null)
                {
                    Alert("请选择购买类型!");
                        return;
                    }

                List<CardPartySignUpViewVO> CostSignUpVO = cBO.PartyCostSignUpView(CostVO.Names, PartyID);
                int _CostNumberSum = CostSignUpVO.Sum(p => p.Number);

                if (CostVO.limitPeopleNum != 0 && CostVO.limitPeopleNum < _CostNumberSum + Number) { Alert("该类型报名人数已满，请选择其他购买类型!"); return; }

                if (CostVO.EffectiveTime.Year > 1900 && CostVO.EffectiveTime < DateTime.Now)
                {
                    Alert("该类型报名已截止,请选择其他购买类型!");
                        return;
                    }

                CardPartyOrderVO OrderVO = new CardPartyOrderVO();
                OrderVO.CardID = CardID;
                OrderVO.CustomerId = CustomerId;
                OrderVO.Name = name;
                OrderVO.Headimg = Headimg;
                OrderVO.Phone = phone;
                OrderVO.PartyID = PartyID;
                OrderVO.CreatedAt = DateTime.Now;
                OrderVO.SignUpForm = SignUpForm;
                OrderVO.CostName = CostVO.Names;
                OrderVO.Cost = CostVO.Cost * Number;
                OrderVO.Number = Number;
                OrderVO.InviterCID = InviterCID;

                Random ran = new Random();
                OrderVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

                int PartyOrderID = cBO.AddPartyOrder(OrderVO);
                if (PartyOrderID > 0)
                {
                    Response.Redirect("OderDetail.aspx?PartyOrderID="+ PartyOrderID);
                }
                else
                {
                    Alert("操作失败，请重试!");
                        return;
                    }
            }
            else
            {
                CardPartySignUpVO suVO = new CardPartySignUpVO();
                suVO.CardID = CardID;
                suVO.CustomerId = CustomerId;
                suVO.Name = name;
                suVO.Headimg = Headimg;
                suVO.Phone = phone;
                suVO.PartyID = PartyID;
                suVO.CreatedAt = DateTime.Now;
                suVO.SignUpForm = SignUpForm;
                suVO.Number = Number;
                suVO.InviterCID = InviterCID;

                int PartySignUpID = cBO.AddCardToParty(suVO);
                if (PartySignUpID > 0)
                {
                    cBO.sendSignUpMessage(PartySignUpID);
                    CardDataVO CardDataVO = cBO.FindCardById(CardID);
                    cBO.AddCardMessage(CardDataVO.Name + "报名参加了" + cpvo.Title, cpvo.CustomerId, "活动报名", "/pages/Party/SignUpList/SignUpList?isadmin=true&PartyID=" + cpvo.PartyID);
                    Response.Redirect("SignUpShow.aspx?PartySignUpID=" + PartySignUpID);
                }
                else
                {
                    Alert("报名失败，请重试!");
                }
            }
            }
            catch (Exception ex)
            {
                Alert("系统错误!");
            }

        }
        void Alert(string text)
        {
            Response.Write("<script language=javascript>alert('"+ text + "');history.go(-1);</script>");
        }
    }
}
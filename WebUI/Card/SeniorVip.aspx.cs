using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;

namespace WebUI.Card
{
    public partial class SeniorVip : System.Web.UI.Page
    {
        public string Token = "";
        public int CustomerId = 0;
        public string VipLevelText = "";
        public string VipLevelTip = "";
        public int Type = 0;
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        protected void Page_Load(object sender, EventArgs e)
        {
            wxlogin.login();
            Token = wxlogin.Token;
            CustomerId = wxlogin.CustomerId;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO uVO = uBO.FindById(CustomerId);

            if (uVO.VipLevel==1 || uVO.VipLevel == 0)
            {
                VipLevelText = "您还不是高级会员";
                VipLevelTip = "购买立享多重优惠";
            }
            else
            {
                VipLevelText = "您是高级VIP会员";
                VipLevelTip = "您可以续费或升级会员";
            }

            Type = Convert.ToInt32(string.IsNullOrEmpty(Request.Form["Cost"]) ? "0" : Request.Form["Cost"]);
            if (Type > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                CardOrderVO oVO = new CardOrderVO();
                string appid = "wx584477316879d7e9";
                if (Type != 1 && Type != 2 && Type != 3 && Type != 6 && Type != 7)
                {
                    Alert("购买类型错误，请重新选择!");
                    return;
                }

                oVO.CustomerId = CustomerId;
                oVO.CreatedAt = DateTime.Now;
                oVO.Status = 0;
                oVO.Type = Type;

                if (Type == 1)
                {
                    oVO.Cost = 39;
                }
                else if (Type == 2)
                {
                    oVO.Cost = 298;
                }
                else if (Type == 3)
                {
                    oVO.Cost = 998;
                }

                else if (Type == 5)
                {
                    oVO.Cost = 68;
                }

                if (Type == 1 || Type == 2 || Type == 3 || Type == 5)
                {
                    if (uVO.isVip && uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 2 || uVO.VipLevel == 3))
                    {
                        Alert("您已经是合伙人或分公司Vip，如想降级为普通Vip请等当前VIP到期后再开通！");
                        return;
                    }
                }

                else if (Type == 6)
                {
                    Response.Redirect("applyAgency.aspx?Type=6", true);
                    oVO.Cost = 10000;
                }
                else if (Type == 7)
                {
                    Response.Redirect("applyAgency.aspx?Type=7", true);
                    oVO.Cost = 50000;
                }

                Random ran = new Random();
                oVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

                int OrderID = cBO.AddOrder(oVO);
                if (OrderID > 0)
                {
                    CardOrderVO OrderVO = cBO.FindOrderById(OrderID);

                    //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                    JsApiPay jsApiPay = new JsApiPay(this);
                    jsApiPay.openid = HttpContext.Current.Session["#Session#Openid"].ToString();

                    string total_fee = Math.Floor(OrderVO.Cost * 100).ToString();
                    jsApiPay.total_fee = int.Parse(total_fee);
                    jsApiPay.out_trade_no = OrderVO.OrderNO;
                    jsApiPay.prepay_id = OrderVO.CardOrderID;
                    jsApiPay.notify_url = "http://api.leliaomp.com/Pay/Card_Notify_Url.aspx";
                    //JSAPI支付预处理
                    try
                    {
                        WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                        wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                        Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                        Response.Write("<script language=javascript>var ispay=1;var Type=" + Type + "</script>");
                    }
                    catch (Exception ex)
                    {
                        Alert("获取订单失败！");
                    }
                }
            }
        }
        void Alert(string text)
        {
            Response.Write("<script language=javascript>alert('" + text + "');history.go(-1);</script>");
        }
    }
}
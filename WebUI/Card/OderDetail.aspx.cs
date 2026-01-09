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
    public partial class OderDetail : System.Web.UI.Page
    {
        public int PartyOrderID = 0;
        public CardPartyOrderViewVO PartyOrder = new CardPartyOrderViewVO();
        public bool isPay = false;
        public int PayTime = 0;
        string total_fee = "0";
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        protected void Page_Load(object sender, EventArgs e)
        {


            PartyOrderID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PartyOrderID"]) ? "0" : Request.QueryString["PartyOrderID"]);

            if (!string.IsNullOrEmpty(Request.QueryString["state"]))
            {
                PartyOrderID= Convert.ToInt32(Request.QueryString["state"]);
            }


            CardBO cBO = new CardBO(new CustomerProfile());
            PartyOrder = cBO.FindPartyOrderViewById(PartyOrderID);
            if (PartyOrder == null)
            {
                Alert("获取订单失败！");
            }else
            {
                isPay = PartyOrder.CreatedAt.AddMinutes(30) > DateTime.Now;
                PayTime = (PartyOrder.CreatedAt.AddMinutes(30) - DateTime.Now).Minutes;
                total_fee = Math.Floor(PartyOrder.Cost * 100).ToString();
            }

            if (!IsPostBack&& PartyOrder.Status==0&&isPay)
            {
                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay(this);
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                if (HttpContext.Current.Session["#Session#Openid"] == null)
                {
                    jsApiPay.GetOpenidAndAccessToken(PartyOrderID);
                    HttpContext.Current.Session["#Session#Openid"] = jsApiPay.openid;
                }else
                {
                    jsApiPay.openid = HttpContext.Current.Session["#Session#Openid"].ToString();
                }
                jsApiPay.total_fee = int.Parse(total_fee);
                jsApiPay.out_trade_no = PartyOrder.OrderNO;
                jsApiPay.prepay_id = PartyOrder.PartyOrderID;

                //JSAPI支付预处理
                try
                {
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);

                }
                catch (Exception ex)
                {
                    Alert("获取订单失败！");
                }
            }
        }
        void Alert(string text)
        {
            Response.Write("<script language=javascript>alert('" + text + "');history.go(-1);</script>");
        }
    }
}
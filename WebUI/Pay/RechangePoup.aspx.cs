using CoreFramework.VO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Pay
{
    public partial class RechangePoup : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#TOKEN"].ToString();
                }
            }
        }

        public string ImageUrl()
        {
           
             //   string token = HttpContext.Current.Session["#Session#TOKEN"].ToString();
                string productId = "123456789123456789";
                string APIURL = ConfigInfo.Instance.APIURL;
                string total_fee = txtAmount.Text;
                string resultStr = HttpHelper.HtmlFromUrlGet(APIURL + "/SPWebAPI/Project/GetCodeURL?productId=" + productId + "&token=" + Token);
                ResultObject result = JsonConvert.DeserializeObject<ResultObject>(resultStr);

                //将url生成二维码图片
                string ImageUrl = "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(result.Result.ToString());
                return ImageUrl;
           
        }


        //protected void rbPayType_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rbPayType.SelectedValue == "1")
        //    {
        //        string token = HttpContext.Current.Session["#Session#TOKEN"].ToString();
        //        string productId = "123456789123456789";
        //        string APIURL = ConfigInfo.Instance.APIURL;
        //        string resultStr = HttpHelper.HtmlFromUrlGet(APIURL + "/SPWebAPI/Project/GetCodeURL?productId=" + productId + "&token=" + token);
        //        ResultObject result = JsonConvert.DeserializeObject<ResultObject>(resultStr);

        //        //将url生成二维码图片
        //        Image2.ImageUrl = "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(result.Result.ToString());
        //        dvWx.Style.Add("display", "");
        //        dvpaytype.Style.Add("display", "");
        //        dvAli.Style.Add("display", "none");
        //    }
        //    else
        //    {
        //        dvAli.Style.Add("display", "");
        //        dvpaytype.Style.Add("display", "");
        //        dvWx.Style.Add("display", "none");




        //        //DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay_public_key, config.charset, false);

        //        //// 外部订单号，商户网站订单系统中唯一的订单号
        //        //string out_trade_no = WIDout_trade_no.Text.Trim();

        //        //// 订单名称
        //        //string subject = WIDsubject.Text.Trim();

        //        //// 付款金额
        //        //string total_amout = WIDtotal_amount.Text.Trim();

        //        //// 商品描述
        //        //string body = WIDbody.Text.Trim();

        //        //// 组装业务参数model
        //        //AlipayTradePagePayModel model = new AlipayTradePagePayModel();
        //        //model.Body = body;
        //        //model.Subject = subject;
        //        //model.TotalAmount = total_amout;
        //        //model.OutTradeNo = out_trade_no;
        //        //model.ProductCode = "FAST_INSTANT_TRADE_PAY";

        //        //AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
        //        //// 设置同步回调地址
        //        //request.SetReturnUrl("");
        //        //// 设置异步通知接收地址
        //        //request.SetNotifyUrl("");
        //        //// 将业务model载入到request
        //        //request.SetBizModel(model);

        //        //AlipayTradePagePayResponse response = null;
        //        //try
        //        //{
        //        //    response = client.pageExecute(request, null, "post");
        //        //    Response.Write(response.Body);
        //        //}
        //        //catch (Exception exp)
        //        //{
        //        //    throw exp;
        //        //}

        //    }
        //}
    }
}
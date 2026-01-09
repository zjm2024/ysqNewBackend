using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using CoreFramework.VO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebUI.Pay
{
    public partial class CustomerRechange : CustomerBasePage
    {
        public static object WxPayConfig { get; private set; }
        public object CacheManager { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是钱包";
                (this.Master as Shared.MasterPage).PageNameText = "充值";
            }

        }


        protected void BtnPay_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtAmount.Text.Trim()) < 0)
                return;
            string out_trade_no = "0";
            out_trade_no = GenerateOutTradeNo("Ali");
            //string token = HttpContext.Current.Session["#Session#TOKEN"].ToString();
            //CustomerProfile up = (CustomerProfile)CacheManager.GetUserProfile(token);
            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            PayinHistoryVO vo = new PayinHistoryVO();
            vo.CustomerId = new CustomerPrincipal().CustomerProfile.CustomerId;
            vo.Cost = Convert.ToDecimal(txtAmount.Text.Trim());
            vo.PayInOrder = out_trade_no;
            vo.PayInStatus = 0;
            vo.PayInDate = DateTime.Now;
            vo.Purpose = "支付宝充值";

            _bo.InsertPayinHistory(vo);

            DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay1_public_key, config.charset, false);

            // 外部订单号，商户网站订单系统中唯一的订单号
            // string out_trade_no = WIDout_trade_no.Text.Trim();

            // 订单名称
            // string subject = WIDsubject.Text.Trim();

            // 付款金额
            // string total_amout = WIDtotal_amount.Text.Trim();

            // 商品描述
            // string body = WIDbody.Text.Trim();

            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.Body = "众销乐-资源共享众包销售平台客户钱包充值";
            model.Subject = "众销乐-资源共享众包销售平台客户钱包充值";
            model.TotalAmount = vo.Cost.ToString();
            model.OutTradeNo = out_trade_no;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(config.ReturnUrl);
            // 设置异步通知接收地址
            request.SetNotifyUrl(config.NotifyUrl);
            // 将业务model载入到request
            request.SetBizModel(model);

            AlipayTradePagePayResponse response = null;
            string returns = "";
            try
            {
                response = client.pageExecute(request, null, "post");
                string body = response.Body;
                Response.Write(response.Body);


            }
            catch (Exception exp)
            {


            }


        }
        private static string GenerateOutTradeNo(string type)
        {
            var ran = new Random();

            return string.Format("{0}{1}{2}", type, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }
        protected void Btntest_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            sArray.Add("gmt_create", "2017-11-09 14:53:47");
            sArray.Add("charset", "UTF-8");
            sArray.Add("gmt_payment", "2017-11-09 14:53:59");
            sArray.Add("notify_time", "2017-11-09 16:17:19");
            sArray.Add("subject", "众销乐-资源共享众包销售平台客户钱包充值");
            sArray.Add("sign", "dQZBZoDuKxuYo31WdSxU8Bymw3gSGY2thBJy4sIQIULIsWj6Sol9y2+0uxyOpufZYrGaPFjUVaSqJFzYVH8ORKX5KCpDAuVH31i7gVPQOZ2sgf68qDtiGDjLK9gCn7Y0O2u09mMVFE7QA2Cg0WTjO636FDQ4sJyJiP5wbhsNcTEZgC06vIFWDAcYhujXeBiCSr0o2ey4RtqfnaB5SuSffiRxld3hzGvoFRhjkOdOFnvNci18ppAYkruNXISh5YOXcuwck7B2RFmufcSE/BzyINxH05QQQPn5qmDOXkqnVDEGoEor7KJ4SIOO3I+3fvgoCT07izQZFWw8wLruzLGwnQ==");

            sArray.Add("buyer_id", "2088302448369236");
            sArray.Add("body", "众销乐-资源共享众包销售平台客户钱包充值");
            sArray.Add("invoice_amount", "0.01");
            sArray.Add("version", "1.0");
            sArray.Add("notify_id", "43c01f2f6f6e903418ec9e2a11c3f30hs1");
            sArray.Add("fund_bill_list","[{\"amount\":\"0.01\",\"fundChannel\":\"ALIPAYACCOUNT\"}]");
            sArray.Add("notify_type","trade_status_sync");
            sArray.Add("out_trade_no","Ali2017110914530133");
            sArray.Add("total_amount","0.01");
            sArray.Add("trade_status","TRADE_SUCCESS");
            sArray.Add("trade_no","2017110921001004230549329016");
            sArray.Add("auth_app_id", "2017102609537389");
            sArray.Add("receipt_amount","0.01");
            sArray.Add("point_amount","0.00");
            sArray.Add("app_id","2017102609537389");
            sArray.Add("buyer_pay_amount","0.01");
            sArray.Add("sign_type", "RSA2");
            sArray.Add("seller_id", "2088721825733141");
            string result = "";
            try
            {
                bool flag = AlipaySignature.RSACheckV1(sArray, config.alipay1_public_key, config.charset, config.sign_type, false);
                result = "返回结果flag=" + flag.ToString();
            }
            catch (Exception exp)
            {

                result = result + " 程序异常,message:" + exp.Message + " StackTrace:" + exp.StackTrace;
            }
            //lblpayReturn.Text = result;
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
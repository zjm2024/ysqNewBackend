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
using WebUI.Common;

namespace WebUI.Pay
{
    public partial class WXPay : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是钱包";
                (this.Master as Shared.MasterPage).PageNameText = "微信支付";
            }
            decimal total_fee = Convert.ToDecimal(Request.QueryString["total_fee"].ToString());
            string productId = GenerateOutTradeNo("WX");
            string out_trade_no = GenerateNonceStr();
            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            PayinHistoryVO vo = new PayinHistoryVO();
            vo.CustomerId = new CustomerPrincipal().CustomerProfile.CustomerId;
            vo.Cost = total_fee;
            vo.PayInOrder = productId;
            vo.PayInStatus = 0;
            vo.ThirdOrder = out_trade_no;
            vo.PayInDate = DateTime.Now;
            vo.Purpose = "微信充值";
            _bo.InsertPayinHistory(vo);


            string total_fee_1 = (total_fee * 100).ToString();
            ResultObject result = SiteCommon.GetCodeURL(out_trade_no, productId, total_fee_1);
            //string resultStr = HttpHelper.HtmlFromUrlGet((this.Master as Shared.MasterPage).APIURL + "/SPWebAPI/Project/GetCodeURL?productId=" + productId + "&total_fee=" + total_fee + "&token=" + (this.Master as Shared.MasterPage).Token);
            //ResultObject result = JsonConvert.DeserializeObject<ResultObject>(resultStr);
            if (result.Flag == 1)
            {
                //将url生成二维码图片
               
                Image2.ImageUrl = "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(result.Result.ToString());
            }
            else
            {
                dvHide.Style.Add("display", "");
                dvshow.Style.Add("display", "none");
            }


        }
        private static string GenerateOutTradeNo(string type)
        {
            var ran = new Random();

            return string.Format("{0}{1}{2}", type, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
using System;
using System.Collections.Generic;
using Aop.Api.Util;
using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.Logging.BO;
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;

namespace WebUI.Pay
{
    public partial class Return_Url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogBO _log = new LogBO(this.Page.GetType());
            /* 实际验证过程建议商户添加以下校验。
             1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
             2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
             3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
             4、验证app_id是否为该商户本身。
             */
            Dictionary<string, string> sArray = GetRequestGet();
            if (sArray.Count != 0)
            {
                try
                {

                    //bool flag = AlipaySignature.RSACheckV1(sArray, config.alipay1_public_key, config.charset, config.sign_type, false);

                    //_log.Info("----flag=" + flag.ToString() + "-------\r\n");

                    string order = Request.QueryString["out_trade_no"];
                    string Aliorder = Request.QueryString["trade_no"];
                    string totalAmount = Request.QueryString["total_amount"];
                    //bool flag = CheckN(order, Aliorder, totalAmount);
                    //_log.Info("----flag=" + flag.ToString() + "-------\r\n");

                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    List<PayinHistoryVO> list = _bo.GetPayinHistoryVO("PayInOrder='" + order+"'");
                    if (list == null || list.Count == 0)
                    {
                        //Response.Write("fail");
                        return;
                    }
                    PayinHistoryVO phvo = list[0];
                    if (Convert.ToDecimal(totalAmount) == phvo.Cost)
                    {
                        phvo.ThirdOrder = Aliorder;
                        phvo.PayInStatus = 1;
                        //_bo.UpdatePayinHistory(phvo, "PayInOrder='" + order+"'");
                        _log.Info("----金额一致，支付记录更新完成 Retun-----");
                        //_bo.PlusBalance(phvo.CustomerId, phvo.Cost);
                        _log.Info("----金额一致，支付记录更新完成 Retun-----");
                    }
                    else
                    {
                        phvo.ThirdOrder = Aliorder;
                        phvo.PayInStatus = 2;
                        //phvo.Cost = Convert.ToDecimal(totalAmount);
                        //_bo.UpdatePayinHistory(phvo, "PayInOrder='" + order + "'");
                        _log.Info("----金额不一致，支付记录更新完成 Retun-----");
                        //_bo.PlusBalance(phvo.CustomerId, phvo.Cost);
                        _log.Info("---- 金额不一致，支付记录更新完成 Retun-----");
                    }

                    lblReturn.Text = "支付成功正在跳转";
                    Response.Redirect("~/CustomerManagement/MyWallet.aspx", true);


                }
                catch (Exception exp)
                {
                    _log.Error(exp.Message + "\r\n StackTrace--" + exp.StackTrace);

                }

            }
        }

        public Dictionary<string, string> GetRequestGet()
        {
            LogBO _log = new LogBO(this.Page.GetType());
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.QueryString;
            String[] requestItem = coll.AllKeys;
            _log.Info("----aliPay Retun-----");

            for (i = 0; i < requestItem.Length; i++)
            {
                _log.Info("Key=" + requestItem[i] + "  Value=" + Request.QueryString[requestItem[i]] + "\r\n");
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }
            return sArray;

        }
        private bool CheckN(string out_trade_no, string trade_no, string total)
        {
            LogBO _log = new LogBO(this.Page.GetType());
            DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay1_public_key, config.charset, false);



            AlipayTradeQueryModel model = new AlipayTradeQueryModel();
            model.OutTradeNo = out_trade_no;
            model.TradeNo = trade_no;

            Aop.Api.Request.AlipayTradeQueryRequest request = new Aop.Api.Request.AlipayTradeQueryRequest();
            request.SetBizModel(model);
            _log.Info("---Query response start--\r\n");
            AlipayTradeQueryResponse response = null;
            try
            {

                response = client.pageExecute(request, null, "post");
                _log.Info("---response=" + response.Body + "--\r\n");
                if (response.TradeStatus == "TRADE_SUCCESS" || response.TotalAmount == total)
                    return true;
                else
                    return false;

            }
            catch (Exception exp)
            {
                return false;
            }

        }
    }
}
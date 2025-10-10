using CoreFramework.VO;
using SPlatformService;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Pay
{
    public partial class Party_Notify_Url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogBO _log = new LogBO(this.GetType());
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            _log.Info("Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                _log.Error("Sign check error : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }

            try
            {
                if (!data.IsSet("transaction_id"))
                {
                    //若transaction_id不存在，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付结果中微信订单号不存在");
                    _log.Error("The Pay result is error : " + res.ToXml());
                    Response.Write(res.ToXml());
                    Response.End();
                }

                string transaction_id = data.GetValue("transaction_id").ToString();

                //查询订单，判断订单真实性
                string order = data.GetValue("out_trade_no").ToString();
                CardBO CardBO = new CardBO(new CustomerProfile());
                List<CardPartyOrderViewVO> list = CardBO.GetPartyOrderViewVO("OrderNO='" + order + "'", false);
                if (list == null || list.Count == 0)
                {
                    _log.Info("can't found order : " + order);
                    return;
                }
                CardPartyOrderViewVO cVO = list[0];
                if (!QueryOrder(transaction_id, cVO.AppType))
                {
                    //若订单查询失败，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败");
                    _log.Error("Order query failure : " + res.ToXml());
                    Response.Write(res.ToXml());
                    Response.End();
                }
                //查询订单成功
                else
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    _log.Info("order query success : " + res.ToXml());
                    //更新订单
                    
                    //string Wxorder = data.GetValue("product_id").ToString();
                    //string totalAmount = Request.QueryString["total_amount"];
                    //bool flag = CheckN(order, Aliorder, totalAmount);
                    _log.Info("----data=" + data.ToXml() + "-------\r\n");

                    CardBO cBO = new CardBO(new CustomerProfile(),cVO.AppType);
                    if (cVO.Status!=1)
                    {
                        //修改订单
                        CardPartyOrderVO cpVO = new CardPartyOrderVO();
                        cpVO.PartyOrderID = cVO.PartyOrderID;
                        cpVO.Status = 1;
                        cpVO.payAt = DateTime.Now;
                        cpVO.transaction_id = transaction_id;
                        cBO.UpdatePartyOrder(cpVO);

                        //完成报名
                        CardPartySignUpVO suVO = new CardPartySignUpVO();
                        suVO.CardID = cVO.CardID;
                        suVO.CustomerId = cVO.CustomerId;
                        suVO.Name = cVO.Name;
                        suVO.Headimg = cVO.Headimg;
                        suVO.Phone = cVO.Phone;
                        suVO.PartyID = cVO.PartyID;
                        suVO.FormId = cVO.FormId;
                        suVO.OpenId = cVO.OpenId;
                        suVO.CreatedAt = DateTime.Now;
                        suVO.SignUpForm = cVO.SignUpForm;
                        suVO.InviterCID = cVO.InviterCID;
                        suVO.Number = cVO.Number;
                        suVO.AppType = cVO.AppType;
                        if (cVO.Type == 2)
                        {
                            suVO.isDeliver = 0;
                        }

                        int PartySignUpID = cBO.AddCardToParty(suVO);
                        if (PartySignUpID > 0)
                        {
                            cBO.sendSignUpMessage(PartySignUpID);
                            CardPartyVO cpvo = cBO.FindPartyById(cVO.PartyID);

                            string url = "/pages/Party/SignUpList/SignUpList?isadmin=true&PartyID=" + cpvo.PartyID;
                            if (cpvo.Type == 3)
                            {
                                url = "/package/package_party/SignUpList/SignUpList?isadmin=true&PartyID=" + cpvo.PartyID;
                            }
                            cBO.AddCardMessage(cVO.Name + "报名了活动《" + cpvo.Title+"》", cpvo.CustomerId, "活动报名", url);
                            string Title = "您成功报名了活动《" + cpvo.Title + "》";
                            if (cpvo.Type == 2)
                            {
                                Title = "您成功购买了商品《" + cpvo.Title + "》";
                            }
                            cBO.AddCardMessage(Title, suVO.CustomerId, "活动报名", "/pages/Party/PartyShow/PartyShow?PartyID=" + cpvo.PartyID);
                        }
                        cpVO.PartySignUpID = PartySignUpID;
                        cBO.UpdatePartyOrder(cpVO);

                        try
                        {
                            //开通会员
                            if(cVO.PartyID== 2607|| cVO.PartyID == 2639 || cVO.PartyID == 2914)
                            {
                                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                                CustomerVO uVO = uBO.FindCustomenById(cVO.CustomerId);

                                int month = 12* cVO.Number;

                                //续费
                                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now)
                                {
                                    uVO.ExpirationAt = uVO.ExpirationAt.AddMonths(month);
                                }
                                //开通
                                else
                                {
                                    uVO.ExpirationAt = DateTime.Now.AddMonths(month);
                                }
                                if (uVO.VipLevel != 2 && uVO.VipLevel != 3)
                                    uVO.VipLevel = 1;
                                uVO.isVip = true;

                                if (uBO.Update(uVO))
                                {
                                    cBO.AddCardMessage("已为您开通了" + month + "个月的vip，感谢您的支持！", cVO.CustomerId, "会员特权", "/pages/MyCenter/MyCenter/MyCenter", "switchTab");
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                    Response.Write(res.ToXml());
                    Response.End();
                }
                //Response.Redirect("~/CustomerManagement/MyWallet.aspx", true);
            }
            catch(Exception err)
            {
                _log.Error( "Save  check error : " + err.StackTrace);
            }
            _log.Info( "WX pay Check sign success");
      
        }
        private bool QueryOrder(string transaction_id,int AppType)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req,6, AppType);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
 
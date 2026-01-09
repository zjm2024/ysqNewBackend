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
    public partial class Card_Notify_Url : System.Web.UI.Page
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
                List<CardOrderVO> list = CardBO.FindOrderByCondtion("OrderNO='" + order + "'");
                if (list == null || list.Count == 0)
                {
                    _log.Info("can't found order : " + order);
                    return;
                }
                CardOrderVO cVO = list[0];
                if (!QueryOrder(transaction_id, cVO.AppType))
                {
                    //若订单查询失败，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败"+ cVO.AppType);
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

                    CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

                    if (cVO.Status!=1)
                    {
                        //修改订单
                        CardOrderVO cpVO = new CardOrderVO();
                        cpVO.CardOrderID = cVO.CardOrderID;
                        cpVO.Status = 1;
                        cpVO.payAt = DateTime.Now;
                        cBO.UpdateOrder(cpVO);

                        //会员返利
                        CustomerBO uBO = new CustomerBO(new CustomerProfile());
                        CustomerVO uVO = uBO.FindCustomenById(cVO.CustomerId);
                        if (uVO != null)
                        {
                            if (cVO.AppType == 3)
                            {
                                //微云智推分销提成
                                if (uVO.City != "")
                                {
                                    int AgentCustomerId = cBO.GetAgentCustomerIdByCity(uVO.City);
                                    if (AgentCustomerId > 0)
                                    {
                                        cpVO.AgentCost = Convert.ToDecimal((double)cVO.Cost * 0.35);
                                        cpVO.AgentCustomerId = AgentCustomerId;
                                        cBO.AddCardMessage("您代理区域的会员开通了VIP，恭喜获得" + cpVO.AgentCost + "元的佣金奖励", cpVO.AgentCustomerId, "会员返利", "/pages/MyCenter/PromotionBonus/PromotionBonus");
                                    }
                                }

                                if (uVO.originCustomerId > 0)
                                {
                                    CustomerVO OneRebateVO = uBO.FindCustomenById(uVO.originCustomerId);

                                    if (OneRebateVO != null)
                                    {
                                        //一级返利
                                        cpVO.OneRebateCustomerId = OneRebateVO.CustomerId;
                                        cpVO.OneRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.12);
                                        cBO.AddCardMessage("您推荐的会员开通了VIP，恭喜获得" + cpVO.OneRebateCost + "元的直推奖励", OneRebateVO.CustomerId, "会员返利", "/pages/MyCenter/PromotionBonus/PromotionBonus");
                                        if (OneRebateVO.City != "")
                                        {
                                            int OneRebateAgentCustomerId = cBO.GetAgentCustomerIdByCity(OneRebateVO.City);
                                            if (OneRebateAgentCustomerId > 0)
                                            {
                                                cpVO.OneRebateAgentCustomerId = OneRebateAgentCustomerId;
                                            }
                                        }


                                        if (OneRebateVO.originCustomerId > 0)
                                        {
                                            CustomerVO TwoRebateVO = uBO.FindCustomenById(OneRebateVO.originCustomerId);
                                            //二级返利
                                            cpVO.TwoRebateCustomerId = TwoRebateVO.CustomerId;
                                            cpVO.TwoRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.08);
                                            cBO.AddCardMessage("您推荐的会员开通了VIP，恭喜获得" + cpVO.TwoRebateCost + "元的间推奖励", TwoRebateVO.CustomerId, "会员返利", "/pages/MyCenter/PromotionBonus/PromotionBonus");
                                            if (TwoRebateVO.City != "")
                                            {
                                                int TwoRebateAgentCustomerId = cBO.GetAgentCustomerIdByCity(TwoRebateVO.City);
                                                if (TwoRebateAgentCustomerId > 0)
                                                {
                                                    cpVO.TwoRebateAgentCustomerId = TwoRebateAgentCustomerId;
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                            else
                            {
                                if (cVO.InviterCID > 0 && cVO.InviterCID != cVO.CustomerId)
                                {
                                    uVO.originCustomerId = cVO.InviterCID;
                                    uBO.Update(uVO);
                                }

                                if (uVO.originCustomerId > 0 && uVO.originCustomerId != cVO.CustomerId)
                                {
                                    CustomerVO OneRebateVO = uBO.FindCustomenById(uVO.originCustomerId);

                                    if (OneRebateVO != null)
                                    {
                                        //一级返利
                                        cpVO.OneRebateCustomerId = uVO.originCustomerId;
                                        if (OneRebateVO.VipLevel == 2 && OneRebateVO.isVip && OneRebateVO.ExpirationAt > DateTime.Now)
                                        {
                                            cpVO.OneRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.6);
                                        }
                                        else if (OneRebateVO.VipLevel == 3 && OneRebateVO.isVip && OneRebateVO.ExpirationAt > DateTime.Now)
                                        {
                                            cpVO.OneRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.7);
                                        }
                                        else
                                        {
                                            //cpVO.OneRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.3);
                                            var NewCost = Convert.ToDecimal((double)cVO.Cost * 0.2);
                                            if ((cpVO.Type == 2) && NewCost == 73)
                                            {

                                                NewCost = 72;
                                            }

                                            cpVO.OneRebateCost = NewCost;
                                        }
                                        _log.Info("----更新72=" + cpVO.OneRebateCost + "-------\r\n");
                                        cBO.AddCardMessage("您获得了一笔推广奖金，快去提现吧", uVO.originCustomerId, "会员返利", "/pages/MyCenter/PromotionBonus/PromotionBonus");

                                        if (OneRebateVO.originCustomerId > 0 && OneRebateVO.originCustomerId!= cVO.CustomerId)
                                        {
                                            CustomerVO TwoRebateVO = uBO.FindCustomenById(OneRebateVO.originCustomerId);
                                            //二级返利
                                            if ((TwoRebateVO.VipLevel == 1 || TwoRebateVO.VipLevel == 2 || TwoRebateVO.VipLevel == 3) && TwoRebateVO.isVip && TwoRebateVO.ExpirationAt > DateTime.Now)
                                            {
                                                cpVO.TwoRebateCustomerId = TwoRebateVO.CustomerId;
                                                //cpVO.TwoRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.2);
                                                cpVO.TwoRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.05);
                                                cBO.AddCardMessage("您获得了一笔推广奖金，快去提现吧", TwoRebateVO.CustomerId, "会员返利", "/pages/MyCenter/PromotionBonus/PromotionBonus");
                                            }
                                        }  
                                    }

                                }
                            }
                        }
                        cBO.UpdateOrder(cpVO);

                        //开通会员
                        cBO.OpeningVIP(cVO.CardOrderID);
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
        private bool QueryOrder(string transaction_id,int AppType=1)
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
 
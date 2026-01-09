using BusinessCard.Controllers;
using CoreFramework.VO;
using SPlatformService;
using SPlatformService.Models;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Pay
{
    public partial class BusinessCard_Notify_Url : System.Web.UI.Page
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
                if (!QueryOrder(transaction_id, data.GetValue("out_trade_no").ToString()))
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
                    string order = data.GetValue("out_trade_no").ToString();
                    //string Wxorder = data.GetValue("product_id").ToString();
                    //string totalAmount = Request.QueryString["total_amount"];
                    //bool flag = CheckN(order, Aliorder, totalAmount);
                    _log.Info("----data=" + data.ToXml() + "-------\r\n");


                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    List<OrderVO> list = cBO.FindOrderList("OrderNO='" + order + "'");
                    if (list == null || list.Count == 0)
                    {
                        _log.Info("can't found order : " + order);
                        return;
                    }
                    OrderVO cVO = list[0];
                    
                    if(cVO.Status!=1)
                    {
                        cVO.Status = 1;
                        cVO.payAt = DateTime.Now;
                        cVO.transaction_id = transaction_id;
                        cBO.UpdateOrder(cVO);

                        //添加分账接收方（分享返佣）
                        if (cVO.ProfitsharingCost > 0 && cVO.ProfitsharingOpenId != "")
                        {
                            //添加分账接收方
                            JsApiPay Ja = new JsApiPay();
                            int AppType = cVO.AppType;
                            if (AppType == 1)
                            {
                                //企业版小程序需要转换AppType
                                AppType = 0;
                            }
                            cBO.AddProfitsharing(cVO.OrderID);
                            Ja.GetProfitsharingResult(cVO.ProfitsharingOpenId, AppType);
                        }

                        //生效积分
                        if (cVO.IntegralID > 0)
                        {
                            IntegralVO IVO = new IntegralVO();
                            IVO.IntegralID = cVO.IntegralID;
                            IVO.Status = 1;
                            cBO.UpdateIntegral(IVO);
                        }


                        //扣除库存
                        cBO.StoreAmount(cVO.InfoID, cVO.CostID, cVO.Number);

                        if (cVO.AppType == 1)
                            cVO.AppType = 0;

                        //如果是选择为自己购买就直接开通或续费
                        if (cVO.isUsed==1|| cVO.isUsed == 2)
                            cBO.EstablishedBusinessCard(cVO.InfoID, cVO.PersonalID, cVO.OrderID, cVO.AppType);


                        //发放vip
                        if (cVO.GiveShopVipID > 0)
                        {
                            cBO.AddShopVipPersonal(cVO.PersonalID, cVO.GiveShopVipID, cVO.GiveShopVipDay, cVO.OrderID);
                        }

                        //创客会员
                        if (cVO.InfoID == 905)
                        {
                            try
                            {
                                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                                CardBO CardBO = new CardBO(new CustomerProfile());
                                PersonalVO pVO = cBO.FindPersonalById(cVO.PersonalID);
                                CustomerVO uVO = uBO.FindCustomenById(pVO.CustomerId);

                                CardOrderVO cpVO = new CardOrderVO();
                                cpVO.CustomerId = uVO.CustomerId;
                                cpVO.OpenId = "";
                                cpVO.CreatedAt = DateTime.Now;
                                cpVO.Type = 2;
                                cpVO.Cost = cVO.Cost;
                                Random ran = new Random();
                                cpVO.OrderNO = "BusinessCardOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

                                cpVO.CardOrderID = 0;
                                cpVO.Status = 1;
                                cpVO.payAt = DateTime.Now;

                                int OrderID = CardBO.AddOrder(cpVO);
                                if (OrderID > 0)
                                {
                                    cpVO.CardOrderID = OrderID;
                                    if (uVO.originCustomerId > 0)
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
                                                cpVO.OneRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.3);
                                            }

                                            CardBO.AddCardMessage("您获得了一笔推广奖金，快去提现吧", uVO.originCustomerId, "会员返利", "/pages/MyCenter/PromotionBonus/PromotionBonus");

                                            if (OneRebateVO.originCustomerId > 0)
                                            {
                                                CustomerVO TwoRebateVO = uBO.FindCustomenById(OneRebateVO.originCustomerId);
                                                //二级返利
                                                if (TwoRebateVO.VipLevel == 3 && TwoRebateVO.isVip && TwoRebateVO.ExpirationAt > DateTime.Now)
                                                {
                                                    cpVO.TwoRebateCustomerId = TwoRebateVO.CustomerId;
                                                    cpVO.TwoRebateCost = Convert.ToDecimal((double)cVO.Cost * 0.2);
                                                    CardBO.AddCardMessage("您获得了一笔推广奖金，快去提现吧", TwoRebateVO.CustomerId, "会员返利", "/pages/MyCenter/PromotionBonus/PromotionBonus");
                                                }
                                            }
                                        }

                                    }
                                    CardBO.UpdateOrder(cpVO);

                                    //开通会员
                                    CardBO.OpeningVIP(OrderID);
                                }
                            }
                            catch
                            {

                            }
                            
                        }

                        InfoVO sVO = cBO.FindInfoById(cVO.InfoID);
                        //如果是购买vip,则立即分账
                        if (sVO.OfficialProducts == "Basic" || sVO.OfficialProducts == "Standard" || sVO.OfficialProducts == "Advanced" || sVO.OfficialProducts == "SelfEmployed" || sVO.OfficialProducts == "SelfEmployed2")
                        {
                             Task.Run(async () =>
                            {
                                Thread.Sleep(30000);
                                await new ProfitsharingToOrder().ProfitsharingToOrderIDTask(cVO.OrderID);
                            });

                        }
                    }

                    Response.Write(res.ToXml());
                    Response.End();
                }
            }
            catch(Exception err)
            {
                string strErrorMsg = "Message:" + err.Message.ToString() + "\r\n  Stack :" + err.StackTrace + " \r\n Source :" + err.Source;
                _log.Error( "Save  check error : " + strErrorMsg);
            }
            _log.Info( "WX pay Check sign success");
      
        }
        private bool QueryOrder(string transaction_id,string out_trade_no)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<OrderVO> list = cBO.FindOrderList("OrderNO='" + out_trade_no + "'");
            if (list == null || list.Count == 0)
            {
                return false;
            }

            WxPayData res = WxPayApi.OrderQuery(req,6, list[0].AppType);
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
 
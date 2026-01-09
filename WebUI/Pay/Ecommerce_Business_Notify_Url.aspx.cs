using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WxEcommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Pay
{
    public partial class Ecommerce_Business_Notify_Url : System.Web.UI.Page
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

            _log.Info("Receive data from WeChat2 : " + builder.ToString());

            NotifyDataVO NotifyDataVO = JsonConvert.DeserializeObject<NotifyDataVO>(builder.ToString());
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string ns = jsonSerializer.Serialize(NotifyDataVO);
            EcommerceBO eBO = new EcommerceBO();
            string Decrypt = "";
            try
            {
                Decrypt = eBO.Decrypt(NotifyDataVO.resource.associated_data, NotifyDataVO.resource.nonce, NotifyDataVO.resource.ciphertext);
            }catch(Exception ex)
            {
                _log.Info("解密失败 : " + ex);
                return;
            }
            
            _log.Info("解密成功 : " + Decrypt);
            //转换数据格式
            ResourceDecrypt ResourceDecrypt = JsonConvert.DeserializeObject<ResourceDecrypt>(Decrypt);
            try
            {

                string transaction_id = ResourceDecrypt.transaction_id;
                string sp_appid= ResourceDecrypt.sp_appid;
                //查询订单，判断订单真实性
                string order = ResourceDecrypt.out_trade_no;
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                List<OrderVO> list = cBO.FindOrderList("OrderNO='" + order + "'");
                if (list == null || list.Count == 0)
                {
                    _log.Info("can't found order : " + order);
                    return;
                }
                OrderVO cVO = list[0];

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                _log.Info("order query success : " + res.ToXml());
                //更新订单

                //string Wxorder = data.GetValue("product_id").ToString();
                //string totalAmount = Request.QueryString["total_amount"];
                //bool flag = CheckN(order, Aliorder, totalAmount);
                if (cVO.Status != 1)
                {
                    //修改订单
                    cVO.Status = 1;
                    cVO.payAt = DateTime.Now;
                    cVO.transaction_id = transaction_id;
                    cBO.UpdateOrder(cVO);

                    //判断是否需要分账(平台抽成)
                    if (cVO.SplitCost > 0)
                    {
                        Task.Run(async () =>
                        {
                            Thread.Sleep(30000);
                            ResultCode response = await eBO.GetProfitsharing(sp_appid, cVO.sub_mchid, transaction_id, cVO.OrderNO, Convert.ToInt32(cVO.SplitCost * 100));
                            _log.Info("分账提交："+ response.PostBody + "分账结果 : " + response.ResultStr);
                            if (response.code == "SUCCESS")
                            {
                                cVO.IsSplitOut = 1;
                                cBO.UpdateOrder(cVO);
                            }
                        });
                        
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
                    if (cVO.isUsed == 1 || cVO.isUsed == 2)
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
                }

                Response.Write(res.ToXml());
                Response.End();
            }
            catch(Exception err)
            {
                _log.Error( "Save  check error : " + err.Message);
            }
            _log.Info( "WX pay Check sign success");
      
        }
    }
}
 
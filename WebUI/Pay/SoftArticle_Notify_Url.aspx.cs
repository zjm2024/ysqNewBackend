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
    public partial class SoftArticle_Notify_Url : System.Web.UI.Page
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
                List<CardSoftArticleOrderVO> list = CardBO.FindSoftArticleOrderByConditionStr("OrderNO='" + order + "'", false);
                if (list == null || list.Count == 0)
                {
                    _log.Info("can't found order : " + order);
                    return;
                }
                CardSoftArticleOrderVO cVO = list[0];
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
                    CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

                    if (cVO.Status!=1)
                    {
                        //修改订单
                        CardSoftArticleOrderVO cpVO = new CardSoftArticleOrderVO();
                        cpVO.SoftArticleOrderID = cVO.SoftArticleOrderID;
                        cpVO.Status = 1;
                        cpVO.payAt = DateTime.Now;

                        //转载文章
                        CardSoftArticleVO sVO = cBO.FindSoftArticleById(cVO.SoftArticleID);

                        sVO.ReprintCount += 1;
                        cBO.UpdateSoftArticle(sVO);

                        CardSoftArticleVO SoftArticleVO = new CardSoftArticleVO();
                        SoftArticleVO.SoftArticleID = 0;
                        SoftArticleVO.CustomerId = cVO.CustomerId;
                        SoftArticleVO.CreatedAt = DateTime.Now;
                        SoftArticleVO.Title = sVO.Title;
                        SoftArticleVO.Image = sVO.Image;
                        SoftArticleVO.Description = sVO.Description;
                        SoftArticleVO.IsCost = sVO.IsCost;
                        SoftArticleVO.Cost = sVO.Cost;
                        SoftArticleVO.PartyID = sVO.PartyID;
                        SoftArticleVO.OriginalCustomerId = sVO.OriginalCustomerId;
                        SoftArticleVO.OriginalSoftArticleID = cVO.SoftArticleID;
                        SoftArticleVO.IsOriginal = sVO.IsOriginal;
                        SoftArticleVO.OriginalName = sVO.OriginalName;
                        SoftArticleVO.OriginalPlatform = sVO.OriginalPlatform;
                        SoftArticleVO.OriginalMedia = sVO.OriginalMedia;
                        SoftArticleVO.Video = sVO.Video;
                        SoftArticleVO.isVideo = sVO.isVideo;          

                        int NewSoftArticleID = cBO.AddSoftArticle(SoftArticleVO);
                        cpVO.NewSoftArticleID = NewSoftArticleID;
                        cBO.UpdateCardSoftArticleOrder(cpVO);
                        //支付给原创
                        cBO.PlusCardBalance(SoftArticleVO.OriginalCustomerId, cVO.Cost);

                        CustomerBO uBO = new CustomerBO(new CustomerProfile());
                        CustomerVO cuVO = uBO.FindCustomenById(cVO.CustomerId);
                        cBO.AddCardMessage(cuVO.CustomerName + "转载了文章《" + SoftArticleVO.Title + "》", SoftArticleVO.OriginalCustomerId, "软文转载", "/package/package_order/HostOrderList/HostOrderList");
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
 
using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService;
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
    public partial class Ecommerce_Party_Notify_Url : System.Web.UI.Page
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
                CardBO CardBO = new CardBO(new CustomerProfile());
                List<CardPartyOrderViewVO> list = CardBO.GetPartyOrderViewVO("OrderNO='" + order + "'", false);
                if (list == null || list.Count == 0)
                {
                    _log.Info("can't found order : " + order);
                    return;
                }
                CardPartyOrderViewVO cVO = list[0];

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                _log.Info("order query success : " + res.ToXml());
                //更新订单

                //string Wxorder = data.GetValue("product_id").ToString();
                //string totalAmount = Request.QueryString["total_amount"];
                //bool flag = CheckN(order, Aliorder, totalAmount);

                CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);
                if (cVO.Status != 1)
                {
                    //修改订单
                    CardPartyOrderVO cpVO = new CardPartyOrderVO();
                    cpVO.PartyOrderID = cVO.PartyOrderID;
                    cpVO.Status = 1;
                    cpVO.payAt = DateTime.Now;
                    cpVO.IsPayOut = 1;
                    cpVO.transaction_id = transaction_id;
                    cpVO.sp_appid = sp_appid;
                    cBO.UpdatePartyOrder(cpVO);

                    //判断是否需要分账
                    if (cVO.SplitCost > 0 || cVO.PromotionAwardCost>0)
                    {
                        Task.Run(async () =>
                        {
                            Thread.Sleep(30000);
                            ResultCode response = await eBO.GetProfitsharing(sp_appid, cVO.sub_mchid, transaction_id, cVO.OrderNO, Convert.ToInt32(cVO.SplitCost * 100)+ Convert.ToInt32(cVO.PromotionAwardCost * 100));
                            _log.Info("分账提交："+ response.PostBody + "分账结果 : " + response.ResultStr);
                            if (response.code == "SUCCESS")
                            {
                               cpVO.IsSplitOut = 1;
                               cBO.UpdatePartyOrder(cpVO);
                            }
                        });
                        
                    }

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
                        cBO.AddCardMessage(cVO.Name + "报名了活动《" + cpvo.Title + "》", cpvo.CustomerId, "活动报名", url);
                        string Title = "您成功报名了活动《" + cpvo.Title + "》";
                        if (cpvo.Type == 2)
                        {
                            Title = "您成功购买了商品《" + cpvo.Title + "》";
                        }
                        cBO.AddCardMessage(Title, suVO.CustomerId, "活动报名", "/pages/Party/PartyShow/PartyShow?PartyID=" + cpvo.PartyID);
                    }
                    cpVO.PartySignUpID = PartySignUpID;
                    cBO.UpdatePartyOrder(cpVO);
                }

                Response.Write(res.ToXml());
                Response.End();
            }
            catch(Exception err)
            {
                _log.Error( "Save  check error : " + err.StackTrace);
            }
            _log.Info( "WX pay Check sign success");
      
        }
    }
}
 
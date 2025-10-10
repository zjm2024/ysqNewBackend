using CoreFramework.VO;
using NPOI.SS.Formula.Functions;
using SPlatformService.Models;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CustomerManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BusinessCard.Controllers
{
    public class ProfitsharingToOrder
    {
        /// <summary>
        /// 对订单执行分账
        /// </summary>
        /// <returns></returns>
        public bool ProfitsharingToOrderID(int OrderID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            OrderVO OrderVO = cBO.FindOrderById(OrderID);
            bool issuss = false;
            bool ispay = false;

            var receivers = "[";
            if (OrderVO.ProfitsharingCost > 0 && OrderVO.ProfitsharingOpenId != "" && OrderVO.ProfitsharingStatus == 0 && OrderVO.Status == 1 && OrderVO.isAgentBuy == 0 && OrderVO.isEcommerceBuy == 0)
            {
                receivers += "{";
                receivers += "\"type\": \"PERSONAL_OPENID\",";
                receivers += "\"account\":\"" + OrderVO.ProfitsharingOpenId + "\",";
                receivers += "\"amount\":" + Convert.ToInt32(OrderVO.ProfitsharingCost * 100) + ",";
                receivers += "\"description\": \"商品推广奖励\"";
                receivers += "}";
                ispay = true;
            }

            if (OrderVO.TowProfitsharingCost > 0 && OrderVO.TowProfitsharingOpenId != "" && OrderVO.TowProfitsharingStatus == 0 && OrderVO.Status == 1 && OrderVO.isAgentBuy == 0 && OrderVO.isEcommerceBuy == 0)
            {
                //二级分账
                if (receivers == "[")
                {
                    receivers += "{";
                }
                else
                {
                    receivers += ",{";
                }

                receivers += "\"type\": \"PERSONAL_OPENID\",";
                receivers += "\"account\":\"" + OrderVO.TowProfitsharingOpenId + "\",";
                receivers += "\"amount\":" + Convert.ToInt32(OrderVO.TowProfitsharingCost * 100) + ",";
                receivers += "\"description\": \"二级推广奖励\"";
                receivers += "}";
                ispay = true;
            }



            receivers += "]";

            if (ispay)
            {
                //请求分账
                JsApiPay Ja = new JsApiPay();
                int AppType = OrderVO.AppType;
                if (AppType == 1)
                {
                    AppType = 0;
                }
                AppVO AppVO = AppBO.GetApp(AppType);
                bool wp = Ja.GetProfitsharingResult(receivers, OrderVO.transaction_id, OrderVO.OrderNO, AppType, AppVO.AppId);
                if (wp)
                {
                    OrderVO newOrderVO = new OrderVO();
                    newOrderVO.OrderID = OrderVO.OrderID;
                    newOrderVO.ProfitsharingAt = DateTime.Now;
                    newOrderVO.ProfitsharingStatus = 1;

                    if (OrderVO.TowProfitsharingCost > 0)
                    {
                        newOrderVO.TowProfitsharingAt = DateTime.Now;
                        newOrderVO.TowProfitsharingStatus = 1;
                    }

                    cBO.UpdateOrder(newOrderVO);
                    issuss = true;
                }
            }
            return issuss;
        }
        /// <summary>
        /// 对订单执行分账
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ProfitsharingToOrderIDTask(int OrderID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            OrderVO OrderVO = cBO.FindOrderById(OrderID);
            bool issuss = false;
            bool ispay = false;

            var receivers = "[";
            if (OrderVO.ProfitsharingCost > 0 && OrderVO.ProfitsharingOpenId != "" && OrderVO.ProfitsharingStatus == 0 && OrderVO.Status == 1 && OrderVO.isAgentBuy == 0 && OrderVO.isEcommerceBuy == 0)
            {
                receivers += "{";
                receivers += "\"type\": \"PERSONAL_OPENID\",";
                receivers += "\"account\":\"" + OrderVO.ProfitsharingOpenId + "\",";
                receivers += "\"amount\":" + Convert.ToInt32(OrderVO.ProfitsharingCost * 100) + ",";
                receivers += "\"description\": \"商品推广奖励\"";
                receivers += "}";
                ispay = true;
            }

            if (OrderVO.TowProfitsharingCost > 0 && OrderVO.TowProfitsharingOpenId != "" && OrderVO.TowProfitsharingStatus == 0 && OrderVO.Status == 1 && OrderVO.isAgentBuy == 0 && OrderVO.isEcommerceBuy == 0)
            {
                //二级分账
                if (receivers == "[")
                {
                    receivers += "{";
                }
                else
                {
                    receivers += ",{";
                }
                
                receivers += "\"type\": \"PERSONAL_OPENID\",";
                receivers += "\"account\":\"" + OrderVO.TowProfitsharingOpenId + "\",";
                receivers += "\"amount\":" + Convert.ToInt32(OrderVO.TowProfitsharingCost * 100) + ",";
                receivers += "\"description\": \"二级推广奖励\"";
                receivers += "}";
                ispay = true;
            }
            receivers += "]";
            if (ispay)
            {
                //请求分账
                JsApiPay Ja = new JsApiPay();
                int AppType = OrderVO.AppType;
                if (AppType == 1)
                {
                    AppType = 0;
                }
                AppVO AppVO = AppBO.GetApp(AppType);
                bool wp = Ja.GetProfitsharingResult(receivers, OrderVO.transaction_id, OrderVO.OrderNO, AppType, AppVO.AppId);
                if (wp)
                {
                    OrderVO newOrderVO = new OrderVO();
                    newOrderVO.OrderID = OrderVO.OrderID;
                    newOrderVO.ProfitsharingAt = DateTime.Now;
                    newOrderVO.ProfitsharingStatus = 1;

                    if (OrderVO.TowProfitsharingCost > 0)
                    {
                        newOrderVO.TowProfitsharingAt = DateTime.Now;
                        newOrderVO.TowProfitsharingStatus = 1;
                    }

                    cBO.UpdateOrder(newOrderVO);
                    issuss = true;
                }
            }
            return issuss;
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CustomerManagement.BO;
using CoreFramework.VO;
using SPLibrary.WebConfigInfo;
using SPLibrary.BusinessCardManagement.BO;

namespace SPlatformService
{
    public partial class Default : BasePage
    {
        public decimal RevenueToday = 0;//今日营收
        public decimal RevenueToday2 = 0;//今日收入
        public decimal RevenueToday3 = 0;//今日支出

        public decimal RevenueYesterday = 0;//昨日营收
        public decimal RevenueYesterday2 = 0;//昨日收入
        public decimal RevenueYesterday3 = 0;//昨日支出
        public decimal RevenueYesterdayPercentage = 0;//昨日营收增加百分比

        public decimal RevenueLastweek = 0;//上周营收
        public decimal RevenueLastweek2 = 0;//上周收入
        public decimal RevenueLastweek3 = 0;//上周支出
        public decimal RevenueLastweekPercentage = 0;//上周营收增加百分比

        public decimal RevenueLastmonth = 0;//上月营收
        public decimal RevenueLastmonth2 = 0;//上月收入
        public decimal RevenueLastmonth3 = 0;//上月支出
        public decimal RevenueLastmonthPercentage = 0;//上月营收增加百分比

        public decimal RevenueThismonth = 0;//本月累计营收
        public decimal RevenueThismonth2 = 0;//本月累计收入
        public decimal RevenueThismonth3 = 0;//本月累计支出

        public decimal Revenue = 0;//累计营收
        public decimal Revenue2 = 0;//累计收入
        public decimal Revenue3 = 0;//累计支出

        public int CustomerToday = 0;//今日新增会员
        public int CustomerYesterday = 0;//昨日新增会员
        public decimal CustomerYesterdayPercentage = 0;//昨日新增会员增加百分比
        public int CustomerLastweek = 0;//上周新增会员
        public decimal CustomerLastweekPercentage = 0;//上周新增会员增加百分比
        public int CustomerLastmonth = 0;//上月新增会员
        public decimal CustomerLastmonthPercentage = 0;//上月新增会员增加百分比
        public int CustomerThismonth = 0;//本月累计新增会员
        public int Customer = 0;//累计会员

        public int VipToday = 0;//今日新增Vip会员
        public int VipYesterday = 0;//昨日新增Vip会员
        public decimal VipYesterdayPercentage = 0;//昨日新增Vip会员增加百分比
        public int VipLastweek = 0;//上周新增Vip会员
        public decimal VipLastweekPercentage = 0;//上周新增Vip会员增加百分比
        public int VipLastmonth = 0;//上月新增Vip会员
        public decimal VipLastmonthPercentage = 0;//上月新增Vip会员增加百分比
        public int VipThismonth = 0;//本月累计新增Vip会员
        public int Vip = 0;//累计Vip会员

        public decimal Income = 0;//平台总收入
        public decimal PartyIncome = 0;//活动总收入
        public decimal SoftarticleIncome = 0;//软文总收入
        public decimal VIPIncome = 0;//VIP总收入
        public decimal Payout = 0;//被提现
        public decimal Balance = 0;//用户现存

        public decimal OneRebateCost = 0;//直推奖
        public decimal TwoRebateCost = 0;//间推奖
        public decimal AgentCost = 0;//代理商佣金
        public decimal AgentDepositCost = 0;//代理商预存佣金

        public int AppType = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "工作面板";
            }

            base.ValidPageRight("工作面板", "Read");

            AppType = UserProfile.AppType;
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            BusinessCardBO BusinessCardBO = new BusinessCardBO(new CustomerProfile());
            try
            {
                //个人版
                Revenue3 = cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL") + cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL") + cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL");
                Revenue2= cBO.FindOrderSumByCondtion("Cost", "Status=1 and payAt is not NULL");

                //企业版
                Revenue2 += BusinessCardBO.getBusinessCost(1, 0,"",0);
                Revenue3 += BusinessCardBO.getBusinessCost(1, 0, "", 2);

                Revenue = Revenue2- Revenue3;

                RevenueToday3 = cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and DATE_FORMAT(payAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')") + cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and DATE_FORMAT(payAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')") + cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and DATE_FORMAT(payAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')");
                RevenueToday2 = cBO.FindOrderSumByCondtion("Cost", "Status=1 and DATE_FORMAT(payAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')");

                //企业版
                RevenueToday2 += BusinessCardBO.getBusinessCost(1, 0, " and DATE_FORMAT(payAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')", 0);
                RevenueToday3 += BusinessCardBO.getBusinessCost(1, 0, " and DATE_FORMAT(payAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')", 2);

                RevenueToday = RevenueToday2- RevenueToday3;//今日营收

                RevenueYesterday3 = cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 1") + cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 1") + cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 1");//昨日营收
                RevenueYesterday2 = cBO.FindOrderSumByCondtion("Cost", "Status=1 and TO_DAYS(NOW()) - TO_DAYS(payAt) = 1");//昨日营收

                //企业版
                RevenueYesterday2 += BusinessCardBO.getBusinessCost(1, 0, " and TO_DAYS(NOW()) - TO_DAYS(payAt) = 1", 0);
                RevenueYesterday3 += BusinessCardBO.getBusinessCost(1, 0, " and TO_DAYS(NOW()) - TO_DAYS(payAt) = 1", 2);

                RevenueYesterday = RevenueYesterday2- RevenueYesterday3;//昨日营收
                    

                decimal RevenueBeforeYesterday = cBO.FindOrderSumByCondtion("Cost", "Status=1 and TO_DAYS(NOW()) - TO_DAYS(payAt) = 2") - cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 2") - cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 2") - cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 2");//前天

                //企业版
                RevenueBeforeYesterday += BusinessCardBO.getBusinessCost(1, 0, "  and TO_DAYS(NOW()) - TO_DAYS(payAt) = 2", 1);

                RevenueYesterdayPercentage = getPercentage(RevenueYesterday, RevenueBeforeYesterday);

                RevenueLastweek3 =cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-1") + cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-1") + cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-1"); //上周营收
                RevenueLastweek2 = cBO.FindOrderSumByCondtion("Cost", "Status=1 and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-1"); //上周营收

                //企业版
                RevenueLastweek2 += BusinessCardBO.getBusinessCost(1, 0, " and YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-1", 0);
                RevenueLastweek3 += BusinessCardBO.getBusinessCost(1, 0, " and YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-1", 2);
                
                RevenueLastweek = RevenueLastweek2 - RevenueLastweek3;


                decimal RevenueBeforeLastweek = cBO.FindOrderSumByCondtion("Cost", "Status=1 and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-2") - cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-2") - cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-2") - cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-2");//上上周营收

                //企业版
                RevenueBeforeLastweek += BusinessCardBO.getBusinessCost(1, 0, " and  YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-2", 1);

                RevenueLastweekPercentage = getPercentage(RevenueLastweek, RevenueBeforeLastweek);

                RevenueLastmonth3 =  cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')") + cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')") + cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')");//上月营收
                RevenueLastmonth2 = cBO.FindOrderSumByCondtion("Cost", "Status=1 and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')");

                //企业版
                RevenueLastmonth2 += BusinessCardBO.getBusinessCost(1, 0, "  and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')", 0);
                RevenueLastmonth3 += BusinessCardBO.getBusinessCost(1, 0, "  and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')", 2);

                RevenueLastmonth = RevenueLastmonth2 - RevenueLastmonth3;


                decimal RevenueBeforeLastmonth = cBO.FindOrderSumByCondtion("Cost", "Status=1 and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')") - cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')") - cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')") - cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')");//上月营收

                //企业版
                RevenueBeforeLastmonth += BusinessCardBO.getBusinessCost(1, 0, "  and  date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')", 1);

                RevenueLastmonthPercentage = getPercentage(RevenueLastmonth, RevenueBeforeLastmonth);

                RevenueThismonth3 = cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(now(),'%Y-%m')") + cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(now(),'%Y-%m')") + cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL and  date_format(payAt,'%Y-%m')=date_format(now(),'%Y-%m')");//本月累计营收
                RevenueThismonth2 = cBO.FindOrderSumByCondtion("Cost", "Status=1 and  date_format(payAt,'%Y-%m')=date_format(now(),'%Y-%m')");//本月累计营收

                //企业版
                RevenueThismonth2 += BusinessCardBO.getBusinessCost(1, 0, " and  date_format(payAt,'%Y-%m')=date_format(now(),'%Y-%m')", 0);
                RevenueThismonth3 += BusinessCardBO.getBusinessCost(1, 0, " and  date_format(payAt,'%Y-%m')=date_format(now(),'%Y-%m')", 2);
                
                RevenueThismonth = RevenueThismonth2 - RevenueThismonth3;
            }
            catch
            {

            }

            //用户统计((originType<>'C_Service' or originType IS NULL) or originType IS NULL)
            try
            {
                Customer = uBO.GetCustomerCount("AppType=" + AppType+ " and (originType<>'C_Service' or originType IS NULL)");//累计会员
                CustomerToday = uBO.GetCustomerCount("AppType=" + AppType+ " and DATE_FORMAT(CreatedAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')" + " and (originType<>'C_Service' or originType IS NULL)");//今日新增会员
                CustomerYesterday = uBO.GetCustomerCount("AppType=" + AppType + " and TO_DAYS(NOW()) - TO_DAYS(CreatedAt) = 1" + " and (originType<>'C_Service' or originType IS NULL)");//昨日新增会员
                int CustomerBeforeYesterday = uBO.GetCustomerCount("AppType=" + AppType + " and TO_DAYS(NOW()) - TO_DAYS(CreatedAt) = 2" + " and (originType<>'C_Service' or originType IS NULL)");//前天新增会员
                CustomerYesterdayPercentage = getPercentage(CustomerYesterday, CustomerBeforeYesterday);//昨日新增会员增加百分比

                CustomerLastweek = uBO.GetCustomerCount("AppType=" + AppType + " and  YEARWEEK(date_format(CreatedAt,'%Y-%m-%d')) = YEARWEEK(now())-1" + " and (originType<>'C_Service' or originType IS NULL)");//上周新增会员
                int CustomerBeforeLastweek= uBO.GetCustomerCount("AppType=" + AppType + "  and  YEARWEEK(date_format(CreatedAt,'%Y-%m-%d')) = YEARWEEK(now())-2" + " and (originType<>'C_Service' or originType IS NULL)");//上上周新增会员
                CustomerLastweekPercentage = getPercentage(CustomerLastweek, CustomerBeforeLastweek);//上周新增会员增加百分比
                CustomerLastmonth = uBO.GetCustomerCount("AppType=" + AppType + " and  date_format(CreatedAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')" + " and (originType<>'C_Service' or originType IS NULL)");//上月新增会员
                int CustomerBeforeLastmonth = uBO.GetCustomerCount("AppType=" + AppType + " and  date_format(CreatedAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')" + " and (originType<>'C_Service' or originType IS NULL)");//上上月新增会员
                CustomerLastmonthPercentage = getPercentage(CustomerLastmonth, CustomerBeforeLastmonth);//上月新增会员增加百分比
                CustomerThismonth = uBO.GetCustomerCount("AppType=" + AppType + " and  date_format(CreatedAt,'%Y-%m')=date_format(now(),'%Y-%m')" + " and (originType<>'C_Service' or originType IS NULL)");//本月累计新增会员
            }
            catch
            {

            }

            //vip统计
            try
            {
                Vip = uBO.GetCustomerCount("AppType=" + AppType + " and  DATE_FORMAT(ExpirationAt,'%y-%m-%d')>DATE_FORMAT('2018-01-01','%y-%m-%d')");//累计vip会员
                VipToday = cBO.FindCardOrderTotalCount("Status=1 and payAt is not NULL and DATE_FORMAT(payAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')");//今日新增vip订单
                VipYesterday = cBO.FindCardOrderTotalCount("Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 1");//昨日新增vip订单
                int VipBeforeYesterday = cBO.FindCardOrderTotalCount("Status=1 and payAt is not NULL and TO_DAYS(NOW()) - TO_DAYS(payAt) = 2");//前天新增vip订单
                VipYesterdayPercentage = getPercentage(VipYesterday, VipBeforeYesterday);//昨日新增vip会员增加百分比

                VipLastweek = cBO.FindCardOrderTotalCount("Status=1 and YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-1");//上周新增vip会员
                int VipBeforeLastweek = cBO.FindCardOrderTotalCount("Status=1 and YEARWEEK(date_format(payAt,'%Y-%m-%d')) = YEARWEEK(now())-2");//上上周新增vip会员
                VipLastweekPercentage = getPercentage(VipLastweek, VipBeforeLastweek);//上周新增vip会员增加百分比
                VipLastmonth = cBO.FindCardOrderTotalCount("Status=1 and date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')");//上月新增vip会员
                int VipBeforeLastmonth = cBO.FindCardOrderTotalCount("Status=1 and date_format(payAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')");//上上月新增vip会员
                VipLastmonthPercentage = getPercentage(VipLastmonth, VipBeforeLastmonth);//上月新增vip会员增加百分比
                VipThismonth = cBO.FindCardOrderTotalCount("Status=1 and date_format(payAt,'%Y-%m')=date_format(now(),'%Y-%m')");//本月累计新增vip会员
            }
            catch
            {

            }

            //财务统计
            try
            {
                
                PartyIncome = cBO.FindPartyOrderSumCost("Status=1 and payAt is not NULL");//活动总收入
                SoftarticleIncome = cBO.FindSoftArticleOrderSumCost("Status=1 and payAt is not NULL");//软文总收入
                VIPIncome = cBO.FindOrderSumByCondtion("Cost", "Status=1 and payAt is not NULL");//VIP总收入
                Income = PartyIncome+ SoftarticleIncome+ VIPIncome;//平台总收入
                Payout = cBO.FindPayOutSumByCondtion("Cost", "(PayOutStatus=1 or PayOutStatus=0)");//被提现
                Balance = cBO.FindBalanceSumCost("1=1")+ cBO.FindPartyOrderSumCost("Status=1 and payAt is not NULL and IsPayOut=0")+ cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and OneRebateStatus=0") + cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and TwoRebateStatus=0"); //用户现存

                if (AppType == 3)
                {
                    OneRebateCost = cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL");
                    TwoRebateCost = cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL");
                    AgentCost = cBO.FindOrderSumByCondtion("AgentCost", "Status=1 and payAt is not NULL");
                    decimal OneRebateCost2 = cBO.FindOrderSumByCondtion("OneRebateCost", "Status=1 and payAt is not NULL and OneRebateStatus=0");
                    decimal TwoRebateCost2 = cBO.FindOrderSumByCondtion("TwoRebateCost", "Status=1 and payAt is not NULL and TwoRebateStatus=0");
                    Balance = OneRebateCost2 + TwoRebateCost2;

                    AgentDepositCost = cBO.FindAgentDepositSumCost("1=1");
                }
            }
            catch
            {

            }
        }
        public decimal getPercentage (decimal today, decimal Before)
        {
            decimal Percentage = 0;
            if (today > Before)
            {
                if (Before == 0)
                {
                    Before = 1;
                }
                Percentage = (today - Before) / Before * 100;//上周营收增加百分比
                Percentage = Math.Round(Percentage, 2);
            }
            else
            {
                if (Before == 0 && today == 0)
                {
                    Percentage = 0;
                }
                else
                {
                    if (Before == 0)
                    {
                        Before = 1;
                    }
                    Percentage = (Before - today) / Before * 100;//上周营收增加百分比
                    Percentage = -Math.Round(Percentage, 2);
                }
            }
            return Percentage;
        }
    }
}
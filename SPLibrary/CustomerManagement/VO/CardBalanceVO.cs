using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardBalanceVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardBalanceVO));
       
		[DataMember]
		 public Int32 BalanceId { get { return (Int32)GetValue(typeof(Int32),"BalanceId") ; } set {  SetValue("BalanceId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public Decimal Balance { get { return (Decimal)GetValue(typeof(Decimal),"Balance") ; } set {  SetValue("Balance",value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardBalanceVO tmp = new CardBalanceVO();
            tmp.changeData = new Dictionary<string, object>(this.changeData);
            tmp.originData = new Dictionary<string, object>(this.originData);
            return tmp;
        }
        #endregion
         
    	#region ICommonVO Member
        List<string> ICommonVO.PropertyList
        {
            get { return _propertyList; }
        }
        #endregion
    }
    public class CardBalanceList : CommonVO, ICommonVO, ICloneable
    {
        public string OrderNO { get { return (String)GetValue(typeof(String), "OrderNO"); } set { SetValue("OrderNO", value); } }
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        public Decimal PromotionAwardCost { get { return (Decimal)GetValue(typeof(Decimal), "PromotionAwardCost"); } set { SetValue("PromotionAwardCost", value); } }
        public String CostName { get { return (String)GetValue(typeof(String), "CostName"); } set { SetValue("CostName", value); } }
        public String CostStyle { get { return (String)GetValue(typeof(String), "CostStyle"); } set { SetValue("CostStyle", value); } }
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } }
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        public Int32 ToID { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
    }
    /// <summary>
    /// 冻结金额
    /// </summary>
    public class FrozenBalanceVO
    {
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal SumBalance { get; set; }

        /// <summary>
        /// 活动冻结金额
        /// </summary>
        public Decimal PartyFrozenBalance { get; set; }

        /// <summary>
        /// 活动推广奖励冻结金额
        /// </summary>
        public Decimal PartyPromotionFrozenBalance { get; set; }

        /// <summary>
        /// 商品冻结金额
        /// </summary>
        public Decimal ProductFrozenBalance { get; set; }

        /// <summary>
        /// 商品推广奖励冻结金额
        /// </summary>
        public Decimal ProductPromotionFrozenBalance { get; set; }

        /// <summary>
        /// 一级返利冻结金额
        /// </summary>
        public Decimal VipOneFrozenBalance { get; set; }

        /// <summary>
        /// 二级返利冻结金额
        /// </summary>
        public Decimal VipTwoFrozenBalance { get; set; }

        /// <summary>
        /// 一级邀约人数
        /// </summary>
        public int OneRebateCount { get; set; }

        /// <summary>
        /// 二级邀约人数
        /// </summary>
        public int TwoRebateCount { get; set; }

        /// <summary>
        /// 还差多少人可以解锁二级
        /// </summary>
        public int UnlockTow { get; set; }

        /// <summary>
        /// 是否显示冻结金额20.4余额
        /// </summary>
        public bool IsReward { get; set; }
    }
}
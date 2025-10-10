using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardOrderVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardOrderVO));

        [DataMember]
        public Int32 CardOrderID { get { return (Int32)GetValue(typeof(Int32), "CardOrderID"); } set { SetValue("CardOrderID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String OrderNO { get { return (String)GetValue(typeof(String), "OrderNO"); } set { SetValue("OrderNO", value); } }
        [DataMember]
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public Int32 isUsed { get { return (Int32)GetValue(typeof(Int32), "isUsed"); } set { SetValue("isUsed", value); } }

        [DataMember]
        public Decimal AgentCost { get { return (Decimal)GetValue(typeof(Decimal), "AgentCost"); } set { SetValue("AgentCost", value); } }
        [DataMember]
        public Int32 AgentCustomerId { get { return (Int32)GetValue(typeof(Int32), "AgentCustomerId"); } set { SetValue("AgentCustomerId", value); } }

        [DataMember]
        public Int32 OneRebateCustomerId { get { return (Int32)GetValue(typeof(Int32), "OneRebateCustomerId"); } set { SetValue("OneRebateCustomerId", value); } }
        [DataMember]
        public Decimal OneRebateCost { get { return (Decimal)GetValue(typeof(Decimal), "OneRebateCost"); } set { SetValue("OneRebateCost", value); } }
        [DataMember]
        public Int32 OneRebateStatus { get { return (Int32)GetValue(typeof(Int32), "OneRebateStatus"); } set { SetValue("OneRebateStatus", value); } }
        [DataMember]
        public Int32 OneRebateAgentCustomerId { get { return (Int32)GetValue(typeof(Int32), "OneRebateAgentCustomerId"); } set { SetValue("OneRebateAgentCustomerId", value); } }

        [DataMember]
        public Int32 TwoRebateCustomerId { get { return (Int32)GetValue(typeof(Int32), "TwoRebateCustomerId"); } set { SetValue("TwoRebateCustomerId", value); } }
        [DataMember]
        public Decimal TwoRebateCost { get { return (Decimal)GetValue(typeof(Decimal), "TwoRebateCost"); } set { SetValue("TwoRebateCost", value); } }
        [DataMember]
        public Int32 TwoRebateStatus { get { return (Int32)GetValue(typeof(Int32), "TwoRebateStatus"); } set { SetValue("TwoRebateStatus", value); } }
        [DataMember]
        public Int32 TwoRebateAgentCustomerId { get { return (Int32)GetValue(typeof(Int32), "TwoRebateAgentCustomerId"); } set { SetValue("TwoRebateAgentCustomerId", value); } }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public Int32 InviterCID { get { return (Int32)GetValue(typeof(Int32), "InviterCID"); } set { SetValue("InviterCID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardOrderVO tmp = new CardOrderVO();
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
    public partial class CardOrderListVO
    {
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public String Headimg { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Decimal Cost { get; set; }
        [DataMember]
        public String Title { get; set; }
        [DataMember]
        public String Type { get; set; }
        [DataMember]
        public Int32 Status { get; set; }
        [DataMember]
        public String CostName { get ;set;}
    }
}

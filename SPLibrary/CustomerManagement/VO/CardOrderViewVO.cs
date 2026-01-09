using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardOrderViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardOrderViewVO));

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
        public Int32 TwoRebateCustomerId { get { return (Int32)GetValue(typeof(Int32), "TwoRebateCustomerId"); } set { SetValue("TwoRebateCustomerId", value); } }
        [DataMember]
        public Decimal TwoRebateCost { get { return (Decimal)GetValue(typeof(Decimal), "TwoRebateCost"); } set { SetValue("TwoRebateCost", value); } }
        [DataMember]
        public Int32 TwoRebateStatus { get { return (Int32)GetValue(typeof(Int32), "TwoRebateStatus"); } set { SetValue("TwoRebateStatus", value); } }

        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Boolean isVip { get { return (Boolean)GetValue(typeof(Boolean), "isVip"); } set { SetValue("isVip", value); } }
        [DataMember]
        public Int32 VipLevel { get { return (Int32)GetValue(typeof(Int32), "VipLevel"); } set { SetValue("VipLevel", value); } }
        [DataMember]
        public DateTime ExpirationAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpirationAt"); } set { SetValue("ExpirationAt", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public String CorporateName { get; set; }

        [DataMember]
        public String OneRebateHeaderLogo { get { return (String)GetValue(typeof(String), "OneRebateHeaderLogo"); } set { SetValue("OneRebateHeaderLogo", value); } }
        [DataMember]
        public String OneRebateCustomerName { get { return (String)GetValue(typeof(String), "OneRebateCustomerName"); } set { SetValue("OneRebateCustomerName", value); } }
        [DataMember]
        public String TwoRebateHeaderLogo { get { return (String)GetValue(typeof(String), "TwoRebateHeaderLogo"); } set { SetValue("TwoRebateHeaderLogo", value); } }
        [DataMember]
        public String TwoRebateCustomerName { get { return (String)GetValue(typeof(String), "TwoRebateCustomerName"); } set { SetValue("TwoRebateCustomerName", value); } }

        [DataMember]
        public String AgentHeaderLogo { get { return (String)GetValue(typeof(String), "AgentHeaderLogo"); } set { SetValue("AgentHeaderLogo", value); } }
        [DataMember]
        public String AgentCustomerName { get { return (String)GetValue(typeof(String), "AgentCustomerName"); } set { SetValue("AgentCustomerName", value); } }

        [DataMember]
        public Decimal RebateCost { get; set; }
        [DataMember]
        public String RebateName { get; set; }

        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            CardOrderViewVO tmp = new CardOrderViewVO();
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
}
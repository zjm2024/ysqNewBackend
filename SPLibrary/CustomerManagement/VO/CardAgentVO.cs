using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardAgentVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardAgentVO));
       
		[DataMember]
		 public Int32 CardAgentID { get { return (Int32)GetValue(typeof(Int32), "CardAgentID") ; } set {  SetValue("CardAgentID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public String AgentName { get { return (String)GetValue(typeof(String), "AgentName"); } set { SetValue("AgentName", value); } }
        [DataMember]
        public Int32 CityId { get { return (Int32)GetValue(typeof(Int32), "CityId"); } set { SetValue("CityId", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Province { get { return (String)GetValue(typeof(String), "Province"); } set { SetValue("Province", value); } }
        [DataMember]
        public String City { get { return (String)GetValue(typeof(String), "City"); } set { SetValue("City", value); } }

        [DataMember]
        public Decimal DepositCost { get { return (Decimal)GetValue(typeof(Decimal), "DepositCost"); } set { SetValue("DepositCost", value); } }
        [DataMember]
        public Decimal TotalCommission { get { return (Decimal)GetValue(typeof(Decimal), "TotalCommission"); } set { SetValue("TotalCommission", value); } }
        [DataMember]
        public Decimal PayableCommission { get { return (Decimal)GetValue(typeof(Decimal), "PayableCommission"); } set { SetValue("PayableCommission", value); } }
        [DataMember]
        public Decimal PaidCommission { get { return (Decimal)GetValue(typeof(Decimal), "PaidCommission"); } set { SetValue("PaidCommission", value); } }
        [DataMember]
        public Decimal AgentCost { get { return (Decimal)GetValue(typeof(Decimal), "AgentCost"); } set { SetValue("AgentCost", value); } }
        [DataMember]
        public Decimal TotalCost { get { return (Decimal)GetValue(typeof(Decimal), "TotalCost"); } set { SetValue("TotalCost", value); } }
        [DataMember]
        public Decimal SettlementCost { get { return (Decimal)GetValue(typeof(Decimal), "SettlementCost"); } set { SetValue("SettlementCost", value); } }

        [DataMember]
        public Int32 UserCount { get { return (Int32)GetValue(typeof(Int32), "UserCount"); } set { SetValue("UserCount", value); } }
        [DataMember]
        public Int32 VipCount { get { return (Int32)GetValue(typeof(Int32), "VipCount"); } set { SetValue("VipCount", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardAgentVO tmp = new CardAgentVO();
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
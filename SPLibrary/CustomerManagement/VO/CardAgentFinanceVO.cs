using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardAgentFinanceVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardAgentFinanceVO));
       
		[DataMember]
		 public Int32 FinanceID { get { return (Int32)GetValue(typeof(Int32), "FinanceID") ; } set {  SetValue("FinanceID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public String MONTH { get { return (String)GetValue(typeof(String), "MONTH"); } set { SetValue("MONTH", value); } }

        [DataMember]
        public Decimal TotalCommission { get { return (Decimal)GetValue(typeof(Decimal), "TotalCommission"); } set { SetValue("TotalCommission", value); } }
        [DataMember]
        public Decimal PayableCommission { get { return (Decimal)GetValue(typeof(Decimal), "PayableCommission"); } set { SetValue("PayableCommission", value); } }
        [DataMember]
        public Decimal PaidCommission { get { return (Decimal)GetValue(typeof(Decimal), "PaidCommission"); } set { SetValue("PaidCommission", value); } }
        [DataMember]
        public Decimal AgentCost { get { return (Decimal)GetValue(typeof(Decimal), "AgentCost"); } set { SetValue("AgentCost", value); } }

        [DataMember]
        public Boolean isSettlement { get { return (Boolean)GetValue(typeof(Boolean), "isSettlement"); } set { SetValue("isSettlement", value); } }
        [DataMember]
        public Decimal SettlementCost { get { return (Decimal)GetValue(typeof(Decimal), "SettlementCost"); } set { SetValue("SettlementCost", value); } }

        [DataMember]
        public DateTime SettlementAt { get { return (DateTime)GetValue(typeof(DateTime), "SettlementAt"); } set { SetValue("SettlementAt", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardAgentFinanceVO tmp = new CardAgentFinanceVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class InfoCostVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(InfoCostVO));

        [DataMember]
        public Int32 CostID { get { return (Int32)GetValue(typeof(Int32), "CostID"); } set { SetValue("CostID", value); } }
        [DataMember]
		public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID") ; } set {  SetValue("InfoID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Decimal DiscountCost { get { return (Decimal)GetValue(typeof(Decimal), "DiscountCost"); } set { SetValue("DiscountCost", value); } }
        [DataMember]
        public Int32 isAgent { get { return (Int32)GetValue(typeof(Int32), "isAgent"); } set { SetValue("isAgent", value); } }
        [DataMember]
        public Decimal AgentCost { get { return (Decimal)GetValue(typeof(Decimal), "AgentCost"); } set { SetValue("AgentCost", value); } }
        [DataMember]
        public Int32 isVipCost { get { return (Int32)GetValue(typeof(Int32), "isVipCost"); } set { SetValue("isVipCost", value); } }
        [DataMember]
        public Decimal VipCost { get { return (Decimal)GetValue(typeof(Decimal), "VipCost"); } set { SetValue("VipCost", value); } }

        [DataMember]
        public Decimal SeckillDiscountCost { get { return (Decimal)GetValue(typeof(Decimal), "SeckillDiscountCost"); } set { SetValue("SeckillDiscountCost", value); } }
        [DataMember]
        public String CostName { get { return (String)GetValue(typeof(String), "CostName"); } set { SetValue("CostName", value); } }
        [DataMember]
        public String Attribute { get { return (String)GetValue(typeof(String), "Attribute"); } set { SetValue("Attribute", value); } }
        [DataMember]
        public Int32 PerPersonLimit { get { return (Int32)GetValue(typeof(Int32), "PerPersonLimit"); } set { SetValue("PerPersonLimit", value); } }
        [DataMember]
        public Decimal GiveIntegral { get { return (Decimal)GetValue(typeof(Decimal), "GiveIntegral"); } set { SetValue("GiveIntegral", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            InfoCostVO tmp = new InfoCostVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class OrderGroupBuyNotGroupViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(OrderGroupBuyNotGroupViewVO));

        [DataMember]
        public Int32 OrderID { get { return (Int32)GetValue(typeof(Int32), "OrderID"); } set { SetValue("OrderID", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } }
        [DataMember]
        public Int32 GroupBuyID { get { return (Int32)GetValue(typeof(Int32), "GroupBuyID"); } set { SetValue("GroupBuyID", value); } }
        [DataMember]
        public Int32 isGroupBuy { get { return (Int32)GetValue(typeof(Int32), "isGroupBuy"); } set { SetValue("isGroupBuy", value); } }
        [DataMember]
        public Int32 isEcommerceBuy { get { return (Int32)GetValue(typeof(Int32), "isEcommerceBuy"); } set { SetValue("isEcommerceBuy", value); } }
        [DataMember]
        public Int32 isAgentBuy { get { return (Int32)GetValue(typeof(Int32), "isAgentBuy"); } set { SetValue("isAgentBuy", value); } }
        [DataMember]
        public DateTime ExpireAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpireAt"); } set { SetValue("ExpireAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public String OrderNO { get { return (String)GetValue(typeof(String), "OrderNO"); } set { SetValue("OrderNO", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            OrderGroupBuyNotGroupViewVO tmp = new OrderGroupBuyNotGroupViewVO();
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
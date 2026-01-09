using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class OrderByGroupbuyIdViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(OrderByGroupbuyIdViewVO));

        [DataMember]
        public Int32 OrderID { get { return (Int32)GetValue(typeof(Int32), "OrderID"); } set { SetValue("OrderID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } }
        [DataMember]
        public Int32 GroupBuyID { get { return (Int32)GetValue(typeof(Int32), "GroupBuyID"); } set { SetValue("GroupBuyID", value); } }
        [DataMember]
        public Int32 COUNT { get { return (Int32)GetValue(typeof(Int32), "COUNT"); } set { SetValue("COUNT", value); } }
        [DataMember]
        public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID"); } set { SetValue("InfoID", value); } }
        [DataMember]
        public Int32 isGroupBuy { get { return (Int32)GetValue(typeof(Int32), "isGroupBuy"); } set { SetValue("isGroupBuy", value); } }
        [DataMember]
        public DateTime ExpireAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpireAt"); } set { SetValue("ExpireAt", value); } }
        [DataMember]
        public Int32 Discount { get { return (Int32)GetValue(typeof(Int32), "Discount"); } set { SetValue("Discount", value); } }
        [DataMember]
        public Int32 PeopleNumber { get { return (Int32)GetValue(typeof(Int32), "PeopleNumber"); } set { SetValue("PeopleNumber", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            OrderByGroupbuyIdViewVO tmp = new OrderByGroupbuyIdViewVO();
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
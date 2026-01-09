using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class GroupBuyMemberVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(GroupBuyMemberVO));
        
		[DataMember]
		public Int32 GroupBuyMemberID { get { return (Int32)GetValue(typeof(Int32), "GroupBuyMemberID") ; } set {  SetValue("GroupBuyMemberID", value); } }
        [DataMember]
		public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 GroupBuyID { get { return (Int32)GetValue(typeof(Int32), "GroupBuyID"); } set { SetValue("GroupBuyID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Order_info { get { return (Int32)GetValue(typeof(Int32), "Order_info"); } set { SetValue("Order_info", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID"); } set { SetValue("InfoID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            GroupBuyMemberVO tmp = new GroupBuyMemberVO();
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
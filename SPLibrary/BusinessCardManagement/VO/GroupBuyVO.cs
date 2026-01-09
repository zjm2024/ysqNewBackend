using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class GroupBuyVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(GroupBuyVO));
        
		[DataMember]
		public Int32 GroupBuyID { get { return (Int32)GetValue(typeof(Int32), "GroupBuyID") ; } set {  SetValue("GroupBuyID", value); } }
        [DataMember]
		public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime ExpireAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpireAt"); } set { SetValue("ExpireAt", value); } }
        [DataMember]
        public Int32 Discount { get { return (Int32)GetValue(typeof(Int32), "Discount"); } set { SetValue("Discount", value); } }
        [DataMember]
        public Int32 AgentPersonalID { get { return (Int32)GetValue(typeof(Int32), "AgentPersonalID"); } set { SetValue("AgentPersonalID", value); } }
        [DataMember]
        public Int32 PeopleNumber { get { return (Int32)GetValue(typeof(Int32), "PeopleNumber"); } set { SetValue("PeopleNumber", value); } }
        [DataMember]
        public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID"); } set { SetValue("InfoID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            GroupBuyVO tmp = new GroupBuyVO();
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
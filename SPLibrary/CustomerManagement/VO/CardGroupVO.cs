using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardGroupVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardGroupVO));

        [DataMember]
        public Int32 GroupID { get { return (Int32)GetValue(typeof(Int32), "GroupID"); } set { SetValue("GroupID", value); } }
        [DataMember]
		public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public String GroupName { get { return (String)GetValue(typeof(String), "GroupName"); } set { SetValue("GroupName", value); } }
        [DataMember]
        public Int32 JoinSetUp { get { return (Int32)GetValue(typeof(Int32), "JoinSetUp"); } set { SetValue("JoinSetUp", value); } }
        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }
        [DataMember]
        public String CardImg { get { return (String)GetValue(typeof(String), "CardImg"); } set { SetValue("CardImg", value); } }
        [DataMember]
        public String QRImg { get { return (String)GetValue(typeof(String), "QRImg"); } set { SetValue("QRImg", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardGroupVO tmp = new CardGroupVO();
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

    public partial class GroupVO
    {
        [DataMember]
        public CardGroupVO CardGroupVO { get; set; }
        [DataMember]
        public Int32 NumberOfPeople { get; set; }
    }

    public partial class MessageCount
    {
        [DataMember]
        public Int32 SendCount { get; set; }
        [DataMember]
        public Int32 GroupCount { get; set; }
        [DataMember]
        public Int32 LeliaoCount { get; set; }
    }
}
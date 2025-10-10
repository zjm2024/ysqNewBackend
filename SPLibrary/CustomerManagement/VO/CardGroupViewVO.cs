using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardGroupViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardGroupViewVO));

        [DataMember]
        public Int32 GroupCardID { get { return (Int32)GetValue(typeof(Int32), "GroupCardID"); } set { SetValue("GroupCardID", value); } }
        [DataMember]
		public Int32 GroupID { get { return (Int32)GetValue(typeof(Int32), "GroupID") ; } set {  SetValue("GroupID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public String GroupName { get { return (String)GetValue(typeof(String), "GroupName"); } set { SetValue("GroupName", value); } }
        [DataMember]
        public Int32 JoinSetUp { get { return (Int32)GetValue(typeof(Int32), "JoinSetUp"); } set { SetValue("JoinSetUp", value); } }
        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }
        [DataMember]
        public String CardImg { get { return (String)GetValue(typeof(String), "CardImg"); } set { SetValue("CardImg", value); } }
        [DataMember]
        public Int32 NumberOfPeople { get { return (Int32)GetValue(typeof(Int32), "NumberOfPeople"); } set { SetValue("NumberOfPeople", value); } }
        [DataMember]
        public Int32 MessageCount { get { return (Int32)GetValue(typeof(Int32), "MessageCount"); } set { SetValue("MessageCount", value); } }
        [DataMember]
        public List<CardGroupCardViewVO> CardGroupCardViewList { get { return (List<CardGroupCardViewVO>)GetValue(typeof(List<CardGroupCardViewVO>), "CardGroupCardViewList"); } set { SetValue("CardGroupCardViewList", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardGroupViewVO tmp = new CardGroupViewVO();
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
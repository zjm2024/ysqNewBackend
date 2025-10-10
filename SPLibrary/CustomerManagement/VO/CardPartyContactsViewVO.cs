using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardPartyContactsViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardPartyContactsViewVO));

        [DataMember]
        public Int32 PartyContactsID { get { return (Int32)GetValue(typeof(Int32), "PartyContactsID"); } set { SetValue("PartyContactsID", value); } }
        [DataMember]
		public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID") ; } set {  SetValue("PartyID", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardPartyContactsViewVO tmp = new CardPartyContactsViewVO();
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
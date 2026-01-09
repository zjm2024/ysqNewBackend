using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardPartySignUpFormVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardPartySignUpFormVO));

        [DataMember]
        public Int32 SignUpFormID { get { return (Int32)GetValue(typeof(Int32), "SignUpFormID"); } set { SetValue("SignUpFormID", value); } }
        [DataMember]
		public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID") ; } set {  SetValue("PartyID", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public Int32 must { get { return (Int32)GetValue(typeof(Int32), "must"); } set { SetValue("must", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String AutioText { get { return (String)GetValue(typeof(String), "AutioText"); } set { SetValue("AutioText", value); } }
        
        [DataMember]
        public String value { get; set; }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardPartySignUpFormVO tmp = new CardPartySignUpFormVO();
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
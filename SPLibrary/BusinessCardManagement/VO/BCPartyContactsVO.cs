using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BCPartyContactsVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BCPartyContactsVO));

        [DataMember]
        public Int32 PartyContactsID { get { return (Int32)GetValue(typeof(Int32), "PartyContactsID"); } set { SetValue("PartyContactsID", value); } }
        [DataMember]
		public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID") ; } set {  SetValue("PartyID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            BCPartyContactsVO tmp = new BCPartyContactsVO();
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
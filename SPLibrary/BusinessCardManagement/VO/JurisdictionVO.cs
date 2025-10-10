using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class JurisdictionVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(JurisdictionVO));
       
		[DataMember]
		 public Int32 JurisdictionID { get { return (Int32)GetValue(typeof(Int32), "JurisdictionID") ; } set {  SetValue("JurisdictionID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            JurisdictionVO tmp = new JurisdictionVO();
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
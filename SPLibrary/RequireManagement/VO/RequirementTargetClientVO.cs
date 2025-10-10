using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RequirementTargetClientVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RequirementTargetClientVO));

        [DataMember]
        public Int32 RequireClientId { get { return (Int32)GetValue(typeof(Int32), "RequireClientId"); } set { SetValue("RequireClientId", value); } }
        [DataMember]
        public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32), "RequirementId"); } set { SetValue("RequirementId", value); } }
        [DataMember]
        public String ClientName { get { return (String)GetValue(typeof(String), "ClientName"); } set { SetValue("ClientName", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            RequirementTargetClientVO tmp = new RequirementTargetClientVO();
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
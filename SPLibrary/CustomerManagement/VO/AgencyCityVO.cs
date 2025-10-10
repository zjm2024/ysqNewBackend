using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencyCityVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyCityVO));
       
		[DataMember]
		 public Int32 AgencyCityId { get { return (Int32)GetValue(typeof(Int32),"AgencyCityId") ; } set {  SetValue("AgencyCityId",value); } } 		[DataMember]
		 public Int32 CityId { get { return (Int32)GetValue(typeof(Int32),"CityId") ; } set {  SetValue("CityId",value); } } 		[DataMember]
		 public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32),"AgencyId") ; } set {  SetValue("AgencyId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencyCityVO tmp = new AgencyCityVO();
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
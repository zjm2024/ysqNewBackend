using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RequirementTargetCityVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RequirementTargetCityVO));
       
		[DataMember]
		 public Int32 RequirementTargetCityId { get { return (Int32)GetValue(typeof(Int32),"RequirementTargetCityId") ; } set {  SetValue("RequirementTargetCityId",value); } } 		[DataMember]
		 public Int32 CityId { get { return (Int32)GetValue(typeof(Int32),"CityId") ; } set {  SetValue("CityId",value); } } 		[DataMember]
		 public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32),"RequirementId") ; } set {  SetValue("RequirementId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            RequirementTargetCityVO tmp = new RequirementTargetCityVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class TargetCityVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TargetCityVO));
       
		[DataMember]
		 public Int32 TargetCityId { get { return (Int32)GetValue(typeof(Int32),"TargetCityId") ; } set {  SetValue("TargetCityId",value); } } 		[DataMember]
		 public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32),"BusinessId") ; } set {  SetValue("BusinessId",value); } } 		[DataMember]
		 public Int32 CityId { get { return (Int32)GetValue(typeof(Int32),"CityId") ; } set {  SetValue("CityId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            TargetCityVO tmp = new TargetCityVO();
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
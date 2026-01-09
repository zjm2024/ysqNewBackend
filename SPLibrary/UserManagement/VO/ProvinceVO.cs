using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ProvinceVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProvinceVO));
       
		[DataMember]
		 public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32),"ProvinceId") ; } set {  SetValue("ProvinceId",value); } } 		[DataMember]
		 public String ProvinceCode { get { return (String)GetValue(typeof(String),"ProvinceCode") ; } set {  SetValue("ProvinceCode",value); } } 		[DataMember]
		 public String ProvinceName { get { return (String)GetValue(typeof(String),"ProvinceName") ; } set {  SetValue("ProvinceName",value); } } 		[DataMember]
		public Boolean Status { get { return (Boolean)GetValue(typeof(Boolean),"Status") ; } set {  SetValue("Status",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            ProvinceVO tmp = new ProvinceVO();
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
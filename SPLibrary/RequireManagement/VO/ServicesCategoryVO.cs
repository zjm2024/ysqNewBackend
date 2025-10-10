using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ServicesCategoryVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ServicesCategoryVO));
       
		[DataMember]
		 public Int32 ServicesCategoryId { get { return (Int32)GetValue(typeof(Int32),"ServicesCategoryId") ; } set {  SetValue("ServicesCategoryId",value); } } 		[DataMember]
		 public Int32 ServicesId { get { return (Int32)GetValue(typeof(Int32),"ServicesId") ; } set {  SetValue("ServicesId",value); } } 		[DataMember]
		 public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32),"CategoryId") ; } set {  SetValue("CategoryId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            ServicesCategoryVO tmp = new ServicesCategoryVO();
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
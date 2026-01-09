using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BusinessIdcardVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BusinessIdcardVO));
       
		[DataMember]
		 public Int32 BusinessIDCardId { get { return (Int32)GetValue(typeof(Int32),"BusinessIDCardId") ; } set {  SetValue("BusinessIDCardId",value); } } 		[DataMember]
		 public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32),"BusinessId") ; } set {  SetValue("BusinessId",value); } } 		[DataMember]
		 public String IDCardImg { get { return (String)GetValue(typeof(String),"IDCardImg") ; } set {  SetValue("IDCardImg",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            BusinessIdcardVO tmp = new BusinessIdcardVO();
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
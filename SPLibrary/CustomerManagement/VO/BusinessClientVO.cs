using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BusinessClientVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BusinessClientVO));
       
		[DataMember]
		 public Int32 BusinessClientId { get { return (Int32)GetValue(typeof(Int32),"BusinessClientId") ; } set {  SetValue("BusinessClientId",value); } } 		[DataMember]
		 public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32),"BusinessId") ; } set {  SetValue("BusinessId",value); } } 		[DataMember]
		 public String ClientName { get { return (String)GetValue(typeof(String),"ClientName") ; } set {  SetValue("ClientName",value); } } 		[DataMember]
		 public String CompanyName { get { return (String)GetValue(typeof(String),"CompanyName") ; } set {  SetValue("CompanyName",value); } } 		[DataMember]
		 public String Description { get { return (String)GetValue(typeof(String),"Description") ; } set {  SetValue("Description",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            BusinessClientVO tmp = new BusinessClientVO();
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
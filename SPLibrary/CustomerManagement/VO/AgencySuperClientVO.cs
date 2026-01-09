using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencySuperClientVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencySuperClientVO));
       
		[DataMember]
		 public Int32 AgencySuperClientId { get { return (Int32)GetValue(typeof(Int32),"AgencySuperClientId") ; } set {  SetValue("AgencySuperClientId",value); } } 		[DataMember]
		 public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32),"AgencyId") ; } set {  SetValue("AgencyId",value); } } 		[DataMember]
		 public String SuperClientName { get { return (String)GetValue(typeof(String),"SuperClientName") ; } set {  SetValue("SuperClientName",value); } } 		[DataMember]
		 public String CompanyName { get { return (String)GetValue(typeof(String),"CompanyName") ; } set {  SetValue("CompanyName",value); } } 		[DataMember]
		 public String Description { get { return (String)GetValue(typeof(String),"Description") ; } set {  SetValue("Description",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencySuperClientVO tmp = new AgencySuperClientVO();
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
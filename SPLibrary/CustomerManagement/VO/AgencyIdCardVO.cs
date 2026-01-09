using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencyIdCardVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyIdCardVO));
       
		[DataMember]
		 public Int32 AgencyIDCardId { get { return (Int32)GetValue(typeof(Int32),"AgencyIDCardId") ; } set {  SetValue("AgencyIDCardId",value); } } 		[DataMember]
		 public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32),"AgencyId") ; } set {  SetValue("AgencyId",value); } } 		[DataMember]
		 public String IDCardImg { get { return (String)GetValue(typeof(String),"IDCardImg") ; } set {  SetValue("IDCardImg",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencyIdCardVO tmp = new AgencyIdCardVO();
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
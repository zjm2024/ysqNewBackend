using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencyTechnicalVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyTechnicalVO));
       
		[DataMember]
		 public Int32 AgencyTechnicalId { get { return (Int32)GetValue(typeof(Int32),"AgencyTechnicalId") ; } set {  SetValue("AgencyTechnicalId",value); } } 		[DataMember]
		 public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32),"AgencyId") ; } set {  SetValue("AgencyId",value); } } 		[DataMember]
		 public String TechnicalImg { get { return (String)GetValue(typeof(String),"TechnicalImg") ; } set {  SetValue("TechnicalImg",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencyTechnicalVO tmp = new AgencyTechnicalVO();
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
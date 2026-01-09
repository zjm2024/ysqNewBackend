using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencyCategoryVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyCategoryVO));
       
		[DataMember]
		 public Int32 AgencyCategoryId { get { return (Int32)GetValue(typeof(Int32),"AgencyCategoryId") ; } set {  SetValue("AgencyCategoryId",value); } } 		[DataMember]
		 public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32),"AgencyId") ; } set {  SetValue("AgencyId",value); } } 		[DataMember]
		 public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32),"CategoryId") ; } set {  SetValue("CategoryId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencyCategoryVO tmp = new AgencyCategoryVO();
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
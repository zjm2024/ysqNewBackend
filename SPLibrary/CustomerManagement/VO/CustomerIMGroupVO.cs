using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CustomerIMGroupVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerIMGroupVO));
       
		[DataMember]
		 public Int32 CustomerIMGroupId { get { return (Int32)GetValue(typeof(Int32),"CustomerIMGroupId") ; } set {  SetValue("CustomerIMGroupId",value); } } 		[DataMember]
		 public Int32 CustomerIMId { get { return (Int32)GetValue(typeof(Int32),"CustomerIMId") ; } set {  SetValue("CustomerIMId",value); } } 		[DataMember]
		 public String IMGroupName { get { return (String)GetValue(typeof(String),"IMGroupName") ; } set {  SetValue("IMGroupName",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            CustomerIMGroupVO tmp = new CustomerIMGroupVO();
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
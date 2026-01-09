using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CustomerIMGroupUserVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerIMGroupUserVO));
       
		[DataMember]
		 public Int32 CustomerIMGroupUserId { get { return (Int32)GetValue(typeof(Int32),"CustomerIMGroupUserId") ; } set {  SetValue("CustomerIMGroupUserId",value); } } 		[DataMember]
		 public Int32 CustomerIMGroupId { get { return (Int32)GetValue(typeof(Int32),"CustomerIMGroupId") ; } set {  SetValue("CustomerIMGroupId",value); } } 		[DataMember]
		 public Int32 CustomerIMId { get { return (Int32)GetValue(typeof(Int32),"CustomerIMId") ; } set {  SetValue("CustomerIMId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            CustomerIMGroupUserVO tmp = new CustomerIMGroupUserVO();
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
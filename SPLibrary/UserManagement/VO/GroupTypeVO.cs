using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class GroupTypeVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(GroupTypeVO));
       
		[DataMember]
		public Int32 GroupTypeId { get { return (Int32)GetValue(typeof(Int32),"GroupTypeId") ; } set {  SetValue("GroupTypeId",value); } }
		[DataMember]
		public String GroupTypeName { get { return (String)GetValue(typeof(String),"GroupTypeName") ; } set {  SetValue("GroupTypeName",value); } }
		[DataMember]
		public Int32 SortNumber { get { return (Int32)GetValue(typeof(Int32),"SortNumber") ; } set {  SetValue("SortNumber",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            GroupTypeVO tmp = new GroupTypeVO();
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
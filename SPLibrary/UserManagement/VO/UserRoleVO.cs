using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class UserRoleVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(UserRoleVO));
       
		[DataMember]
		public Int32 UserRoleId { get { return (Int32)GetValue(typeof(Int32),"UserRoleId") ; } set {  SetValue("UserRoleId",value); } }
		[DataMember]
		public Int32 UserId { get { return (Int32)GetValue(typeof(Int32),"UserId") ; } set {  SetValue("UserId",value); } }
		[DataMember]
		public Int32 RoleId { get { return (Int32)GetValue(typeof(Int32),"RoleId") ; } set {  SetValue("RoleId",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            UserRoleVO tmp = new UserRoleVO();
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
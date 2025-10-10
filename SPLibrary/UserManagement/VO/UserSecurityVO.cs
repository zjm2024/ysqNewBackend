using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class UserSecurityVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(UserSecurityVO));
       
		[DataMember]
		public Int32 UserSecurityId { get { return (Int32)GetValue(typeof(Int32),"UserSecurityId") ; } set {  SetValue("UserSecurityId",value); } }
		[DataMember]
		public Int32 SecurityId { get { return (Int32)GetValue(typeof(Int32),"SecurityId") ; } set {  SetValue("SecurityId",value); } }
		[DataMember]
		public Int32 UserId { get { return (Int32)GetValue(typeof(Int32),"UserId") ; } set {  SetValue("UserId",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            UserSecurityVO tmp = new UserSecurityVO();
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
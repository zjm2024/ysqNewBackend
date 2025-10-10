using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RoleSecurityVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RoleSecurityVO));
       
		[DataMember]
		public Int32 RoleSecurityId { get { return (Int32)GetValue(typeof(Int32),"RoleSecurityId") ; } set {  SetValue("RoleSecurityId",value); } }
		[DataMember]
		public Int32 RoleId { get { return (Int32)GetValue(typeof(Int32),"RoleId") ; } set {  SetValue("RoleId",value); } }
		[DataMember]
		public Int32 SecurityId { get { return (Int32)GetValue(typeof(Int32),"SecurityId") ; } set {  SetValue("SecurityId",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            RoleSecurityVO tmp = new RoleSecurityVO();
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
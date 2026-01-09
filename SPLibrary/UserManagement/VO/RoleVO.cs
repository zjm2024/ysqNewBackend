using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RoleVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RoleVO));
       
		[DataMember]
		public Int32 RoleId { get { return (Int32)GetValue(typeof(Int32),"RoleId") ; } set {  SetValue("RoleId",value); } }
		[DataMember]
		public String RoleName { get { return (String)GetValue(typeof(String),"RoleName") ; } set {  SetValue("RoleName",value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
		[DataMember]
		public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } }
		[DataMember]
		public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime),"UpdatedAt") ; } set {  SetValue("UpdatedAt",value); } }
		[DataMember]
		public Int32 UpdatedBy { get { return (Int32)GetValue(typeof(Int32),"UpdatedBy") ; } set {  SetValue("UpdatedBy",value); } }
        [DataMember]
        public Int32 CompanyId { get { return (Int32)GetValue(typeof(Int32), "CompanyId"); } set { SetValue("CompanyId", value); } }
        [DataMember]
        public Boolean IsDefault { get { return (Boolean)GetValue(typeof(Boolean), "IsDefault"); } set { SetValue("IsDefault", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            RoleVO tmp = new RoleVO();
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
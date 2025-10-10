using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RoleViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RoleViewVO));

        [DataMember]
        public Int32 CompanyId { get { return (Int32)GetValue(typeof(Int32), "CompanyId"); } set { SetValue("CompanyId", value); } }
        [DataMember]
        public Boolean IsDefault { get { return (Boolean)GetValue(typeof(Boolean), "IsDefault"); } set { SetValue("IsDefault", value); } }
        [DataMember]
        public Int32 RoleId { get { return (Int32)GetValue(typeof(Int32), "RoleId"); } set { SetValue("RoleId", value); } }
        [DataMember]
        public String RoleName { get { return (String)GetValue(typeof(String), "RoleName"); } set { SetValue("RoleName", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String CreatedBy { get { return (String)GetValue(typeof(String), "CreatedBy"); } set { SetValue("CreatedBy", value); } }
        [DataMember]
        public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime), "UpdatedAt"); } set { SetValue("UpdatedAt", value); } }
        [DataMember]
        public String UpdatedBy { get { return (String)GetValue(typeof(String), "UpdatedBy"); } set { SetValue("UpdatedBy", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            RoleViewVO tmp = new RoleViewVO();
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
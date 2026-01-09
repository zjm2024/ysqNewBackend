using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class DepartmentViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(DepartmentViewVO));

        [DataMember]
        public Int32 CompanyId { get { return (Int32)GetValue(typeof(Int32), "CompanyId"); } set { SetValue("CompanyId", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public Int32 DepartmentId { get { return (Int32)GetValue(typeof(Int32), "DepartmentId"); } set { SetValue("DepartmentId", value); } }
        [DataMember]
        public String DepartmentName { get { return (String)GetValue(typeof(String), "DepartmentName"); } set { SetValue("DepartmentName", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String UserName { get { return (String)GetValue(typeof(String), "UserName"); } set { SetValue("UserName", value); } }
        [DataMember]
        public String DepartmentHeader { get { return (String)GetValue(typeof(String), "DepartmentHeader"); } set { SetValue("DepartmentHeader", value); } }
        [DataMember]
        public String DepartmentCode { get { return (String)GetValue(typeof(String), "DepartmentCode"); } set { SetValue("DepartmentCode", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String ContactPerson { get { return (String)GetValue(typeof(String), "ContactPerson"); } set { SetValue("ContactPerson", value); } }
        [DataMember]
        public String ContactPhone { get { return (String)GetValue(typeof(String), "ContactPhone"); } set { SetValue("ContactPhone", value); } }
        [DataMember]
        public String CreatedBy { get { return (String)GetValue(typeof(String), "CreatedBy"); } set { SetValue("CreatedBy", value); } }
        [DataMember]
        public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime), "UpdatedAt"); } set { SetValue("UpdatedAt", value); } }
        [DataMember]
        public String UpdatedBy { get { return (String)GetValue(typeof(String), "UpdatedBy"); } set { SetValue("UpdatedBy", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            DepartmentViewVO tmp = new DepartmentViewVO();
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
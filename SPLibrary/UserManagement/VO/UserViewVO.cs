using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class UserViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(UserViewVO));

        [DataMember]
        public Int32 CompanyId { get { return (Int32)GetValue(typeof(Int32), "CompanyId"); } set { SetValue("CompanyId", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public String DepartmentName { get { return (String)GetValue(typeof(String), "DepartmentName"); } set { SetValue("DepartmentName", value); } }
        [DataMember]
        public Int32 UserId { get { return (Int32)GetValue(typeof(Int32), "UserId"); } set { SetValue("UserId", value); } }        
        [DataMember]
        public Int32 DepartmentId { get { return (Int32)GetValue(typeof(Int32), "DepartmentId"); } set { SetValue("DepartmentId", value); } }
        [DataMember]
        public String UserCode { get { return (String)GetValue(typeof(String), "UserCode"); } set { SetValue("UserCode", value); } }
        [DataMember]
        public String UserName { get { return (String)GetValue(typeof(String), "UserName"); } set { SetValue("UserName", value); } }
        [DataMember]
        public String LoginName { get { return (String)GetValue(typeof(String), "LoginName"); } set { SetValue("LoginName", value); } }
        [DataMember]
        public String Email { get { return (String)GetValue(typeof(String), "Email"); } set { SetValue("Email", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Sex { get { return (String)GetValue(typeof(String), "Sex"); } set { SetValue("Sex", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String CreatedBy { get { return (String)GetValue(typeof(String), "CreatedBy"); } set { SetValue("CreatedBy", value); } }
        [DataMember]
        public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime), "UpdatedAt"); } set { SetValue("UpdatedAt", value); } }
        [DataMember]
        public String UpdatedBy { get { return (String)GetValue(typeof(String), "UpdatedBy"); } set { SetValue("UpdatedBy", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            UserViewVO tmp = new UserViewVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class UserVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(UserVO));
       
		[DataMember]
		public Int32 UserId { get { return (Int32)GetValue(typeof(Int32),"UserId") ; } set {  SetValue("UserId",value); } }
		[DataMember]
		public Int32 DepartmentId { get { return (Int32)GetValue(typeof(Int32), "DepartmentId") ; } set {  SetValue("DepartmentId", value); } }
		[DataMember]
		public Int32 UserTitleId { get { return (Int32)GetValue(typeof(Int32),"UserTitleId") ; } set {  SetValue("UserTitleId",value); } }
		[DataMember]
		public String UserCode { get { return (String)GetValue(typeof(String),"UserCode") ; } set {  SetValue("UserCode",value); } }
		[DataMember]
		public String UserName { get { return (String)GetValue(typeof(String),"UserName") ; } set {  SetValue("UserName",value); } }
		[DataMember]
		public String LoginName { get { return (String)GetValue(typeof(String),"LoginName") ; } set {  SetValue("LoginName",value); } }
		[DataMember]
		public String Password { get { return (String)GetValue(typeof(String),"Password") ; } set {  SetValue("Password",value); } }
		[DataMember]
		public String Email { get { return (String)GetValue(typeof(String),"Email") ; } set {  SetValue("Email",value); } }
		[DataMember]
		public String Phone { get { return (String)GetValue(typeof(String),"Phone") ; } set {  SetValue("Phone",value); } }
		[DataMember]
		public Int32 Status { get { return (Int32)GetValue(typeof(Int32),"Status") ; } set {  SetValue("Status",value); } }
		[DataMember]
		public Boolean Sex { get { return (Boolean)GetValue(typeof(Boolean),"Sex") ; } set {  SetValue("Sex",value); } }
		[DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
		[DataMember]
		public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } }
		[DataMember]
		public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime),"UpdatedAt") ; } set {  SetValue("UpdatedAt",value); } }
		[DataMember]
		public Int32 UpdatedBy { get { return (Int32)GetValue(typeof(Int32),"UpdatedBy") ; } set {  SetValue("UpdatedBy",value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            UserVO tmp = new UserVO();
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
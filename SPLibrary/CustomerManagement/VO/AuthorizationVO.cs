using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AuthorizationVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AuthorizationVO));

        [DataMember]
        public Int32 AuthorizationID { get { return (Int32)GetValue(typeof(Int32), "AuthorizationID"); } set { SetValue("AuthorizationID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String AppId { get { return (String)GetValue(typeof(String), "AppId"); } set { SetValue("AppId", value); } }
        [DataMember]
        public String authorizer_appid { get { return (String)GetValue(typeof(String), "authorizer_appid"); } set { SetValue("authorizer_appid", value); } }
        [DataMember]
        public String authorizer_access_token { get { return (String)GetValue(typeof(String), "authorizer_access_token"); } set { SetValue("authorizer_access_token", value); } }
        [DataMember]
        public String expires_in { get { return (String)GetValue(typeof(String), "expires_in"); } set { SetValue("expires_in", value); } }
        [DataMember]
        public String authorizer_refresh_token { get { return (String)GetValue(typeof(String), "authorizer_refresh_token"); } set { SetValue("authorizer_refresh_token", value); } }
        [DataMember]
        public String nick_name { get { return (String)GetValue(typeof(String), "nick_name"); } set { SetValue("nick_name", value); } }
        [DataMember]
        public String head_img { get { return (String)GetValue(typeof(String), "head_img"); } set { SetValue("head_img", value); } }
        [DataMember]
        public Int32 service_type_info { get { return (Int32)GetValue(typeof(Int32), "service_type_info"); } set { SetValue("service_type_info", value); } }
        [DataMember]
        public Int32 verify_type_info { get { return (Int32)GetValue(typeof(Int32), "verify_type_info"); } set { SetValue("verify_type_info", value); } }
        [DataMember]
        public String user_name { get { return (String)GetValue(typeof(String), "user_name"); } set { SetValue("user_name", value); } }
        [DataMember]
        public String principal_name { get { return (String)GetValue(typeof(String), "principal_name"); } set { SetValue("principal_name", value); } }
        [DataMember]
        public String alias { get { return (String)GetValue(typeof(String), "alias"); } set { SetValue("alias", value); } }
        [DataMember]
        public Int32 open_pay { get { return (Int32)GetValue(typeof(Int32), "open_pay"); } set { SetValue("open_pay", value); } }
        [DataMember]
        public String qrcode_url { get { return (String)GetValue(typeof(String), "qrcode_url"); } set { SetValue("qrcode_url", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime ExpiresAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpiresAt"); } set { SetValue("ExpiresAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AuthorizationVO tmp = new AuthorizationVO();
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
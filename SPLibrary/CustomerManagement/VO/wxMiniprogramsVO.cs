using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class wxMiniprogramsVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(wxMiniprogramsVO));

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public String AppName { get { return (String)GetValue(typeof(String), "AppName"); } set { SetValue("AppName", value); } }
        [DataMember]
        public String AppId { get { return (String)GetValue(typeof(String), "AppId"); } set { SetValue("AppId", value); } }
        [DataMember]
        public String Secret { get { return (String)GetValue(typeof(String), "Secret"); } set { SetValue("Secret", value); } }
        [DataMember]
        public String MCHID { get { return (String)GetValue(typeof(String), "MCHID"); } set { SetValue("MCHID", value); } }
        [DataMember]
        public String MCH_KEY { get { return (String)GetValue(typeof(String), "MCH_KEY"); } set { SetValue("MCH_KEY", value); } }
        [DataMember]
        public String APPSECRET { get { return (String)GetValue(typeof(String), "APPSECRET"); } set { SetValue("APPSECRET", value); } }
        [DataMember]
        public String SSLCERT_PATH { get { return (String)GetValue(typeof(String), "SSLCERT_PATH"); } set { SetValue("SSLCERT_PATH", value); } }
        [DataMember]
        public String SSLCERT_PASSWORD { get { return (String)GetValue(typeof(String), "SSLCERT_PASSWORD"); } set { SetValue("SSLCERT_PASSWORD", value); } }

        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 TBusinessID { get { return (Int32)GetValue(typeof(Int32), "TBusinessID"); } set { SetValue("TBusinessID", value); } }
        [DataMember]
        public Int32 TPersonalID { get { return (Int32)GetValue(typeof(Int32), "TPersonalID"); } set { SetValue("TPersonalID", value); } }
        [DataMember]
        public Int32 templateID { get { return (Int32)GetValue(typeof(Int32), "templateID"); } set { SetValue("templateID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            wxMiniprogramsVO tmp = new wxMiniprogramsVO();
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
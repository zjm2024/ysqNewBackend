using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ZXTMessageViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ZXTMessageViewVO));

        [DataMember]
        public Int32 MessageID { get { return (Int32)GetValue(typeof(Int32), "MessageID"); } set { SetValue("MessageID", value); } }
        [DataMember]
        public Int32 MessageFrom { get { return (Int32)GetValue(typeof(Int32), "MessageFrom"); } set { SetValue("MessageFrom", value); } }
        [DataMember]
        public Int32 MessageTo { get { return (Int32)GetValue(typeof(Int32), "MessageTo"); } set { SetValue("MessageTo", value); } }
        [DataMember]
        public String MessageType { get { return (String)GetValue(typeof(String), "MessageType"); } set { SetValue("MessageType", value); } }
        [DataMember]
        public String Message { get { return (String)GetValue(typeof(String), "Message"); } set { SetValue("Message", value); } }
        [DataMember]
        public String FileURL { get { return (String)GetValue(typeof(String), "FileURL"); } set { SetValue("FileURL", value); } }
        [DataMember]
        public String FileName { get { return (String)GetValue(typeof(String), "FileName"); } set { SetValue("FileName", value); } }
        [DataMember]
        public Decimal FileSize { get { return (Decimal)GetValue(typeof(Decimal), "FileSize"); } set { SetValue("FileSize", value); } }
        [DataMember]
        public DateTime SendAt { get { return (DateTime)GetValue(typeof(DateTime), "SendAt"); } set { SetValue("SendAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String MeHeaderLogo { get { return (String)GetValue(typeof(String), "MeHeaderLogo"); } set { SetValue("MeHeaderLogo", value); } }
        [DataMember]
        public String MeCustomerName { get { return (String)GetValue(typeof(String), "MeCustomerName"); } set { SetValue("MeCustomerName", value); } }
        [DataMember]
        public String ToHeaderLogo { get { return (String)GetValue(typeof(String), "ToHeaderLogo"); } set { SetValue("ToHeaderLogo", value); } }
        [DataMember]
        public String ToCustomerName { get { return (String)GetValue(typeof(String), "ToCustomerName"); } set { SetValue("ToCustomerName", value); } }

        [DataMember]
        public Int32 UnreadCount { get { return (Int32)GetValue(typeof(Int32), "UnreadCount"); } set { SetValue("UnreadCount", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ZXTMessageViewVO tmp = new ZXTMessageViewVO();
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

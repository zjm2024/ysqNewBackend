using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ZXTMessageVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ZXTMessageVO));

        [DataMember]
        public Int32 MessageId { get { return (Int32)GetValue(typeof(Int32), "MessageID"); } set { SetValue("MessageID", value); } }
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

        #region ICloneable Member
        public override object Clone()
        {
            ZXTMessageVO tmp = new ZXTMessageVO();
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

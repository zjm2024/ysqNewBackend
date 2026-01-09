using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ActivityVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ActivityVO));

        [DataMember]
        public Int32 ActId { get { return (Int32)GetValue(typeof(Int32), "ActId"); } set { SetValue("ActId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public Int32 AddressType { get { return (Int32)GetValue(typeof(Int32), "AddressType"); } set { SetValue("AddressType", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public String BossList { get { return (String)GetValue(typeof(String), "BossList"); } set { SetValue("BossList", value); } }
        [DataMember]
        public String BossList2 { get { return (String)GetValue(typeof(String), "BossList2"); } set { SetValue("BossList2", value); } }
        [DataMember]
        public Int32 CountType { get { return (Int32)GetValue(typeof(Int32), "CountType"); } set { SetValue("CountType", value); } }
        [DataMember]
        public DateTime StartAt { get { return (DateTime)GetValue(typeof(DateTime), "StartAt"); } set { SetValue("StartAt", value); } }
        [DataMember]
        public DateTime EndAt { get { return (DateTime)GetValue(typeof(DateTime), "EndAt"); } set { SetValue("EndAt", value); } }
        [DataMember]
        public DateTime SignAt { get { return (DateTime)GetValue(typeof(DateTime), "SignAt"); } set { SetValue("SignAt", value); } }
        [DataMember]
        public String SignUp { get { return (String)GetValue(typeof(String), "SignUp"); } set { SetValue("SignUp", value); } }
        [DataMember]
        public String ForPersonal { get { return (String)GetValue(typeof(String), "ForPersonal"); } set { SetValue("ForPersonal", value); } }
        [DataMember]
        public String CodeImg { get { return (String)GetValue(typeof(String), "CodeImg"); } set { SetValue("CodeImg", value); } }
        [DataMember]
        public String ImgUrls { get { return (String)GetValue(typeof(String), "ImgUrls"); } set { SetValue("ImgUrls", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ActivityVO tmp = new ActivityVO();
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
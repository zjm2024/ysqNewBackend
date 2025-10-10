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
    public partial class ActivityCountVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ActivityCountVO));

        [DataMember]
        public Int32 ActCountId { get { return (Int32)GetValue(typeof(Int32), "ActCountId"); } set { SetValue("ActCountId", value); } }
        [DataMember]
        public Int32 ActId { get { return (Int32)GetValue(typeof(Int32), "ActId"); } set { SetValue("ActId", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public DateTime StartAt { get { return (DateTime)GetValue(typeof(DateTime), "StartAt"); } set { SetValue("StartAt", value); } }
        [DataMember]
        public DateTime EndAt { get { return (DateTime)GetValue(typeof(DateTime), "EndAt"); } set { SetValue("EndAt", value); } }
        [DataMember]
        public DateTime SignAt { get { return (DateTime)GetValue(typeof(DateTime), "SignAt"); } set { SetValue("SignAt", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ActivityCountVO tmp = new ActivityCountVO();
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
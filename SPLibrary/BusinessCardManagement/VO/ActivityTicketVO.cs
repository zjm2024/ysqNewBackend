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
    public partial class ActivityTicketVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ActivityTicketVO));

        [DataMember]
        public Int32 ActTicketId { get { return (Int32)GetValue(typeof(Int32), "ActTicketId"); } set { SetValue("ActTicketId", value); } }
        [DataMember]
        public Int32 ActId { get { return (Int32)GetValue(typeof(Int32), "ActId"); } set { SetValue("ActId", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Int32 Store { get { return (Int32)GetValue(typeof(Int32), "Store"); } set { SetValue("Store", value); } }
        [DataMember]
        public Int32 ComType { get { return (Int32)GetValue(typeof(Int32), "ComType"); } set { SetValue("ComType", value); } }
        [DataMember]
        public Int32 ComCost { get { return (Int32)GetValue(typeof(Int32), "ComCost"); } set { SetValue("ComCost", value); } }
        [DataMember]
        public DateTime EndAt { get { return (DateTime)GetValue(typeof(DateTime), "EndAt"); } set { SetValue("EndAt", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ActivityTicketVO tmp = new ActivityTicketVO();
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
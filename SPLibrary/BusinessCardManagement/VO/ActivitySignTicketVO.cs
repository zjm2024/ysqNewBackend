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
    public partial class ActivitySignTicketVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ActivitySignTicketVO));

        [DataMember]
        public Int32 ActivitySignTicketId { get { return (Int32)GetValue(typeof(Int32), "ActivitySignTicketId"); } set { SetValue("ActivitySignTicketId", value); } }
        [DataMember]
        public Int32 ActTicketId { get { return (Int32)GetValue(typeof(Int32), "ActTicketId"); } set { SetValue("ActTicketId", value); } }
        [DataMember]
        public Int32 ActCountId { get { return (Int32)GetValue(typeof(Int32), "ActCountId"); } set { SetValue("ActCountId", value); } }
        [DataMember]
        public Int32 ActId { get { return (Int32)GetValue(typeof(Int32), "ActId"); } set { SetValue("ActId", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 ForPersonalID { get { return (Int32)GetValue(typeof(Int32), "ForPersonalID"); } set { SetValue("ForPersonalID", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Decimal RewardCost { get { return (Decimal)GetValue(typeof(Decimal), "RewardCost"); } set { SetValue("RewardCost", value); } }
        [DataMember]
        public Int32 IsTX { get { return (Int32)GetValue(typeof(Int32), "IsTX"); } set { SetValue("IsTX", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public Int32 IsYao { get { return (Int32)GetValue(typeof(Int32), "IsYao"); } set { SetValue("IsYao", value); } }
        [DataMember]
        public Int32 IsPay { get { return (Int32)GetValue(typeof(Int32), "IsPay"); } set { SetValue("IsPay", value); } }
        [DataMember]
        public Int32 IsUse { get { return (Int32)GetValue(typeof(Int32), "IsUse"); } set { SetValue("IsUse", value); } }
        [DataMember]
        public String Code { get { return (String)GetValue(typeof(String), "Code"); } set { SetValue("Code", value); } }
        [DataMember]
        public String CodeUrl { get { return (String)GetValue(typeof(String), "CodeUrl"); } set { SetValue("CodeUrl", value); } }
        [DataMember]
        public DateTime UseAt { get { return (DateTime)GetValue(typeof(DateTime), "UseAt"); } set { SetValue("UseAt", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ActivitySignTicketVO tmp = new ActivitySignTicketVO();
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
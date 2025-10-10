using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class PayinHistoryVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(PayinHistoryVO));

        [DataMember]
        public Int32 PayInHistoryId { get { return (Int32)GetValue(typeof(Int32), "PayInHistoryId"); } set { SetValue("PayInHistoryId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public DateTime PayInDate { get { return (DateTime)GetValue(typeof(DateTime), "PayInDate"); } set { SetValue("PayInDate", value); } }
        [DataMember]
        public String PayInOrder { get { return (String)GetValue(typeof(String), "PayInOrder"); } set { SetValue("PayInOrder", value); } }
        [DataMember]
        public Int32 PayInStatus { get { return (Int32)GetValue(typeof(Int32), "PayInStatus"); } set { SetValue("PayInStatus", value); } }
        [DataMember]
        public String ThirdOrder { get { return (String)GetValue(typeof(String), "ThirdOrder"); } set { SetValue("ThirdOrder", value); } }
        [DataMember]
        public String Purpose { get { return (String)GetValue(typeof(String), "Purpose"); } set { SetValue("Purpose", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            PayinHistoryVO tmp = new PayinHistoryVO();
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
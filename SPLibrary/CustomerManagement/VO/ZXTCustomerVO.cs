using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;
namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ZXTCustomerVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerViewVO));

        [DataMember]
        public Int32 CustomerId
        {
            get
            {
                return (Int32)GetValue(typeof(Int32), "CustomerId");
            }
            set
            {
                SetValue("CustomerId", value);
            }
        }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerCode { get { return (String)GetValue(typeof(String), "CustomerCode"); } set { SetValue("CustomerCode", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Boolean Sex { get { return (Boolean)GetValue(typeof(Boolean), "Sex"); } set { SetValue("Sex", value); } }
        [DataMember]
        public DateTime Birthday { get { return (DateTime)GetValue(typeof(DateTime), "Birthday"); } set { SetValue("Birthday", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String SexName { get { return (String)GetValue(typeof(String), "SexName"); } set { SetValue("SexName", value); } }
        [DataMember]
        public String StatusName { get { return (String)GetValue(typeof(String), "StatusName"); } set { SetValue("StatusName", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Int32 BusinessStatus { get { return (Int32)GetValue(typeof(Int32), "BusinessStatus"); } set { SetValue("BusinessStatus", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
        [DataMember]
        public Int32 AgencyStatus { get { return (Int32)GetValue(typeof(Int32), "AgencyStatus"); } set { SetValue("AgencyStatus", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            ZXTCustomerVO tmp = new ZXTCustomerVO();
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

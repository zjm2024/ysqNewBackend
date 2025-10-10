using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class DemandOfferViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(DemandOfferViewVO));

        [DataMember]
        public Int32 OfferId { get { return (Int32)GetValue(typeof(Int32), "OfferId"); } set { SetValue("OfferId", value); } }
        [DataMember]
        public Int32 DemandId { get { return (Int32)GetValue(typeof(Int32), "DemandId"); } set { SetValue("DemandId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String OfferDescription { get { return (String)GetValue(typeof(String), "OfferDescription"); } set { SetValue("OfferDescription", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }

        [DataMember]
        public Int32 BCustomerId { get { return (Int32)GetValue(typeof(Int32), "BCustomerId"); } set { SetValue("BCustomerId", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            DemandOfferViewVO tmp = new DemandOfferViewVO();
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

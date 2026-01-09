using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class FarmgamePrizeOrderViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(FarmgamePrizeOrderViewVO));
       
		[DataMember]
		 public Int32 PrizeOrderID { get { return (Int32)GetValue(typeof(Int32), "PrizeOrderID") ; } set {  SetValue("PrizeOrderID", value); } }
        [DataMember]
        public Int32 PrizeID { get { return (Int32)GetValue(typeof(Int32), "PrizeID"); } set { SetValue("PrizeID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 Price { get { return (Int32)GetValue(typeof(Int32), "Price"); } set { SetValue("Price", value); } }

        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }

        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }

        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }

        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }

        [DataMember]
        public String PrizeName { get { return (String)GetValue(typeof(String), "PrizeName"); } set { SetValue("PrizeName", value); } }

        [DataMember]
        public String ImgUrl { get { return (String)GetValue(typeof(String), "ImgUrl"); } set { SetValue("ImgUrl", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            FarmgamePrizeOrderViewVO tmp = new FarmgamePrizeOrderViewVO();
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
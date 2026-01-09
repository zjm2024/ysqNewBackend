using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class FarmgamePrizeVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(FarmgamePrizeVO));
       
		[DataMember]
		 public Int32 PrizeID { get { return (Int32)GetValue(typeof(Int32), "PrizeID") ; } set {  SetValue("PrizeID", value); } }

        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String ImgUrl { get { return (String)GetValue(typeof(String), "ImgUrl"); } set { SetValue("ImgUrl", value); } }

        [DataMember]
        public Int32 Price { get { return (Int32)GetValue(typeof(Int32), "Price"); } set { SetValue("Price", value); } }
        [DataMember]
        public Int32 OrderNo { get { return (Int32)GetValue(typeof(Int32), "OrderNo"); } set { SetValue("OrderNo", value); } }

        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }

        [DataMember]
        public String ProductUrl { get { return (String)GetValue(typeof(String), "ProductUrl"); } set { SetValue("ProductUrl", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            FarmgamePrizeVO tmp = new FarmgamePrizeVO();
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
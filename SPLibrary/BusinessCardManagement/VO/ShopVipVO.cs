using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ShopVipVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ShopVipVO));
        
		[DataMember]
		public Int32 ShopVipID { get { return (Int32)GetValue(typeof(Int32), "ShopVipID") ; } set {  SetValue("ShopVipID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String VipName { get { return (String)GetValue(typeof(String), "VipName"); } set { SetValue("VipName", value); } }
        [DataMember]
        public Int32 Level { get { return (Int32)GetValue(typeof(Int32), "Level"); } set { SetValue("Level", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ShopVipVO tmp = new ShopVipVO();
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
    public class ProfitsharingToJSON
    {
        public int ShopVipID { set; get; }
        public int Profitsharing { set; get; }=0;
        public int TowProfitsharing { set; get; }=0;
        
    }
    public class VipDiscountToJSON
    {
        public int ShopVipID { set; get; }
        public int VipDiscount { set; get; } = 100;
    }
}
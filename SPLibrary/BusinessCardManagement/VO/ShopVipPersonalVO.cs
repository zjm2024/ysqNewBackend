using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ShopVipPersonalVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ShopVipPersonalVO));

        [DataMember]
        public Int32 ShopVipPersonalID { get { return (Int32)GetValue(typeof(Int32), "ShopVipPersonalID"); } set { SetValue("ShopVipPersonalID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
		public Int32 ShopVipID { get { return (Int32)GetValue(typeof(Int32), "ShopVipID") ; } set {  SetValue("ShopVipID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime ExpirationAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpirationAt"); } set { SetValue("ExpirationAt", value); } }

        [DataMember]
        public String Headimg { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String VipName { get; set; }


        #region ICloneable Member
        public override object Clone()
        {
            ShopVipPersonalVO tmp = new ShopVipPersonalVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardRebateViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardRebateViewVO));

        [DataMember]
		public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Decimal OneRebateCost { get { return (Decimal)GetValue(typeof(Decimal), "OneRebateCost"); } set { SetValue("OneRebateCost", value); } }
        [DataMember]
        public Decimal OneRebateCostNo { get { return (Decimal)GetValue(typeof(Decimal), "OneRebateCostNo"); } set { SetValue("OneRebateCostNo", value); } }

        [DataMember]
        public Decimal TwoRebateCost { get { return (Decimal)GetValue(typeof(Decimal), "TwoRebateCost"); } set { SetValue("TwoRebateCost", value); } }
        [DataMember]
        public Decimal TwoRebateCostNo { get { return (Decimal)GetValue(typeof(Decimal), "TwoRebateCostNo"); } set { SetValue("TwoRebateCostNo", value); } }

        [DataMember]
        public Decimal PromotionAwardCost { get { return (Decimal)GetValue(typeof(Decimal), "PromotionAwardCost"); } set { SetValue("PromotionAwardCost", value); } }
        [DataMember]
        public Decimal PromotionAwardCostNo { get { return (Decimal)GetValue(typeof(Decimal), "PromotionAwardCostNo"); } set { SetValue("PromotionAwardCostNo", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            CardRebateViewVO tmp = new CardRebateViewVO();
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
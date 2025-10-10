using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class CardPartyCostVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardPartyCostVO));

        [DataMember]
        public Int32 PartyCostID { get { return (Int32)GetValue(typeof(Int32), "PartyCostID"); } set { SetValue("PartyCostID", value); } }
        [DataMember]
        public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID"); } set { SetValue("PartyID", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public String Names { get { return (String)GetValue(typeof(String), "Names"); } set { SetValue("Names", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 limitPeopleNum { get { return (Int32)GetValue(typeof(Int32), "limitPeopleNum"); } set { SetValue("limitPeopleNum", value); } }

        [DataMember]
        public DateTime EffectiveTime { get { return (DateTime)GetValue(typeof(DateTime), "EffectiveTime"); } set { SetValue("EffectiveTime", value); } }
        [DataMember]
        public Int32 QuantitySold { get; set; }

        [DataMember]
        public Int32 PromotionAward { get { return (Int32)GetValue(typeof(Int32), "PromotionAward"); } set { SetValue("PromotionAward", value); } }

        [DataMember]
        public Int32 isPromotionSignup { get; set; }
        [DataMember]
        public Int32 MyPromotionSignup { get; set; }
        [DataMember]
        public Int32 PromotionSignup { get { return (Int32)GetValue(typeof(Int32), "PromotionSignup"); } set { SetValue("PromotionSignup", value); } }
        [DataMember]
        public Int32 isPromotionRead { get; set; }
        [DataMember]
        public Int32 MyPromotionRead { get; set; }
        [DataMember]
        public Int32 PromotionRead { get { return (Int32)GetValue(typeof(Int32), "PromotionRead"); } set { SetValue("PromotionRead", value); } }

        [DataMember]
        public Int32 isDiscount { get { return (Int32)GetValue(typeof(Int32), "isDiscount"); } set { SetValue("isDiscount", value); } }
        [DataMember]
        public Decimal DiscountCost { get { return (Decimal)GetValue(typeof(Decimal), "DiscountCost"); } set { SetValue("DiscountCost", value); } }
        [DataMember]
        public DateTime DiscountTime { get { return (DateTime)GetValue(typeof(DateTime), "DiscountTime"); } set { SetValue("DiscountTime", value); } }
        [DataMember]
        public Int32 DiscountNum { get { return (Int32)GetValue(typeof(Int32), "DiscountNum"); } set { SetValue("DiscountNum", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }

        [DataMember]
        public Int32 isAutoPay { get { return (Int32)GetValue(typeof(Int32), "isAutoPay"); } set { SetValue("isAutoPay", value); } }
        [DataMember]
        public Int32 isFirstPrize { get { return (Int32)GetValue(typeof(Int32), "isFirstPrize"); } set { SetValue("isFirstPrize", value); } }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public Int32 maxImum { get { return (Int32)GetValue(typeof(Int32), "maxImum"); } set { SetValue("maxImum", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardPartyCostVO tmp = new CardPartyCostVO();
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

    public class Winning
    {
        public CardPartyCostVO PartyCost { get; set; }
        public List<CardPartySignUpVO> PartySignUp { get; set; }
    }

    public class CostItem
    {
        public String CostName { get; set; }
        public Decimal Cost { get; set; }
        public Int32 People { get; set; }  
    }

    public class PrizeVO
    {
        public String Name { get; set; }
        public String Content { get; set; }
        public Int32 isAutoPay { get; set; }
        public Decimal Cost { get; set; }
    }
}
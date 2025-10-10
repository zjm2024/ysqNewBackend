using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardRankingViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardRankingViewVO));

        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public Int32 OriginCount { get { return (Int32)GetValue(typeof(Int32), "OriginCount"); } set { SetValue("OriginCount", value); } }
        [DataMember]
        public Int32 ReadCardCount { get { return (Int32)GetValue(typeof(Int32), "ReadCardCount"); } set { SetValue("ReadCardCount", value); } }
        [DataMember]
        public Int32 ForwardCardCount { get { return (Int32)GetValue(typeof(Int32), "ForwardCardCount"); } set { SetValue("ForwardCardCount", value); } }
        [DataMember]
        public Int32 PartyCount { get { return (Int32)GetValue(typeof(Int32), "PartyCount"); } set { SetValue("PartyCount", value); } }
        [DataMember]
        public Int32 SoftarticleCount { get { return (Int32)GetValue(typeof(Int32), "SoftarticleCount"); } set { SetValue("SoftarticleCount", value); } }
        [DataMember]
        public Int32 QuestionnaireCount { get { return (Int32)GetValue(typeof(Int32), "QuestionnaireCount"); } set { SetValue("QuestionnaireCount", value); } }
        [DataMember]
        public Int32 PartySignupCount { get { return (Int32)GetValue(typeof(Int32), "PartySignupCount"); } set { SetValue("PartySignupCount", value); } }
        [DataMember]
        public Int32 SoftarticleSignupCount { get { return (Int32)GetValue(typeof(Int32), "SoftarticleSignupCount"); } set { SetValue("SoftarticleSignupCount", value); } }
        [DataMember]
        public Int32 QuestionnaireSignupCount { get { return (Int32)GetValue(typeof(Int32), "QuestionnaireSignupCount"); } set { SetValue("QuestionnaireSignupCount", value); } }
        [DataMember]
        public Int32 Ranking { get { return (Int32)GetValue(typeof(Int32), "Ranking"); } set { SetValue("Ranking", value); } }

        [DataMember]
        public CardDataVO CardData { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            CardRankingViewVO tmp = new CardRankingViewVO();
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
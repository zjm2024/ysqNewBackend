using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardQuestionnaireViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardQuestionnaireViewVO));

        [DataMember]
        public Int32 QuestionnaireID { get { return (Int32)GetValue(typeof(Int32), "QuestionnaireID"); } set { SetValue("QuestionnaireID", value); } }
        [DataMember]
		public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Promoter { get { return (String)GetValue(typeof(String), "Promoter"); } set { SetValue("Promoter", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public Int32 TimeStyle { get { return (Int32)GetValue(typeof(Int32), "TimeStyle"); } set { SetValue("TimeStyle", value); } }
        [DataMember]
        public DateTime EndTime { get { return (DateTime)GetValue(typeof(DateTime), "EndTime"); } set { SetValue("EndTime", value); } }
        [DataMember]
        public Int32 limitPeopleNum { get { return (Int32)GetValue(typeof(Int32), "limitPeopleNum"); } set { SetValue("limitPeopleNum", value); } }
        [DataMember]
        public String QRImg { get { return (String)GetValue(typeof(String), "QRImg"); } set { SetValue("QRImg", value); } }
        [DataMember]
        public String Form { get { return (String)GetValue(typeof(String), "Form"); } set { SetValue("Form", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }

        [DataMember]
        public String TemplateType { get { return (String)GetValue(typeof(String), "TemplateType"); } set { SetValue("TemplateType", value); } }
        [DataMember]
        public Boolean TemplateRecommend { get { return (Boolean)GetValue(typeof(Boolean), "TemplateRecommend"); } set { SetValue("TemplateRecommend", value); } }
        [DataMember]
        public Boolean isTemplate { get { return (Boolean)GetValue(typeof(Boolean), "isTemplate"); } set { SetValue("isTemplate", value); } }

        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String counts { get { return (String)GetValue(typeof(String), "counts"); } set { SetValue("counts", value); } }

        [DataMember]
        public String Instructions { get { return (String)GetValue(typeof(String), "Instructions"); } set { SetValue("Instructions", value); } }

        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public String CardQRImg { get { return (String)GetValue(typeof(String), "CardQRImg"); } set { SetValue("CardQRImg", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }

        [DataMember]
        public Boolean isRepeat { get { return (Boolean)GetValue(typeof(Boolean), "isRepeat"); } set { SetValue("isRepeat", value); } }
        [DataMember]
        public Int32 Style { get { return (Int32)GetValue(typeof(Int32), "Style"); } set { SetValue("Style", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardQuestionnaireViewVO tmp = new CardQuestionnaireViewVO();
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
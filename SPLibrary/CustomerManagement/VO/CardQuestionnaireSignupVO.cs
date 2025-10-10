using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardQuestionnaireSignupVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardQuestionnaireSignupVO));

        [DataMember]
        public Int32 QuestionnaireSignupID { get { return (Int32)GetValue(typeof(Int32), "QuestionnaireSignupID"); } set { SetValue("QuestionnaireSignupID", value); } }
        [DataMember]
        public Int32 QuestionnaireID { get { return (Int32)GetValue(typeof(Int32), "QuestionnaireID"); } set { SetValue("QuestionnaireID", value); } }
        [DataMember]
		public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String SigupForm { get { return (String)GetValue(typeof(String), "SigupForm"); } set { SetValue("SigupForm", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 InviterCID { get { return (Int32)GetValue(typeof(Int32), "InviterCID"); } set { SetValue("InviterCID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardQuestionnaireSignupVO tmp = new CardQuestionnaireSignupVO();
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
    public class QuestionnaireSigupForm {
        public String Name { get; set; }
        public Int32 Status { get; set; }
        public Int32 must { get; set; }
        public Int32 Type { get; set; }
        public String value { get; set; }
        public List<UrlList> UrlList { get; set; }
    }
    public class UrlList
    {
        public String url { get; set; }
    }
}
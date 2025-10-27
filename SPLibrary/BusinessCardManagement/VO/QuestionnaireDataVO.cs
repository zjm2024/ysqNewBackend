using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class QuestionnaireDataVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(QuestionnaireDataVO));

        [DataMember]
        public Int32 QuestionId { get { return (Int32)GetValue(typeof(Int32), "QuestionId"); } set { SetValue("QuestionId", value); } }
        [DataMember]
        public String activity_id { get { return (String)GetValue(typeof(String), "activity_id"); } set { SetValue("activity_id", value); } }
        [DataMember]
        public String activity_name { get { return (String)GetValue(typeof(String), "activity_name"); } set { SetValue("activity_name", value); } }
        [DataMember]
        public String activity_img { get { return (String)GetValue(typeof(String), "activity_img"); } set { SetValue("activity_img", value); } }
        [DataMember]
        public String activity_domain { get { return (String)GetValue(typeof(String), "activity_domain"); } set { SetValue("activity_domain", value); } }
        [DataMember]
        public String activity_pc_url { get { return (String)GetValue(typeof(String), "activity_pc_url"); } set { SetValue("activity_pc_url", value); } }
        [DataMember]
        public String activity_h5_url { get { return (String)GetValue(typeof(String), "activity_h5_url"); } set { SetValue("activity_h5_url", value); } }
        [DataMember]
        public String content { get { return (String)GetValue(typeof(String), "content"); } set { SetValue("content", value); } }
        [DataMember]
        public String custom_params { get { return (String)GetValue(typeof(String), "custom_params"); } set { SetValue("custom_params", value); } }
        [DataMember]
        public String activity_description { get { return (String)GetValue(typeof(String), "activity_description"); } set { SetValue("activity_description", value); } }
        [DataMember]
        public String txt1 { get { return (String)GetValue(typeof(String), "txt1"); } set { SetValue("txt1", value); } }
        [DataMember]
        public String txt2 { get { return (String)GetValue(typeof(String), "txt2"); } set { SetValue("txt2", value); } }
        [DataMember]
        public Int32 status { get { return (Int32)GetValue(typeof(Int32), "status"); } set { SetValue("status", value); } }
        [DataMember]
        public Int32 sort { get { return (Int32)GetValue(typeof(Int32), "sort"); } set { SetValue("sort", value); } }
        [DataMember]
        public Int32 need_obtain_ip { get { return (Int32)GetValue(typeof(Int32), "need_obtain_ip"); } set { SetValue("need_obtain_ip", value); } }
        [DataMember]
        public String notice { get { return (String)GetValue(typeof(String), "notice"); } set { SetValue("notice", value); } }
        [DataMember]
        public Int32 enablerecording { get { return (Int32)GetValue(typeof(Int32), "enablerecording"); } set { SetValue("enablerecording", value); } }

        [DataMember]
        public DateTime create_time { get { return (DateTime)GetValue(typeof(DateTime), "create_time"); } set { SetValue("create_time", value); } }
        [DataMember]
        public DateTime update_time { get { return (DateTime)GetValue(typeof(DateTime), "update_time"); } set { SetValue("update_time", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            QuestionnaireDataVO tmp = new QuestionnaireDataVO();
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
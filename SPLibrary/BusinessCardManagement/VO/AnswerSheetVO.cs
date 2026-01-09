using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AnswerSheetVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AnswerSheetVO));

        [DataMember]
        public Int32 AnswerSheetId { get { return (Int32)GetValue(typeof(Int32), "AnswerSheetId"); } set { SetValue("AnswerSheetId", value); } }
        [DataMember]
        public String activity_id { get { return (String)GetValue(typeof(String), "activity_id"); } set { SetValue("activity_id", value); } }
        [DataMember]
        public String content { get { return (String)GetValue(typeof(String), "content"); } set { SetValue("content", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public DateTime createdAt { get { return (DateTime)GetValue(typeof(DateTime), "createdAt"); } set { SetValue("createdAt", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            AnswerSheetVO tmp = new AnswerSheetVO();
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
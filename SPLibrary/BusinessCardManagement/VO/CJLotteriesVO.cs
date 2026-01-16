using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CJLotteriesVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CJLotteriesVO));

        [DataMember]
        public Int32 lottery_id { get { return (Int32)GetValue(typeof(Int32), "lottery_id"); } set { SetValue("lottery_id", value); } }
        [DataMember]
        public Int32 personal_id { get { return (Int32)GetValue(typeof(Int32), "personal_id"); } set { SetValue("personal_id", value); } }
        [DataMember]
        public String title { get { return (String)GetValue(typeof(String), "title"); } set { SetValue("title", value); } }
        [DataMember]
        public String cover_image { get { return (String)GetValue(typeof(String), "cover_image"); } set { SetValue("cover_image", value); } }
        [DataMember]
        public String description { get { return (String)GetValue(typeof(String), "description"); } set { SetValue("description", value); } }
        [DataMember]
        public DateTime end_time { get { return (DateTime)GetValue(typeof(DateTime), "end_time"); } set { SetValue("end_time", value); } }
        [DataMember]
        public DateTime start_time { get { return (DateTime)GetValue(typeof(DateTime), "start_time"); } set { SetValue("start_time", value); } }
        [DataMember]
        public Int32 status { get { return (Int32)GetValue(typeof(Int32), "status") ; } set {  SetValue("status", value); } }
        [DataMember]
        public Int32 participation_limit { get { return (Int32)GetValue(typeof(Int32), "participation_limit"); } set { SetValue("participation_limit", value); } }
        [DataMember]
        public Int32 require_follow { get { return (Int32)GetValue(typeof(Int32), "require_follow"); } set { SetValue("require_follow", value); } }
        [DataMember]
        public Int32 require_share { get { return (Int32)GetValue(typeof(Int32), "require_share"); } set { SetValue("require_share", value); } }
        [DataMember]
        public Int32 require_contact { get { return (Int32)GetValue(typeof(Int32), "require_contact"); } set { SetValue("require_contact", value); } }
        [DataMember]
        public decimal total_amount { get { return (decimal)GetValue(typeof(decimal), "total_amount"); } set { SetValue("total_amount", value); } }
        [DataMember]
        public decimal min_value { get { return (decimal)GetValue(typeof(decimal), "min_value"); } set { SetValue("min_value", value); } }
        [DataMember]
        public decimal max_value { get { return (decimal)GetValue(typeof(decimal), "max_value"); } set { SetValue("max_value", value); } }
        [DataMember]
        public Int32 winner_count { get { return (Int32)GetValue(typeof(Int32), "winner_count"); } set { SetValue("winner_count", value); } }
        [DataMember]
        public Int32 participant_count { get { return (Int32)GetValue(typeof(Int32), "participant_count"); } set { SetValue("participant_count", value); } }
        [DataMember]
        public Int32 view_count { get { return (Int32)GetValue(typeof(Int32), "view_count"); } set { SetValue("view_count", value); } }
        [DataMember]
        public Int32 share_count { get { return (Int32)GetValue(typeof(Int32), "share_count"); } set { SetValue("share_count", value); } }
        [DataMember]
        public Int32 is_deleted { get { return (Int32)GetValue(typeof(Int32), "is_deleted"); } set { SetValue("is_deleted", value); } }
        [DataMember]
        public DateTime created_at { get { return (DateTime)GetValue(typeof(DateTime), "created_at"); } set { SetValue("created_at", value); } }
        [DataMember]
        public DateTime updated_at { get { return (DateTime)GetValue(typeof(DateTime), "updated_at"); } set { SetValue("updated_at", value); } }
        [DataMember]
        public String remark { get { return (String)GetValue(typeof(String), "remark"); } set { SetValue("remark", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CJLotteriesVO tmp = new CJLotteriesVO();
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
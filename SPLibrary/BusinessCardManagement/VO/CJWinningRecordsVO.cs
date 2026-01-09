using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CJWinningRecordsVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CJWinningRecordsVO));

        [DataMember]
        public Int32 winningrecords_id { get { return (Int32)GetValue(typeof(Int32), "winningrecords_id"); } set { SetValue("winningrecords_id", value); } }
        [DataMember]
        public Int32 lottery_id { get { return (Int32)GetValue(typeof(Int32), "lottery_id"); } set { SetValue("lottery_id", value); } }
        [DataMember]
        public Int32 personal_id { get { return (Int32)GetValue(typeof(Int32), "personal_id"); } set { SetValue("personal_id", value); } }
        [DataMember]
        public DateTime winning_time { get { return (DateTime)GetValue(typeof(DateTime), "winning_time"); } set { SetValue("winning_time", value); } }
        [DataMember]
		public decimal winning_amount { get { return (decimal)GetValue(typeof(decimal), "winning_amount") ; } set {  SetValue("winning_amount", value); } }
        [DataMember]
        public Int32 status { get { return (Int32)GetValue(typeof(Int32), "status"); } set { SetValue("status", value); } }
        [DataMember]
        public String contact_info { get { return (String)GetValue(typeof(String), "contact_info"); } set { SetValue("contact_info", value); } }

        [DataMember]
        public DateTime created_at { get { return (DateTime)GetValue(typeof(DateTime), "created_at"); } set { SetValue("created_at", value); } }
        [DataMember]
        public String openid { get { return (String)GetValue(typeof(String), "openid"); } set { SetValue("openid", value); } }
        [DataMember]
        public String payment_no { get { return (String)GetValue(typeof(String), "payment_no"); } set { SetValue("payment_no", value); } }
        [DataMember]
        public DateTime payment_time { get { return (DateTime)GetValue(typeof(DateTime), "payment_time"); } set { SetValue("payment_time", value); } }
        [DataMember]
        public DateTime updated_at { get { return (DateTime)GetValue(typeof(DateTime), "updated_at"); } set { SetValue("updated_at", value); } }
        [DataMember]
        public Int32 apptype { get { return (Int32)GetValue(typeof(Int32), "apptype"); } set { SetValue("apptype", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CJWinningRecordsVO tmp = new CJWinningRecordsVO();
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
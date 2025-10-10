using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class PunchVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(PunchVO));

        [DataMember]
        public Int32 PunchID { get { return (Int32)GetValue(typeof(Int32), "PunchID"); } set { SetValue("PunchID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public DateTime PunchInAt { get { return (DateTime)GetValue(typeof(DateTime), "PunchInAt"); } set { SetValue("PunchInAt", value); } }
        [DataMember]
        public String PunchInAddress { get { return (String)GetValue(typeof(String), "PunchInAddress"); } set { SetValue("PunchInAddress", value); } }
        [DataMember]
        public Decimal PunchInLatitude { get { return (Decimal)GetValue(typeof(Decimal), "PunchInLatitude"); } set { SetValue("PunchInLatitude", value); } }
        [DataMember]
        public Decimal PunchInLongitude { get { return (Decimal)GetValue(typeof(Decimal), "PunchInLongitude"); } set { SetValue("PunchInLongitude", value); } }
        
        [DataMember]
        public DateTime PunchOutAt { get { return (DateTime)GetValue(typeof(DateTime), "PunchOutAt"); } set { SetValue("PunchOutAt", value); } }
        [DataMember]
        public String PunchOutAddress { get { return (String)GetValue(typeof(String), "PunchOutAddress"); } set { SetValue("PunchOutAddress", value); } }
        [DataMember]
        public Decimal PunchOutLatitude { get { return (Decimal)GetValue(typeof(Decimal), "PunchOutLatitude"); } set { SetValue("PunchOutLatitude", value); } }
        [DataMember]
        public Decimal PunchOutLongitude { get { return (Decimal)GetValue(typeof(Decimal), "PunchOutLongitude"); } set { SetValue("PunchOutLongitude", value); } }
        [DataMember]
        public Boolean isPunchOut { get { return (Boolean)GetValue(typeof(Boolean), "isPunchOut"); } set { SetValue("isPunchOut", value); } }


        [DataMember]
        public List<CrmVO> CrmList { get; set; }
        [DataMember]
        public Boolean isDaily { get; set; }
        [DataMember]
        public Int32 GoOutCount { get; set; }
        [DataMember]
        public Int32 DailyID { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            PunchVO tmp = new PunchVO();
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
    public partial class PersonalPunchVO
    {
        [DataMember]
        public Int32 PersonalID { get; set; }
        [DataMember]
        public Int32 BusinessID { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Phone { get; set; }
        [DataMember]
        public String Headimg { get; set; }
        [DataMember]
        public String Position { get; set; }
        [DataMember]
        public Int32 PunchInCount { get; set; }
        [DataMember]
        public Int32 PunchOutCount { get; set; }
        [DataMember]
        public Int32 DailyCount { get; set; }
        [DataMember]
        public Int32 GoOutCount { get; set; }
        [DataMember]
        public Boolean isWork { get; set; }
        [DataMember]
        public Int32 WorkStatus { get; set; }
    }

    public partial class PunchMapVO
    {
        [DataMember]
        public Int32 PersonalID { get; set; }
        [DataMember]
        public Int32 BusinessID { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Phone { get; set; }
        [DataMember]
        public String Headimg { get; set; }
        [DataMember]
        public String HeadimgByPunchIn { get; set; }
        [DataMember]
        public String HeadimgByPunchOut { get; set; }
        [DataMember]
        public String HeadimgByGoOut { get; set; }
        [DataMember]
        public String Position { get; set; }
        [DataMember]
        public List<MapVO> MapList { get; set; }
    }

    public partial class MapVO
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Address { get; set; }
        [DataMember]
        public Decimal Latitude { get; set; }
        [DataMember]
        public Decimal Longitude { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public String Type { get; set; }
    }
}
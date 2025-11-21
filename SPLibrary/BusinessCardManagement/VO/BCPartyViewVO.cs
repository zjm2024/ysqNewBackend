using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BCPartyViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BCPartyViewVO));

        [DataMember]
        public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID"); } set { SetValue("PartyID", value); } }

        [DataMember]
        public Int32 HostCustomerId { get { return (Int32)GetValue(typeof(Int32), "HostCustomerId"); } set { SetValue("HostCustomerId", value); } }

        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }

        [DataMember]
        public String MainImg { get { return (String)GetValue(typeof(String), "MainImg"); } set { SetValue("MainImg", value); } }

        [DataMember]
        public String QRImg { get { return (String)GetValue(typeof(String), "QRImg"); } set { SetValue("QRImg", value); } }

        [DataMember]
        public DateTime StartTime { get { return (DateTime)GetValue(typeof(DateTime), "StartTime"); } set { SetValue("StartTime", value); } }

        [DataMember]
        public DateTime EndTime { get { return (DateTime)GetValue(typeof(DateTime), "EndTime"); } set { SetValue("EndTime", value); } }

        [DataMember]
        public DateTime SignUpTime { get { return (DateTime)GetValue(typeof(DateTime), "SignUpTime"); } set { SetValue("SignUpTime", value); } }

        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }

        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "latitude"); } set { SetValue("latitude", value); } }

        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "longitude"); } set { SetValue("longitude", value); } }

        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }

        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }

        [DataMember]
        public String PosterImg { get { return (String)GetValue(typeof(String), "PosterImg"); } set { SetValue("PosterImg", value); } }

        [DataMember]
        public String Host { get { return (String)GetValue(typeof(String), "Host"); } set { SetValue("Host", value); } }

        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }

        [DataMember]
        public Int32 Forward { get { return (Int32)GetValue(typeof(Int32), "Forward"); } set { SetValue("Forward", value); } }

        [DataMember]
        public Int32 limitPeopleNum { get { return (Int32)GetValue(typeof(Int32), "limitPeopleNum"); } set { SetValue("limitPeopleNum", value); } }

        [DataMember]
        public Int32 GroupID { get { return (Int32)GetValue(typeof(Int32), "GroupID"); } set { SetValue("GroupID", value); } }

        [DataMember]
        public Int32 LuckDrawType { get { return (Int32)GetValue(typeof(Int32), "LuckDrawType"); } set { SetValue("LuckDrawType", value); } }

        [DataMember]
        public Int32 IsDisplayIndex { get { return (Int32)GetValue(typeof(Int32), "IsDisplayIndex"); } set { SetValue("IsDisplayIndex", value); } }

        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }

        [DataMember]
        public Int32 isEndTime { get { return (Int32)GetValue(typeof(Int32), "isEndTime"); } set { SetValue("isEndTime", value); } }

        [DataMember]
        public Int32 isStartTime { get { return (Int32)GetValue(typeof(Int32), "isStartTime"); } set { SetValue("isStartTime", value); } }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public Int32 IntegrateCount { get { return (Int32)GetValue(typeof(Int32), "IntegrateCount"); } set { SetValue("IntegrateCount", value); } }

        [DataMember]
        public Int32 PartyLuckDrawStatus { get { return (Int32)GetValue(typeof(Int32), "PartyLuckDrawStatus"); } set { SetValue("PartyLuckDrawStatus", value); } }

        [DataMember]
        public DateTime LuckDrawAt { get { return (DateTime)GetValue(typeof(DateTime), "LuckDrawAt"); } set { SetValue("LuckDrawAt", value); } }

        [DataMember]
        public String DetailedAddress { get { return (String)GetValue(typeof(String), "DetailedAddress"); } set { SetValue("DetailedAddress", value); } }

        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }

        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }

        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }

        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }

        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public List<BCPartyContactsViewVO> BCPartyContactsView { get; set; }

        [DataMember]
        public List<BCPartySignUpVO> BCPartySignUp { get; set; }




        #region ICloneable Member
        public override object Clone()
        {
            BCPartyViewVO tmp = new BCPartyViewVO();
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
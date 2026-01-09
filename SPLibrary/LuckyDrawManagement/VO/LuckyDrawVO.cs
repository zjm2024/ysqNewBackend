using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.LuckyDrawManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class LuckyDrawVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(LuckyDrawVO));
       
		[DataMember]
		public Int32 LuckyDrawID { get { return (Int32)GetValue(typeof(Int32), "LuckyDrawID") ; } set {  SetValue("LuckyDrawID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }

        [DataMember]
        public Int32 LotteryType { get { return (Int32)GetValue(typeof(Int32), "LotteryType"); } set { SetValue("LotteryType", value); } }
        [DataMember]
        public Int32 LotteryNumber { get { return (Int32)GetValue(typeof(Int32), "LotteryNumber"); } set { SetValue("LotteryNumber", value); } }

        [DataMember]
        public DateTime LotteryTime { get { return (DateTime)GetValue(typeof(DateTime), "LotteryTime"); } set { SetValue("LotteryTime", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }

        [DataMember]
        public Int32 FollowType { get { return (Int32)GetValue(typeof(Int32), "FollowType"); } set { SetValue("FollowType", value); } }
        [DataMember]
        public Int32 ShareType { get { return (Int32)GetValue(typeof(Int32), "ShareType"); } set { SetValue("ShareType", value); } }

        [DataMember]
        public String FollowAccount { get { return (String)GetValue(typeof(String), "FollowAccount"); } set { SetValue("FollowAccount", value); } }
        [DataMember]
        public String FollowText { get { return (String)GetValue(typeof(String), "FollowText"); } set { SetValue("FollowText", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }

        [DataMember]
        public List<PrizeVO> PrizeList { get; set; }


        #region ICloneable Member
        public override object Clone()
        {
            LuckyDrawVO tmp = new LuckyDrawVO();
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
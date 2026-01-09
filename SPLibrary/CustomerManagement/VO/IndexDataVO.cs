using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class IndexDataVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(IndexDataVO));
       
		[DataMember]
		 public Int32 IndexDataID { get { return (Int32)GetValue(typeof(Int32), "IndexDataID") ; } set {  SetValue("IndexDataID", value); } }
        [DataMember]
		 public String ADList { get { return (String)GetValue(typeof(String), "ADList") ; } set {  SetValue("ADList", value); } }
        [DataMember]
        public Int32 PartyCount { get { return (Int32)GetValue(typeof(Int32), "PartyCount"); } set { SetValue("PartyCount", value); } }
        [DataMember]
        public Int32 GoodsCount { get { return (Int32)GetValue(typeof(Int32), "GoodsCount"); } set { SetValue("GoodsCount", value); } }
        [DataMember]
        public Int32 PosterCount { get { return (Int32)GetValue(typeof(Int32), "PosterCount"); } set { SetValue("PosterCount", value); } }
        [DataMember]
        public Int32 SoftarticleCount { get { return (Int32)GetValue(typeof(Int32), "SoftarticleCount"); } set { SetValue("SoftarticleCount", value); } }
        [DataMember]
        public Int32 QuestionnaireCount { get { return (Int32)GetValue(typeof(Int32), "QuestionnaireCount"); } set { SetValue("QuestionnaireCount", value); } }
        [DataMember]
        public Int32 GroupCount { get { return (Int32)GetValue(typeof(Int32), "GroupCount"); } set { SetValue("GroupCount", value); } }
        [DataMember]
        public Int32 DemandCount { get { return (Int32)GetValue(typeof(Int32), "DemandCount"); } set { SetValue("DemandCount", value); } }
        [DataMember]
        public Int32 BusinessCardCount { get { return (Int32)GetValue(typeof(Int32), "BusinessCardCount"); } set { SetValue("BusinessCardCount", value); } }
        [DataMember]
        public Int32 CompanyCount { get { return (Int32)GetValue(typeof(Int32), "CompanyCount"); } set { SetValue("CompanyCount", value); } }
        [DataMember]
        public Int32 LuckDrawCount { get { return (Int32)GetValue(typeof(Int32), "LuckDrawCount"); } set { SetValue("LuckDrawCount", value); } }
        [DataMember]
        public DateTime UpDataAt { get { return (DateTime)GetValue(typeof(DateTime), "UpDataAt"); } set { SetValue("UpDataAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            IndexDataVO tmp = new IndexDataVO();
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
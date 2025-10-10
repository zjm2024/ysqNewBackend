using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardSoftArticleVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardSoftArticleVO));

        [DataMember]
        public Int32 SoftArticleID { get { return (Int32)GetValue(typeof(Int32), "SoftArticleID"); } set { SetValue("SoftArticleID", value); } }
        [DataMember]
        public Int32 OriginalSoftArticleID { get { return (Int32)GetValue(typeof(Int32), "OriginalSoftArticleID"); } set { SetValue("OriginalSoftArticleID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public Boolean IsShowCard { get { return (Boolean)GetValue(typeof(Boolean), "IsShowCard"); } set { SetValue("IsShowCard", value); } }
        [DataMember]
        public Int32 OriginalCustomerId { get { return (Int32)GetValue(typeof(Int32), "OriginalCustomerId"); } set { SetValue("OriginalCustomerId", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public String Video { get { return (String)GetValue(typeof(String), "Video"); } set { SetValue("Video", value); } }
        [DataMember]
        public Int32 isVideo { get { return (Int32)GetValue(typeof(Int32), "isVideo"); } set { SetValue("isVideo", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Boolean IsCost { get { return (Boolean)GetValue(typeof(Boolean), "IsCost"); } set { SetValue("IsCost", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID"); } set { SetValue("PartyID", value); } }  
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }
        [DataMember]
        public Int32 ReprintCount { get { return (Int32)GetValue(typeof(Int32), "ReprintCount"); } set { SetValue("ReprintCount", value); } }
        [DataMember]
        public Int32 GoodCount { get { return (Int32)GetValue(typeof(Int32), "GoodCount"); } set { SetValue("GoodCount", value); } }
        [DataMember]
        public Int32 ExposureCount { get { return (Int32)GetValue(typeof(Int32), "ExposureCount"); } set { SetValue("ExposureCount", value); } }
        [DataMember]
        public Boolean IsOriginal { get { return (Boolean)GetValue(typeof(Boolean), "IsOriginal"); } set { SetValue("IsOriginal", value); } }
        [DataMember]
        public String OriginalName { get { return (String)GetValue(typeof(String), "OriginalName"); } set { SetValue("OriginalName", value); } }
        [DataMember]
        public String OriginalPlatform { get { return (String)GetValue(typeof(String), "OriginalPlatform"); } set { SetValue("OriginalPlatform", value); } }
        [DataMember]
        public String OriginalMedia { get { return (String)GetValue(typeof(String), "OriginalMedia"); } set { SetValue("OriginalMedia", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String QRImg { get { return (String)GetValue(typeof(String), "QRImg"); } set { SetValue("QRImg", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public CardDataVO Card { get; set; }
        [DataMember]
        public string Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public string Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public CardDataVO OriginalCard { get; set; }
        [DataMember]
        public Int32 MediaID { get { return (Int32)GetValue(typeof(Int32), "MediaID"); } set { SetValue("MediaID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        

        #region ICloneable Member
        public override object Clone()
        {
            CardSoftArticleVO tmp = new CardSoftArticleVO();
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
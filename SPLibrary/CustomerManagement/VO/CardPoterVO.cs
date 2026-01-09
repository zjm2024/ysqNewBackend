using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardPoterVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardPoterVO));

        [DataMember]
        public Int32 CardPoterID { get { return (Int32)GetValue(typeof(Int32), "CardPoterID"); } set { SetValue("CardPoterID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String FileName { get { return (String)GetValue(typeof(String), "FileName"); } set { SetValue("FileName", value); } }
        [DataMember]
        public String Url { get { return (String)GetValue(typeof(String), "Url"); } set { SetValue("Url", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public Int32 UploadPc { get { return (Int32)GetValue(typeof(Int32), "UploadPc"); } set { SetValue("UploadPc", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 SizeType { get { return (Int32)GetValue(typeof(Int32), "SizeType"); } set { SetValue("SizeType", value); } }

        [DataMember]
        public Int32 Order_info { get { return (Int32)GetValue(typeof(Int32), "Order_info"); } set { SetValue("Order_info", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardPoterVO tmp = new CardPoterVO();
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
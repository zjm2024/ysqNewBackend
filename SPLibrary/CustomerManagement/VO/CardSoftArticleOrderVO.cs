using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardSoftArticleOrderVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardSoftArticleOrderVO));

        [DataMember]
        public Int32 SoftArticleOrderID { get { return (Int32)GetValue(typeof(Int32), "SoftArticleOrderID"); } set { SetValue("SoftArticleOrderID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 SoftArticleID { get { return (Int32)GetValue(typeof(Int32), "SoftArticleID"); } set { SetValue("SoftArticleID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String OrderNO { get { return (String)GetValue(typeof(String), "OrderNO"); } set { SetValue("OrderNO", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } } 
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public Int32 NewSoftArticleID { get { return (Int32)GetValue(typeof(Int32), "NewSoftArticleID"); } set { SetValue("NewSoftArticleID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardSoftArticleOrderVO tmp = new CardSoftArticleOrderVO();
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

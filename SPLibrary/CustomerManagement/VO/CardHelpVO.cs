using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardHelpVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardHelpVO));

        [DataMember]
        public Int32 HelpID { get { return (Int32)GetValue(typeof(Int32), "HelpID"); } set { SetValue("HelpID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public String Url { get { return (String)GetValue(typeof(String), "Url"); } set { SetValue("Url", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Order_Num { get { return (Int32)GetValue(typeof(Int32), "Order_Num"); } set { SetValue("Order_Num", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public String finderUserName { get { return (String)GetValue(typeof(String), "finderUserName"); } set { SetValue("finderUserName", value); } }
        [DataMember]
        public String feedId { get { return (String)GetValue(typeof(String), "feedId"); } set { SetValue("feedId", value); } }
        [DataMember]
        public Int32 isFinder { get { return (Int32)GetValue(typeof(Int32), "isFinder"); } set { SetValue("isFinder", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardHelpVO tmp = new CardHelpVO();
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
    public class HelpList
    {
        public string Title { get; set; }
        public List<CardHelpVO> List { get; set; }
    } 
}
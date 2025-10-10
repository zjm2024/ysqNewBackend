using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardKeywordViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardKeywordViewVO));

        [DataMember]
        public Int32 KeywordID { get { return (Int32)GetValue(typeof(Int32), "KeywordID"); } set { SetValue("KeywordID", value); } }
        [DataMember]
		 public String Keyword { get { return (String)GetValue(typeof(String), "Keyword") ; } set {  SetValue("Keyword", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public Int32 Count { get { return (Int32)GetValue(typeof(Int32), "Count"); } set { SetValue("Count", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardKeywordViewVO tmp = new CardKeywordViewVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardKeywordVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardKeywordVO));

        [DataMember]
        public Int32 KeywordID { get { return (Int32)GetValue(typeof(Int32), "KeywordID"); } set { SetValue("KeywordID", value); } }
        [DataMember]
		 public String Keyword { get { return (String)GetValue(typeof(String), "Keyword") ; } set {  SetValue("Keyword", value); } }
        [DataMember]
		 public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt") ; } set {  SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardKeywordVO tmp = new CardKeywordVO();
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
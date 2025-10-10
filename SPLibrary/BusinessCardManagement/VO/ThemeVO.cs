using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ThemeVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ThemeVO));

        [DataMember]
        public Int32 ThemeID { get { return (Int32)GetValue(typeof(Int32), "ThemeID"); } set { SetValue("ThemeID", value); } }
        [DataMember]
        public String ThemeName { get { return (String)GetValue(typeof(String), "ThemeName"); } set { SetValue("ThemeName", value); } }
        [DataMember]
		 public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID") ; } set {  SetValue("BusinessID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String CardBackImg { get { return (String)GetValue(typeof(String), "CardBackImg"); } set { SetValue("CardBackImg", value); } }
        [DataMember]
        public String ShowBackColor { get { return (String)GetValue(typeof(String), "ShowBackColor"); } set { SetValue("ShowBackColor", value); } }

        [DataMember]
        public Int32 isShowLogo { get { return (Int32)GetValue(typeof(Int32), "isShowLogo"); } set { SetValue("isShowLogo", value); } }
        [DataMember]
        public Int32 isShowData { get { return (Int32)GetValue(typeof(Int32), "isShowData"); } set { SetValue("isShowData", value); } }
        
        [DataMember]
        public String CardFontColor { get { return (String)GetValue(typeof(String), "CardFontColor"); } set { SetValue("CardFontColor", value); } }
        [DataMember]
        public String NavFontColor { get { return (String)GetValue(typeof(String), "NavFontColor"); } set { SetValue("NavFontColor", value); } }

        public void setThemeVO()
        {
            this.CardFontColor = "black";
            this.CardBackImg = "https://www.zhongxiaole.net/SPManager/Style/images/wxBusinessCard/cardback2.jpg";
            this.ShowBackColor = "rgb(41,171,226)";
            this.isShowLogo = 0;
            this.isShowData = 1;
            this.NavFontColor = "white";
        }

        #region ICloneable Member
        public override object Clone()
        {
            ThemeVO tmp = new ThemeVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class GreetingCardVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(GreetingCardVO));
        
		[DataMember]
		public Int32 GreetingCardID { get { return (Int32)GetValue(typeof(Int32), "GreetingCardID") ; } set {  SetValue("GreetingCardID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public String Audio { get { return (String)GetValue(typeof(String), "Audio"); } set { SetValue("Audio", value); } }
        [DataMember]
        public String ShareText { get { return (String)GetValue(typeof(String), "ShareText"); } set { SetValue("ShareText", value); } }
        [DataMember]
        public String ShareImage { get { return (String)GetValue(typeof(String), "ShareImage"); } set { SetValue("ShareImage", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public String Video { get { return (String)GetValue(typeof(String), "Video"); } set { SetValue("Video", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String Fit { get { return (String)GetValue(typeof(String), "Fit"); } set { SetValue("Fit", value); } }
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            GreetingCardVO tmp = new GreetingCardVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ShortUrlVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ShortUrlVO));
        
		[DataMember]
		public Int32 ShortUrlID { get { return (Int32)GetValue(typeof(Int32), "ShortUrlID") ; } set {  SetValue("ShortUrlID", value); } }
        [DataMember]
        public String Code { get { return (String)GetValue(typeof(String), "Code"); } set { SetValue("Code", value); } }
        [DataMember]
        public String Url { get { return (String)GetValue(typeof(String), "Url"); } set { SetValue("Url", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ShortUrlVO tmp = new ShortUrlVO();
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
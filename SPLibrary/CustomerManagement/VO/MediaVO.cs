using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class MediaVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(MediaVO));
       
		[DataMember]
		 public Int32 MediaID { get { return (Int32)GetValue(typeof(Int32), "MediaID") ; } set {  SetValue("MediaID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
		 public String media_id { get { return (String)GetValue(typeof(String), "media_id") ; } set {  SetValue("media_id", value); } }
        [DataMember]
        public String title { get { return (String)GetValue(typeof(String), "title"); } set { SetValue("title", value); } }
        [DataMember]
        public String content { get { return (String)GetValue(typeof(String), "content"); } set { SetValue("content", value); } }
        [DataMember]
        public String msg_id { get { return (String)GetValue(typeof(String), "msg_id"); } set { SetValue("msg_id", value); } }
        [DataMember]
        public String msg_data_id { get { return (String)GetValue(typeof(String), "msg_data_id"); } set { SetValue("msg_data_id", value); } }
        [DataMember]
        public String ArticleUrl { get { return (String)GetValue(typeof(String), "ArticleUrl"); } set { SetValue("ArticleUrl", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            MediaVO tmp = new MediaVO();
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
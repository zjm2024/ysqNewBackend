using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CommentViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CommentViewVO));
        
		[DataMember]
		public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID") ; } set {  SetValue("InfoID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 toPersonalID { get { return (Int32)GetValue(typeof(Int32), "toPersonalID"); } set { SetValue("toPersonalID", value); } }
        [DataMember]
        public String CrmType { get { return (String)GetValue(typeof(String), "CrmType"); } set { SetValue("CrmType", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 CrmID { get { return (Int32)GetValue(typeof(Int32), "CrmID"); } set { SetValue("CrmID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CommentViewVO tmp = new CommentViewVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ProfitsharingVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProfitsharingVO));

        [DataMember]
        public Int32 ProfitsharingID { get { return (Int32)GetValue(typeof(Int32), "ProfitsharingID"); } set { SetValue("ProfitsharingID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 ToCustomerId { get { return (Int32)GetValue(typeof(Int32), "ToCustomerId"); } set { SetValue("ToCustomerId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String ToOpenID { get { return (String)GetValue(typeof(String), "ToOpenID"); } set { SetValue("ToOpenID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            ProfitsharingVO tmp = new ProfitsharingVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ViolationVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ViolationVO));

        [DataMember]
        public Int32 ViolationID { get { return (Int32)GetValue(typeof(Int32), "ViolationID"); } set { SetValue("ViolationID", value); } }
        [DataMember]
        public String ViolationText { get { return (String)GetValue(typeof(String), "ViolationText"); } set { SetValue("ViolationText", value); } }
        [DataMember]
        public DateTime ViolationAt { get { return (DateTime)GetValue(typeof(DateTime), "ViolationAt"); } set { SetValue("ViolationAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ViolationVO tmp = new ViolationVO();
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
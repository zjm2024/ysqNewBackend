using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CallNumberVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CallNumberVO));

        [DataMember]
        public Int32 NumberID { get { return (Int32)GetValue(typeof(Int32), "NumberID"); } set { SetValue("NumberID", value); } }
        [DataMember]
        public Int32 CallCenterID { get { return (Int32)GetValue(typeof(Int32), "CallCenterID"); } set { SetValue("CallCenterID", value); } }
        [DataMember]
        public String Number { get { return (String)GetValue(typeof(String), "Number"); } set { SetValue("Number", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CallNumberVO tmp = new CallNumberVO();
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
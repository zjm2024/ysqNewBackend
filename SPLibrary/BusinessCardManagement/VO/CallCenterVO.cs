using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CallCenterVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CallCenterVO));

        [DataMember]
        public Int32 CallCenterID { get { return (Int32)GetValue(typeof(Int32), "CallCenterID"); } set { SetValue("CallCenterID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public String InstanceId { get { return (String)GetValue(typeof(String), "InstanceId"); } set { SetValue("InstanceId", value); } }
        [DataMember]
        public String ConsoleUrl { get { return (String)GetValue(typeof(String), "ConsoleUrl"); } set { SetValue("ConsoleUrl", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CallCenterVO tmp = new CallCenterVO();
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
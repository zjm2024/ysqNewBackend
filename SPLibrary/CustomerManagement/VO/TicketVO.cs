using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class TicketVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TicketVO));

        [DataMember]
        public Int32 TicketID { get { return (Int32)GetValue(typeof(Int32), "TicketID"); } set { SetValue("TicketID", value); } }
        [DataMember]
        public String AppId { get { return (String)GetValue(typeof(String), "AppId"); } set { SetValue("AppId", value); } }
        [DataMember]
        public String CreateTime { get { return (String)GetValue(typeof(String), "CreateTime"); } set { SetValue("CreateTime", value); } }
        [DataMember]
        public String ComponentVerifyTicket { get { return (String)GetValue(typeof(String), "ComponentVerifyTicket"); } set { SetValue("ComponentVerifyTicket", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            TicketVO tmp = new TicketVO();
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
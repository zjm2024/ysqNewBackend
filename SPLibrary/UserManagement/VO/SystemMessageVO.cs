using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class SystemMessageVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(SystemMessageVO));
       
		[DataMember]
		 public Int32 SystemMessageId { get { return (Int32)GetValue(typeof(Int32),"SystemMessageId") ; } set {  SetValue("SystemMessageId",value); } } 		[DataMember]
		 public Int32 MessageTypeId { get { return (Int32)GetValue(typeof(Int32),"MessageTypeId") ; } set {  SetValue("MessageTypeId",value); } } 		[DataMember]
		public DateTime SendAt { get { return (DateTime)GetValue(typeof(DateTime),"SendAt") ; } set {  SetValue("SendAt",value); } }
				[DataMember]
		 public String Title { get { return (String)GetValue(typeof(String),"Title") ; } set {  SetValue("Title",value); } } 		[DataMember]
		 public String Message { get { return (String)GetValue(typeof(String),"Message") ; } set {  SetValue("Message",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            SystemMessageVO tmp = new SystemMessageVO();
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
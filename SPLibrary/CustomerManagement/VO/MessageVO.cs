using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class MessageVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(MessageVO));
       
		[DataMember]
		 public Int32 MessageId { get { return (Int32)GetValue(typeof(Int32),"MessageId") ; } set {  SetValue("MessageId",value); } } 		[DataMember]
		 public Int32 MessageTypeId { get { return (Int32)GetValue(typeof(Int32),"MessageTypeId") ; } set {  SetValue("MessageTypeId",value); } } 		[DataMember]
		 public Int32 SendTo { get { return (Int32)GetValue(typeof(Int32),"SendTo") ; } set {  SetValue("SendTo",value); } } 		[DataMember]
		public DateTime SendAt { get { return (DateTime)GetValue(typeof(DateTime),"SendAt") ; } set {  SetValue("SendAt",value); } }
				[DataMember]
		 public String Title { get { return (String)GetValue(typeof(String),"Title") ; } set {  SetValue("Title",value); } } 		[DataMember]
		 public String Message { get { return (String)GetValue(typeof(String),"Message") ; } set {  SetValue("Message",value); } } 		[DataMember]
		 public Int32 Status { get { return (Int32)GetValue(typeof(Int32),"Status") ; } set {  SetValue("Status",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            MessageVO tmp = new MessageVO();
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
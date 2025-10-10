using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class MessageTypeVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(MessageTypeVO));
       
		[DataMember]
		 public Int32 MessageTypeId { get { return (Int32)GetValue(typeof(Int32),"MessageTypeId") ; } set {  SetValue("MessageTypeId",value); } } 		[DataMember]
		 public String MessageTypeCode { get { return (String)GetValue(typeof(String),"MessageTypeCode") ; } set {  SetValue("MessageTypeCode",value); } } 		[DataMember]
		 public String MessageTypeName { get { return (String)GetValue(typeof(String),"MessageTypeName") ; } set {  SetValue("MessageTypeName",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            MessageTypeVO tmp = new MessageTypeVO();
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
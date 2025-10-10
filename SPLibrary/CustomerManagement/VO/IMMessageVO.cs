using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class IMMessageVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(IMMessageVO));
       
		[DataMember]
		 public Int32 IMMessageId { get { return (Int32)GetValue(typeof(Int32),"IMMessageId") ; } set {  SetValue("IMMessageId",value); } } 		[DataMember]
		 public Int32 MessageFrom { get { return (Int32)GetValue(typeof(Int32),"MessageFrom") ; } set {  SetValue("MessageFrom",value); } } 		[DataMember]
		 public Int32 MessageTo { get { return (Int32)GetValue(typeof(Int32),"MessageTo") ; } set {  SetValue("MessageTo",value); } } 		[DataMember]
		 public String IMTargetType { get { return (String)GetValue(typeof(String),"IMTargetType") ; } set {  SetValue("IMTargetType",value); } } 		[DataMember]
		 public String IMMessageType { get { return (String)GetValue(typeof(String),"IMMessageType") ; } set {  SetValue("IMMessageType",value); } } 		[DataMember]
		 public String Message { get { return (String)GetValue(typeof(String),"Message") ; } set {  SetValue("Message",value); } } 		[DataMember]
		 public String FileURL { get { return (String)GetValue(typeof(String),"FileURL") ; } set {  SetValue("FileURL",value); } } 		[DataMember]
		 public String FileName { get { return (String)GetValue(typeof(String),"FileName") ; } set {  SetValue("FileName",value); } } 		[DataMember]
		 public Decimal FileSize { get { return (Decimal)GetValue(typeof(Decimal),"FileSize") ; } set {  SetValue("FileSize",value); } } 		[DataMember]
		public DateTime SendAt { get { return (DateTime)GetValue(typeof(DateTime),"SendAt") ; } set {  SetValue("SendAt",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            IMMessageVO tmp = new IMMessageVO();
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
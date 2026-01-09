using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AliasVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AliasVO));
       
		[DataMember]
		 public Int32 AliasId { get { return (Int32)GetValue(typeof(Int32),"AliasId") ; } set {  SetValue("AliasId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public String Alias { get { return (String)GetValue(typeof(String),"Alias") ; } set {  SetValue("Alias",value); } } 		[DataMember]
		 public String DeviceType { get { return (String)GetValue(typeof(String),"DeviceType") ; } set {  SetValue("DeviceType",value); } } 		[DataMember]
		public Boolean IsAllowReceive { get { return (Boolean)GetValue(typeof(Boolean),"IsAllowReceive") ; } set {  SetValue("IsAllowReceive",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            AliasVO tmp = new AliasVO();
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
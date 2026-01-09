using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class IMTokenVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(IMTokenVO));
       
		[DataMember]
		 public Int32 IMTokenId { get { return (Int32)GetValue(typeof(Int32),"IMTokenId") ; } set {  SetValue("IMTokenId",value); } } 		[DataMember]
		 public String IMToken { get { return (String)GetValue(typeof(String),"IMToken") ; } set {  SetValue("IMToken",value); } } 		[DataMember]
		 public Int32 Expire { get { return (Int32)GetValue(typeof(Int32),"Expire") ; } set {  SetValue("Expire",value); } } 		[DataMember]
		public DateTime SetDate { get { return (DateTime)GetValue(typeof(DateTime),"SetDate") ; } set {  SetValue("SetDate",value); } }
				[DataMember]
		 public String Application { get { return (String)GetValue(typeof(String),"Application") ; } set {  SetValue("Application",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            IMTokenVO tmp = new IMTokenVO();
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
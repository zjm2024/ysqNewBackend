using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CustomerIMVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerIMVO));
       
		[DataMember]
		 public Int32 CustomerIMId { get { return (Int32)GetValue(typeof(Int32),"CustomerIMId") ; } set {  SetValue("CustomerIMId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public String IMId { get { return (String)GetValue(typeof(String),"IMId") ; } set {  SetValue("IMId",value); } } 		[DataMember]
		 public String IMPWD { get { return (String)GetValue(typeof(String),"IMPWD") ; } set {  SetValue("IMPWD",value); } } 		[DataMember]
		 public String NickName { get { return (String)GetValue(typeof(String),"NickName") ; } set {  SetValue("NickName",value); } } 		[DataMember]
		 public Int32 Status { get { return (Int32)GetValue(typeof(Int32),"Status") ; } set {  SetValue("Status",value); } } 		[DataMember]
		 public String Sign { get { return (String)GetValue(typeof(String),"Sign") ; } set {  SetValue("Sign",value); } } 		[DataMember]
		 public String HeaderLogo { get { return (String)GetValue(typeof(String),"HeaderLogo") ; } set {  SetValue("HeaderLogo",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            CustomerIMVO tmp = new CustomerIMVO();
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
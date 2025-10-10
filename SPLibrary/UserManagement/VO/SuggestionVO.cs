using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class SuggestionVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(SuggestionVO));
       
		[DataMember]
		 public Int32 SuggestionId { get { return (Int32)GetValue(typeof(Int32),"SuggestionId") ; } set {  SetValue("SuggestionId",value); } } 		[DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }[DataMember]
        public String ContactPerson { get { return (String)GetValue(typeof(String),"ContactPerson") ; } set {  SetValue("ContactPerson",value); } } 		[DataMember]
		 public String ContactPhone { get { return (String)GetValue(typeof(String),"ContactPhone") ; } set {  SetValue("ContactPhone",value); } } 		[DataMember]
		 public String Title { get { return (String)GetValue(typeof(String),"Title") ; } set {  SetValue("Title",value); } } 		[DataMember]
		 public String Description { get { return (String)GetValue(typeof(String),"Description") ; } set {  SetValue("Description",value); } } 		[DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            SuggestionVO tmp = new SuggestionVO();
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
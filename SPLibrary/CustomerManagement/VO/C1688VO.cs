using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class C1688VO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(C1688VO));
       
		[DataMember]
		 public String CorporateName { get { return (String)GetValue(typeof(String), "CorporateName") ; } set {  SetValue("CorporateName", value); } }
        [DataMember]
        public String Contacts { get { return (String)GetValue(typeof(String), "Contacts"); } set { SetValue("Contacts", value); } }
        [DataMember]
        public String MobilePhone { get { return (String)GetValue(typeof(String), "MobilePhone"); } set { SetValue("MobilePhone", value); } }
        [DataMember]
        public String Telephone { get { return (String)GetValue(typeof(String), "Telephone"); } set { SetValue("Telephone", value); } }
        [DataMember]
        public String Fax { get { return (String)GetValue(typeof(String), "Fax"); } set { SetValue("Fax", value); } }
        [DataMember]
        public String Province { get { return (String)GetValue(typeof(String), "Province"); } set { SetValue("Province", value); } }
        [DataMember]
        public String City { get { return (String)GetValue(typeof(String), "City"); } set { SetValue("City", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String Business { get { return (String)GetValue(typeof(String), "Business"); } set { SetValue("Business", value); } }
        [DataMember]
        public String Products { get { return (String)GetValue(typeof(String), "Products"); } set { SetValue("Products", value); } }
        [DataMember]
        public String Model { get { return (String)GetValue(typeof(String), "Model"); } set { SetValue("Model", value); } }
        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }
        [DataMember]
        public String Pic { get { return (String)GetValue(typeof(String), "Pic"); } set { SetValue("Pic", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            C1688VO tmp = new C1688VO();
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
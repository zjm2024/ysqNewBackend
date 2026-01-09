using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardAchievemenViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardAchievemenViewVO));

        [DataMember]
        public String MONTH { get { return (String)GetValue(typeof(String), "MONTH"); } set { SetValue("MONTH", value); } }
        [DataMember]
        public Int32 count { get { return (Int32)GetValue(typeof(Int32), "count"); } set { SetValue("count", value); } }
        [DataMember]
		public Int32 originCustomerId { get { return (Int32)GetValue(typeof(Int32), "originCustomerId") ; } set {  SetValue("originCustomerId", value); } }
        [DataMember]
        public String CustomerAccount { get { return (String)GetValue(typeof(String), "CustomerAccount"); } set { SetValue("CustomerAccount", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public String CorporateName { get { return (String)GetValue(typeof(String), "CorporateName"); } set { SetValue("CorporateName", value); } }
        [DataMember]
        public String Tel { get { return (String)GetValue(typeof(String), "Tel"); } set { SetValue("Tel", value); } }
        [DataMember]
        public String Email { get { return (String)GetValue(typeof(String), "Email"); } set { SetValue("Email", value); } }
        [DataMember]
        public String WeChat { get { return (String)GetValue(typeof(String), "WeChat"); } set { SetValue("WeChat", value); } }


        [DataMember]
        public Int32 FirstlevelUsers { get { return (Int32)GetValue(typeof(Int32), "FirstlevelUsers"); } set { SetValue("FirstlevelUsers", value); } }
        [DataMember]
        public Int32 QualifiedFirstlevelUsersof { get { return (Int32)GetValue(typeof(Int32), "QualifiedFirstlevelUsersof"); } set { SetValue("QualifiedFirstlevelUsersof", value); } }
        [DataMember]
        public Int32 SecondaryUsers { get { return (Int32)GetValue(typeof(Int32), "SecondaryUsers"); } set { SetValue("SecondaryUsers", value); } }
        [DataMember]
        public Int32 QualifiedSecondaryUsers { get { return (Int32)GetValue(typeof(Int32), "QualifiedSecondaryUsers"); } set { SetValue("QualifiedSecondaryUsers", value); } }
        [DataMember]
        public Double Reward { get { return (Double)GetValue(typeof(Double), "Reward"); } set { SetValue("Reward", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardAchievemenViewVO tmp = new CardAchievemenViewVO();
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
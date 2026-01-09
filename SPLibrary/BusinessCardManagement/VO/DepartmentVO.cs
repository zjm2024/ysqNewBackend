using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class DepartmentVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(DepartmentVO));
       
		[DataMember]
		 public Int32 DepartmentID { get { return (Int32)GetValue(typeof(Int32), "DepartmentID") ; } set {  SetValue("DepartmentID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String DepartmentName { get { return (String)GetValue(typeof(String), "DepartmentName"); } set { SetValue("DepartmentName", value); } }
        [DataMember]
        public Int32 DirectorPersonalID { get { return (Int32)GetValue(typeof(Int32), "DirectorPersonalID"); } set { SetValue("DirectorPersonalID", value); } }
        [DataMember]
        public Int32 Order_info { get { return (Int32)GetValue(typeof(Int32), "Order_info"); } set { SetValue("Order_info", value); } }

        

        #region ICloneable Member
        public override object Clone()
        {
            DepartmentVO tmp = new DepartmentVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CompanyLocationViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CompanyLocationViewVO));

        [DataMember]
        public String Location { get { return (String)GetValue(typeof(String), "Location"); } set { SetValue("Location", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CompanyLocationViewVO tmp = new CompanyLocationViewVO();
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
    public class CompanyLocation
    {
        public string Province { get; set; }
        public List<string> City { get; set; }
    }
}
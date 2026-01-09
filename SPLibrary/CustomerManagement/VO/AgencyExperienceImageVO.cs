using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencyExperienceImageVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyExperienceImageVO));
       
		[DataMember]
		 public Int32 AgencyExperienceImageId { get { return (Int32)GetValue(typeof(Int32),"AgencyExperienceImageId") ; } set {  SetValue("AgencyExperienceImageId",value); } } 		[DataMember]
		 public Int32 AgencyExperienceId { get { return (Int32)GetValue(typeof(Int32),"AgencyExperienceId") ; } set {  SetValue("AgencyExperienceId",value); } } 		[DataMember]
		 public String ImgPath { get { return (String)GetValue(typeof(String),"ImgPath") ; } set {  SetValue("ImgPath",value); } }
        [DataMember]
        public Int32 TypeId { get { return (Int32)GetValue(typeof(Int32), "TypeId"); } set { SetValue("TypeId", value); } }
        [DataMember]
        public String FileName { get { return (String)GetValue(typeof(String), "FileName"); } set { SetValue("FileName", value); } }
        [DataMember]
        public DateTime CreatedDate { get { return (DateTime)GetValue(typeof(DateTime), "CreatedDate"); } set { SetValue("CreatedDate", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AgencyExperienceImageVO tmp = new AgencyExperienceImageVO();
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
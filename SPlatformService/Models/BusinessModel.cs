using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class BusinessModel
    {
        [DataMember]
        public BusinessVO Business { get; set; }
        [DataMember]
        public List<BusinessCategoryVO> BusinessCategory { get; set; }
        [DataMember]
        public List<TargetCategoryVO> TargetCategory { get; set; }
        [DataMember]
        public List<TargetCityVO> TargetCity { get; set; }
        [DataMember]
        public List<BusinessIdcardVO> BusinessIdCard { get; set; }
    }
}
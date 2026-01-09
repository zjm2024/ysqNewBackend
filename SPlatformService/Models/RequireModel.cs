using SPLibrary.RequireManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class RequireModel
    {
        [DataMember]
        public RequirementVO Requirement { get; set; }
        [DataMember]
        public List<RequirementTargetCategoryVO> RequireCategory { get; set; }
        [DataMember]
        public List<RequirementTargetCityVO> RequireCity { get; set; }
        [DataMember]
        public List<RequirementFileVO> RequireFile { get; set; }
        [DataMember]
        public List<RequirementTargetClientVO> RequireClient { get; set; }
    }
}
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class AgencyModel
    {
        [DataMember]
        public AgencyVO Agency { get; set; }
        [DataMember]
        public List<AgencyCategoryVO> AgencyCategory { get; set; }
        [DataMember]
        public List<AgencyCityVO> AgencyCity { get; set; }
        [DataMember]
        public List<AgencyIdCardVO> AgencyIdCard { get; set; }
        [DataMember]
        public List<AgencyTechnicalVO> AgencyTechnical { get; set; }
        [DataMember]
        public List<AgencySuperClientVO> AgencySuperClient { get; set; }
        [DataMember]
        public List<AgencySolutionVO> AgencySolution { get; set; }
    }
}
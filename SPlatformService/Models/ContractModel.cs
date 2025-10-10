using SPLibrary.ProjectManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class ContractModel
    {
        [DataMember]
        public ContractVO Contract { get; set; }
        [DataMember]
        public List<ContractStepsVO> ContractSteps { get; set; }
        [DataMember]
        public List<ContractFileVO> ContractFile { get; set; }
    }
}
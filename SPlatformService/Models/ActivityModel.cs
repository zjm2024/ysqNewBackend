using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class ActivityModel
    {
        [DataMember]
        public ActivityVO ActivityInfo { get; set; }

        [DataMember]
        public List<ActivityCountVO> ActivityCountInfo { get; set; }

        [DataMember]
        public List<ActivityTicketVO> ActivityTicketInfo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using SPLibrary.UserManagement.VO;
using SPLibrary.WebConfigInfo;

namespace SPlatformService.Models
{
    [DataContract]
    public class ConditionModel
    {
        [DataMember]
        public Filter Filter { get; set; }
        [DataMember]
        public Paging PageInfo { get; set; }
    }
}
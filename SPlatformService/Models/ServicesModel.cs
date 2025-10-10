using SPLibrary.RequireManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class ServicesModel
    {
        [DataMember]
        public ServicesVO Services { get; set; }
        [DataMember]
        public List<ServicesCategoryVO> ServicesCategory { get; set; }
    }
}
using SPLibrary.CustomerManagement.VO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class CustomerLoginModel
    {
        [DataMember]
        public CustomerViewVO Customer { get; set; }
        [DataMember]
        public string Token { get; set; }
        [DataMember]
        public PersonalVO Personal { get; set; }
    }
}
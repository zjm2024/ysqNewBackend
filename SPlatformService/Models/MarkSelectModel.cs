using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class MarkSelectModel
    {
        [DataMember]
        public Filter FilterInfo { get; set; }
        [DataMember]
        public Paging PageInfo { get; set; }
    }
}
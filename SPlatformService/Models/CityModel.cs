using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class CityModel
    {
        [DataMember]
        public ProvinceVO Province { get; set; }
        [DataMember]
        public List<CityVO> City { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.Models
{
    [DataContract]
    public class CategoryModel
    {
        [DataMember]
        public CategoryVO Category { get; set; }
        [DataMember]
        public List<CategoryVO> ChildCategory { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.VO
{
    [Serializable]
    public class UserProfile
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public int CompanyId { get; set; }
        public int DeaprtmentId { get; set; }
        public string Phone { get; set; }
        public int AppType { get; set; }
    }
}

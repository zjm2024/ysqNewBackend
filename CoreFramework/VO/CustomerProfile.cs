using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.VO
{
    [Serializable]
    public class CustomerProfile:UserProfile
    {
        public int CustomerId { get; set; }
        public string CustomerAccount { get; set; }
        public string CustomerName { get; set; }
        //public int BusinessId { get; set; }
        //public int UserId { get; set; }
    }
}

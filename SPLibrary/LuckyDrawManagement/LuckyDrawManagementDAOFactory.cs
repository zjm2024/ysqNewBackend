using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFramework.VO;

namespace SPLibrary.LuckyDrawManagement.DAO
{
    public partial class LuckyDrawManagementDAOFactory
    {
        public static ILuckyDrawDAO LuckyDrawDAO(UserProfile userProfile)
        {
            return new LuckyDrawDAO(userProfile);
        }
        public static IPrizeDAO PrizeDAO(UserProfile userProfile)
        {
            return new PrizeDAO(userProfile);
        }
    }
}

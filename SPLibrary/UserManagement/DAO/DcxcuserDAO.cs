using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.UserManagement.DAO
{
    public partial class DcxcuserDAO : CommonDAO, IUserDAO
    {
        public DcxcuserDAO(DcxcUser DcxcUser)
        {
            base._tableName = "t_dcxc_user";
            base._pkId = "UserId";
            base._voType = typeof(DcxcuserVo);
            base.CurrentDcxcUser = DcxcUser;
        }
    }
}

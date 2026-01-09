using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.UserManagement.VO
{
    [DataContract]
    [Serializable]
    public class DcxcuserVo : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(DcxcuserVo));

        [DataMember]
        public Int32 UserId { get { return (Int32)GetValue(typeof(Int32), "UserId"); } set { SetValue("UserId", value); } }
        [DataMember]
        public String UserName { get { return (String)GetValue(typeof(String), "UserName"); } set { SetValue("UserName", value); } }
        [DataMember]
        public String Account { get { return (String)GetValue(typeof(String), "Account"); } set { SetValue("Account", value); } }
        [DataMember]
        public String Password { get { return (String)GetValue(typeof(String), "Password"); } set { SetValue("Password", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public Int32 IsAdmin { get { return (Int32)GetValue(typeof(Int32), "IsAdmin"); } set { SetValue("IsAdmin", value); } }
        [DataMember]
        public String Unit { get { return (String)GetValue(typeof(String), "Unit"); } set { SetValue("Unit", value); } }

        [DataMember]
        public String UnitaddRess { get { return (String)GetValue(typeof(String), "UnitaddRess"); } set { SetValue("UnitaddRess", value); } }

        [DataMember]
        public String UnitDirector { get { return (String)GetValue(typeof(String), "UnitDirector"); } set { SetValue("UnitDirector", value); } }
        [DataMember]
        public String DirectorPhone { get { return (String)GetValue(typeof(String), "DirectorPhone"); } set { SetValue("DirectorPhone", value); } }
        [DataMember]
        public DateTime CreateDate { get { return (DateTime)GetValue(typeof(DateTime), "CreateDate"); } set { SetValue("CreateDate", value); } }
        [DataMember]
        public DateTime UpDate { get { return (DateTime)GetValue(typeof(DateTime), "UpDate"); } set { SetValue("UpDate", value); } }
        [DataMember]
        public Int32 IsEnable { get { return (Int32)GetValue(typeof(Int32), "IsEnable"); } set { SetValue("IsEnable", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            DcxcuserVo tmp = new DcxcuserVo();
            tmp.changeData = new Dictionary<string, object>(this.changeData);
            tmp.originData = new Dictionary<string, object>(this.originData);
            return tmp;
        }
        #endregion

        #region ICommonVO Member
        List<string> ICommonVO.PropertyList
        {
            get { return _propertyList; }
        }
        #endregion

    }
}

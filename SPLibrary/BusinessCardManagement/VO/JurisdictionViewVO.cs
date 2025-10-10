using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class JurisdictionViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(JurisdictionViewVO));

        /// <summary>
        /// 主管理员
        /// </summary>
        [DataMember]
        public bool Admin { get; set; }
        /// <summary>
        /// 官网修改权限
        /// </summary>
        [DataMember]
        public bool Web { get; set; }
        /// <summary>
        /// 产品修改权限
        /// </summary>
        [DataMember]
        public bool Product { get; set; }
        /// <summary>
        /// 查看转移所有人的客户权限，默认只能查看转移自己与下属的客户
        /// </summary>
        [DataMember]
        public bool Clients { get; set; }
        /// <summary>
        /// 查看所有人的业绩与合同权限，默认只能查看自己与下属的业绩
        /// </summary>
        [DataMember]
        public bool Performance { get; set; }
        /// <summary>
        /// 员工管理权限
        /// </summary>
        [DataMember]
        public bool Personnel { get; set; }

        /// <summary>
        /// 购买订单权限
        /// </summary>
        [DataMember]
        public bool Order { get; set; }

        /// <summary>
        /// 使用云呼的权限
        /// </summary>
        [DataMember]
        public bool CloudCall { get; set; }

        /// <summary>
        /// 邀请代理商的权限
        /// </summary>
        [DataMember]
        public bool AddAgent { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            JurisdictionViewVO tmp = new JurisdictionViewVO();
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
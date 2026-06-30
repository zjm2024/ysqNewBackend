using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
    /// <summary>
    /// 广告曝光日志实体
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class AdPopupLogVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AdPopupLogVO));

        /// <summary>主键ID</summary>
        [DataMember]
        public Int64 AdPopupLogId { get { return (Int64)GetValue(typeof(Int64), "AdPopupLogId"); } set { SetValue("AdPopupLogId", value); } }

        /// <summary>关联广告配置ID</summary>
        [DataMember]
        public Int64 AdPopupConfigId { get { return (Int64)GetValue(typeof(Int64), "AdPopupConfigId"); } set { SetValue("AdPopupConfigId", value); } }

        /// <summary>小程序用户标识（openid/unionid）</summary>
        [DataMember]
        public String UserId { get { return (String)GetValue(typeof(String), "UserId"); } set { SetValue("UserId", value); } }

        /// <summary>曝光时所在的页面路径</summary>
        [DataMember]
        public String PagePath { get { return (String)GetValue(typeof(String), "PagePath"); } set { SetValue("PagePath", value); } }

        /// <summary>动作：1-曝光 2-点击 3-关闭</summary>
        [DataMember]
        public Int32 Action { get { return (Int32)GetValue(typeof(Int32), "Action"); } set { SetValue("Action", value); } }

        /// <summary>会话ID</summary>
        [DataMember]
        public String SessionId { get { return (String)GetValue(typeof(String), "SessionId"); } set { SetValue("SessionId", value); } }

        /// <summary>用户IP</summary>
        [DataMember]
        public String Ip { get { return (String)GetValue(typeof(String), "Ip"); } set { SetValue("Ip", value); } }

        /// <summary>记录时间</summary>
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AdPopupLogVO tmp = new AdPopupLogVO();
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
using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
    /// <summary>
    /// 广告弹框配置实体
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class AdPopupConfigVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AdPopupConfigVO));

        /// <summary>主键ID</summary>
        [DataMember]
        public Int32 AdPopupConfigId { get { return (Int32)GetValue(typeof(Int32), "AdPopupConfigId"); } set { SetValue("AdPopupConfigId", value); } }

        /// <summary>广告名称</summary>
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }

        /// <summary>广告位唯一标识（如 home_banner）</summary>
        [DataMember]
        public String SlotId { get { return (String)GetValue(typeof(String), "SlotId"); } set { SetValue("SlotId", value); } }

        /// <summary>广告图片URL</summary>
        [DataMember]
        public String ImageUrl { get { return (String)GetValue(typeof(String), "ImageUrl"); } set { SetValue("ImageUrl", value); } }

        /// <summary>跳转类型：1-小程序页面 2-外部H5 3-其他小程序 4-不跳转</summary>
        [DataMember]
        public Int32 LinkType { get { return (Int32)GetValue(typeof(Int32), "LinkType"); } set { SetValue("LinkType", value); } }

        /// <summary>跳转地址</summary>
        [DataMember]
        public String LinkUrl { get { return (String)GetValue(typeof(String), "LinkUrl"); } set { SetValue("LinkUrl", value); } }

        /// <summary>页面匹配方式：0-所有页面 1-精确匹配 2-前缀匹配</summary>
        [DataMember]
        public Int32 PageMatchMode { get { return (Int32)GetValue(typeof(Int32), "PageMatchMode"); } set { SetValue("PageMatchMode", value); } }

        /// <summary>匹配的页面路径列表（JSON数组）</summary>
        [DataMember]
        public String PagePaths { get { return (String)GetValue(typeof(String), "PagePaths"); } set { SetValue("PagePaths", value); } }

        /// <summary>展示位置：center/bottom/top</summary>
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }

        /// <summary>是否显示关闭按钮：0-否 1-是</summary>
        [DataMember]
        public Int32 ShowCloseBtn { get { return (Int32)GetValue(typeof(Int32), "ShowCloseBtn"); } set { SetValue("ShowCloseBtn", value); } }

        /// <summary>点击遮罩是否关闭：0-否 1-是</summary>
        [DataMember]
        public Int32 CloseOnMask { get { return (Int32)GetValue(typeof(Int32), "CloseOnMask"); } set { SetValue("CloseOnMask", value); } }

        /// <summary>自动关闭秒数（0表示不自动关闭）</summary>
        [DataMember]
        public Int32 AutoCloseSeconds { get { return (Int32)GetValue(typeof(Int32), "AutoCloseSeconds"); } set { SetValue("AutoCloseSeconds", value); } }

        /// <summary>投放开始时间</summary>
        [DataMember]
        public DateTime? StartTime { get { return (DateTime?)GetValue(typeof(DateTime?), "StartTime"); } set { SetValue("StartTime", value); } }

        /// <summary>投放结束时间</summary>
        [DataMember]
        public DateTime? EndTime { get { return (DateTime?)GetValue(typeof(DateTime?), "EndTime"); } set { SetValue("EndTime", value); } }

        /// <summary>状态：0-删除 1-启用 2-禁用</summary>
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }

        /// <summary>每个用户每天最大展示次数（0表示不限）</summary>
        [DataMember]
        public Int32 FrequencyLimit { get { return (Int32)GetValue(typeof(Int32), "FrequencyLimit"); } set { SetValue("FrequencyLimit", value); } }

        /// <summary>优先级（越大越优先）</summary>
        [DataMember]
        public Int32 Priority { get { return (Int32)GetValue(typeof(Int32), "Priority"); } set { SetValue("Priority", value); } }

        /// <summary>扩展字段（JSON）</summary>
        [DataMember]
        public String Extra { get { return (String)GetValue(typeof(String), "Extra"); } set { SetValue("Extra", value); } }

        /// <summary>创建时间</summary>
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        /// <summary>更新时间</summary>
        [DataMember]
        public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime), "UpdatedAt"); } set { SetValue("UpdatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AdPopupConfigVO tmp = new AdPopupConfigVO();
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
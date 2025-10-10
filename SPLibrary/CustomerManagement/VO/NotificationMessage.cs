using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.CustomerManagement.VO
{
    /// <summary>
    /// 小程序首页通知消息
    /// </summary>
    public class NotificationMessage
    {
        public List<ContractMessage> contract{ get; set;}
        public List<ProjectMessage> project { get; set; }
        public List<NoticeMessage> notice { get; set; }
    }
    /// <summary>
    /// 合同
    /// </summary>
    public class ContractMessage
    {
        /// <summary>
        /// 合同ID
        /// </summary>
        public int contractid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 酬金
        /// </summary>
        public decimal commission { get; set; }
        /// <summary>
        /// 销售名称
        /// </summary>
        public string agencyName { get; set; }
        /// <summary>
        /// 销售状态
        /// </summary>
        public int agencyStatus { get; set; }
        /// <summary>
        /// 雇主名称
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 雇主状态
        /// </summary>
        public int businessStatus { get; set; }
        /// <summary>
        /// 当前身份时雇主：1还是销售：2
        /// </summary>
        public int identity { get; set; }
    }
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectMessage
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public int projectid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 酬金
        /// </summary>
        public decimal commission { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 最新工作进度或付款历史
        /// </summary>
        public string progress { get; set; }
        /// <summary>
        /// 销售名称
        /// </summary>
        public string agencyName { get; set; }
        /// <summary>
        /// 雇主名称
        /// </summary>
        public string businessname { get; set; }
        /// <summary>
        /// 申请信息：阶段付款申请/更改酬金申请/关闭项目申请/申请完工
        /// </summary>
        public string applymessage { get; set; }
        /// <summary>
        /// 当前身份时雇主：1还是销售：2
        /// </summary>
        public int identity { get; set; }
    }
    /// <summary>
    /// 通知类消息
    /// </summary>
    public class NoticeMessage
    {
        /// <summary>
        /// 跳转ID
        /// </summary>
        public int RID { get; set; }
        /// <summary>
        /// 识别ID
        /// </summary>
        public int AID { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string image { get; set;}
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 类型：任务(Requirement)/商机需求(Demand)/面试邀请(Tender)/钱包(Wallet)
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 类型名称：任务/商机需求/面试邀请/钱包
        /// </summary>
        public string typename { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal balance { get; set; }
    }
}

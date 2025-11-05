using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.VO
{
    /// <summary>
    /// 微信转账返回结果
    /// </summary>
    public class WeChatTransferResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        public string OutBillNo { get; set; }

        /// <summary>
        /// 微信转账单号
        /// </summary>
        public string TransferBillNo { get; set; }

        /// <summary>
        /// 转账状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 小程序确认收款所需信息
        /// </summary>
        public string PackageInfo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 错误详情
        /// </summary>
        public object ErrorDetail { get; set; }

        /// <summary>
        /// 成功返回
        /// </summary>
        public static WeChatTransferResult SuccessResult(string outBillNo, string transferBillNo, string state, string packageInfo = null, string message = null)
        {
            return new WeChatTransferResult
            {
                Success = true,
                Code = "SUCCESS",
                Message = message ?? "转账申请提交成功", // 默认消息
                OutBillNo = outBillNo,
                TransferBillNo = transferBillNo,
                State = state,
                PackageInfo = packageInfo,
                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        /// <summary>
        /// 失败返回
        /// </summary>
        public static WeChatTransferResult FailResult(string message, string code = "FAIL", object errorDetail = null)
        {
            return new WeChatTransferResult
            {
                Success = false,
                Code = code,
                Message = message,
                ErrorDetail = errorDetail,
                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        /// <summary>
        /// 设置消息并返回当前实例（链式调用）
        /// </summary>
        public WeChatTransferResult WithMessage(string message)
        {
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 设置错误详情并返回当前实例（链式调用）
        /// </summary>
        public WeChatTransferResult WithErrorDetail(object errorDetail)
        {
            this.ErrorDetail = errorDetail;
            return this;
        }
    }
}

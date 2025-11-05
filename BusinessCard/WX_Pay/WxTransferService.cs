using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.WX_Pay
{
    public class WxTransferService
    {
        /// <summary>
        /// 查询微信商家转账订单状态
        /// </summary>
        /// <param name="transferBillNo">微信转账单号</param>
        /// <param name="outBillNo">商户单号</param>
        /// <param name="appType">应用类型</param>
        /// <returns>微信转账查询结果</returns>
        public WeChatTransferResult QueryWeChatTransferStatus(string transferBillNo = null, string outBillNo = null, int appType = 0)
        {
            LogBO _log = new LogBO(typeof(WxTransferService));

            // 参数校验
            if (string.IsNullOrEmpty(transferBillNo) && string.IsNullOrEmpty(outBillNo))
            {
                return WeChatTransferResult.FailResult("微信转账单号和商户单号不能同时为空");
            }

            try
            {
                AppVO AppVO = AppBO.GetApp(appType);

                string mchid = AppVO.MCHID;
                string mchCertSerialNo = AppVO.MCH_CERT_SERIAL_NO;
                string certPath = AppVO.SSLCERT_PATH;
                string certPassword = AppVO.SSLCERT_PASSWORD;

                // 验证必要参数
                if (string.IsNullOrEmpty(certPath) || string.IsNullOrEmpty(certPassword))
                {
                    _log.Error("微信支付证书路径或密码未配置");
                    return WeChatTransferResult.FailResult("微信支付证书配置不完整");
                }

                // 构建查询URL - 使用V3 API
                string endpoint = string.IsNullOrEmpty(transferBillNo)
                    ? $"/v3/fund-app/mch-transfer/transfer-bills/out-bill-no/{outBillNo}"
                    : $"/v3/fund-app/mch-transfer/transfer-bills/transfer-bill-no/{transferBillNo}";

                string url = "https://api.mch.weixin.qq.com" + endpoint;

                // 发送查询请求
                string responseJson = WeChatPayV3Get(url, mchid, mchCertSerialNo, certPath, certPassword);

                _log.Info($"微信转账查询API返回: {responseJson}");

                // 解析返回结果
                return ParseQueryResponse(responseJson);
            }
            catch (Exception ex)
            {
                string strErrorMsg = $"微信转账查询异常 - Message: {ex.Message}, Stack: {ex.StackTrace}";
                _log.Error(strErrorMsg);
                return WeChatTransferResult.FailResult($"查询系统异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 微信支付V3 API GET请求
        /// </summary>
        private string WeChatPayV3Get(string url, string mchId, string certSerialNo, string certPath, string certPassword)
        {
            try
            {
                string nonce = Guid.NewGuid().ToString("N");
                string timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                string method = "GET";
                string body = ""; // GET请求没有请求体

                // 构造签名原文
                string message = $"{method}\n{url}\n{timestamp}\n{nonce}\n{body}\n";

                // 生成签名
                string signature = GenerateSignatureWithCertificate(message, certPath, certPassword);

                // 构造Authorization头
                string authorization = $"WECHATPAY2-SHA256-RSA2048 mchid=\"{mchId}\",nonce_str=\"{nonce}\",signature=\"{signature}\",timestamp=\"{timestamp}\",serial_no=\"{certSerialNo}\"";

                // 设置TLS 1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("Authorization", authorization);
                    client.Headers.Add("User-Agent", "Mozilla/4.0");
                    client.Headers.Add("Accept", "application/json");

                    return client.DownloadString(url);
                }
            }
            catch (WebException webEx)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));

                if (webEx.Response != null)
                {
                    using (Stream stream = webEx.Response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string errorResponse = reader.ReadToEnd();
                        _log.Error($"查询API请求Web异常: {webEx.Status}, 响应: {errorResponse}");

                        // 如果是404错误，订单不存在
                        if ((webEx.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new Exception("订单不存在，请检查单号是否正确");
                        }
                    }
                }
                throw new Exception($"查询请求失败: {webEx.Message}", webEx);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                _log.Error($"查询API请求异常: {ex.Message}");
                throw;
            }
        }

        ///// <summary>
        ///// 使用证书生成签名
        ///// </summary>
        private string GenerateSignatureWithCertificate(string message, string certPath, string certPassword)
        {
            try
            {
                // 1. 将证书文件作为字节数组读取
                byte[] certData = File.ReadAllBytes(certPath);

                // 2. 从字节数组和密码加载证书
                X509Certificate2 cert = new X509Certificate2(certData, certPassword,
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

                // 3. 获取支持新算法的RSA实例
                using (RSA rsa = cert.GetRSAPrivateKey())
                {
                    if (rsa == null)
                    {
                        throw new Exception("无法从证书中获取支持算法的RSA私钥。");
                    }

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    // 4. 使用支持SHA256的RSA实例进行签名
                    byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    return Convert.ToBase64String(signature);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                _log.Error($"使用新方法生成证书签名失败: {ex.Message}");
                throw new Exception($"使用新方法生成证书签名失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解析查询响应
        /// </summary>
        private WeChatTransferResult ParseQueryResponse(string responseJson)
        {
            if (string.IsNullOrEmpty(responseJson))
            {
                return WeChatTransferResult.FailResult("微信支付返回空响应");
            }

            try
            {
                dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);

                // 检查是否存在错误码
                if (resultObj.code != null)
                {
                    string errorCode = resultObj.code.ToString();
                    string errorMessage = resultObj.message?.ToString();
                    return WeChatTransferResult.FailResult(errorMessage, errorCode);
                }

                // 解析成功响应
                string state = resultObj.state?.ToString();
                string outBillNo = resultObj.out_bill_no?.ToString();
                string transferBillNo = resultObj.transfer_bill_no?.ToString();
                string createTime = resultObj.create_time?.ToString();
                string successTime = resultObj.success_time?.ToString();
                string failReason = resultObj.fail_reason?.ToString();

                var result = WeChatTransferResult.SuccessResult(outBillNo, transferBillNo, state);
                result.CreateTime = createTime;

                // 根据状态设置相应消息
                switch (state)
                {
                    case "SUCCESS":
                        result.Message = "转账成功";
                        break;
                    case "WAIT_USER_CONFIRM":
                        result.Message = "等待用户确认收款";
                        break;
                    case "PROCESSING":
                        result.Message = "转账处理中";
                        break;
                    case "FAILED":
                        result.Message = $"转账失败: {failReason}";
                        result.Success = false;
                        result.Code = "TRANSFER_FAILED";
                        break;
                    case "CANCELLED":
                        result.Message = "转账已取消";
                        result.Success = false;
                        result.Code = "CANCELLED";
                        break;
                    default:
                        result.Message = $"未知状态: {state}";
                        break;
                }

                return result;
            }
            catch (Exception jsonEx)
            {
                return WeChatTransferResult.FailResult($"解析查询响应JSON失败: {jsonEx.Message}");
            }
        }
    }
}

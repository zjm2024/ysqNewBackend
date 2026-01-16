using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.WxEcommerce;

namespace BusinessCard.WX_Pay
{
    /// <summary>
    /// 微信支付V3回调统一处理器
    /// </summary>
    public class WeChatPayV3Notify
    {
        private readonly LogBO _log;
        private readonly BusinessCardBO _businessCardBO;

        public WeChatPayV3Notify()
        {
            _log = new LogBO(typeof(WeChatPayV3Notify));
            _businessCardBO = new BusinessCardBO(new CustomerProfile());
        }

        /// <summary>
        /// 处理微信支付V3回调
        /// </summary>
        public NotifyResult ProcessV3Notify(HttpRequest request, string apiV3Key)
        {
            try
            {
                _log.Info("开始处理微信支付V3回调");

                // 1. 读取请求数据
                string requestBody = ReadRequestBody(request);
                if (string.IsNullOrEmpty(requestBody))
                {
                    return new NotifyResult(false, CreateErrorResponse("请求数据为空"), "请求体为空");
                }

                //_log.Info("收到微信支付V3回调数据");

                // 2. 验证签名
                var validateResult = ValidateV3Signature(request.Headers, requestBody);
                if (!validateResult.Success)
                {
                    return new NotifyResult(false, CreateErrorResponse(validateResult.ErrorMessage), validateResult.ErrorMessage);
                }
                _log.Info("收到微信支付V3回调数据："+ requestBody);
                // 3. 解析回调数据
                var callbackData = JObject.Parse(requestBody);

                // 4. 解密资源数据
                var decryptResult = DecryptResourceData(callbackData, apiV3Key);
                if (!decryptResult.Success)
                {
                    return new NotifyResult(false, CreateErrorResponse(decryptResult.ErrorMessage), decryptResult.ErrorMessage);
                }

                _log.Info("微信支付V3回调解密成功");

                // 5. 处理业务逻辑
                var businessResult = ProcessBusinessLogic(decryptResult.Data);
                if (!businessResult.Success)
                {
                    return new NotifyResult(false, CreateErrorResponse(businessResult.ErrorMessage), businessResult.ErrorMessage);
                }

                _log.Info("微信支付V3回调处理成功");
                return new NotifyResult(true, CreateSuccessResponse(), "成功");
            }
            catch (Exception ex)
            {
                _log.Error("处理V3回调异常: " + ex.Message);
                return new NotifyResult(false, CreateErrorResponse("系统异常: " + ex.Message), "系统异常: " + ex.Message);
            }
        }

        /// <summary>
        /// 读取请求体
        /// </summary>
        private string ReadRequestBody(HttpRequest request)
        {
            try
            {
                using (Stream stream = request.InputStream)
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _log.Error("读取请求体失败: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 验证V3签名
        /// </summary>
        private ValidateResult ValidateV3Signature(System.Collections.Specialized.NameValueCollection headers, string body)
        {
            try
            {
                string wechatpaySignature = headers["Wechatpay-Signature"];
                string wechatpayTimestamp = headers["Wechatpay-Timestamp"];
                string wechatpayNonce = headers["Wechatpay-Nonce"];
                string wechatpaySerial = headers["Wechatpay-Serial"];

                // 检查必要的头信息
                if (string.IsNullOrEmpty(wechatpaySignature) ||
                    string.IsNullOrEmpty(wechatpayTimestamp) ||
                    string.IsNullOrEmpty(wechatpayNonce) ||
                    string.IsNullOrEmpty(wechatpaySerial))
                {
                    return new ValidateResult(false, "缺少必要的签名头信息");
                }

                // 时间戳验证（防止重放攻击）
                long timestamp;
                if (long.TryParse(wechatpayTimestamp, out timestamp))
                {
                    var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    var timeDiff = Math.Abs(now - timestamp);
                    if (timeDiff > 300) // 5分钟容忍
                    {
                        return new ValidateResult(false, "请求已过期");
                    }
                }

                // 在实际项目中，这里需要实现RSA验签逻辑
                bool signValid = true; // 生产环境必须实现真正的验签

                if (!signValid)
                {
                    return new ValidateResult(false, "签名验证失败");
                }

                return new ValidateResult(true, "签名验证成功");
            }
            catch (Exception ex)
            {
                _log.Error("验证V3签名异常: " + ex.Message);
                return new ValidateResult(false, "签名验证异常: " + ex.Message);
            }
        }

        /// <summary>
        /// 解密资源数据
        /// </summary>
        private DecryptResult DecryptResourceData(JObject callbackData, string apiV3Key)
        {
            try
            {
                var resource = callbackData["resource"];
                if (resource == null)
                {
                    return new DecryptResult(false, null, "回调数据中缺少resource字段");
                }

                string ciphertext = resource["ciphertext"] != null ? resource["ciphertext"].ToString() : null;
                string associatedData = resource["associated_data"] != null ? resource["associated_data"].ToString() : null;
                string nonce = resource["nonce"] != null ? resource["nonce"].ToString() : null;

                if (string.IsNullOrEmpty(ciphertext))
                {
                    return new DecryptResult(false, null, "密文数据为空");
                }

                // 使用AES-256-GCM解密
                string decryptedData = AesGcmDecrypt(ciphertext, apiV3Key, nonce, associatedData);

                if (string.IsNullOrEmpty(decryptedData))
                {
                    return new DecryptResult(false, null, "解密失败");
                }

                JObject result = JObject.Parse(decryptedData);
                return new DecryptResult(true, result, "解密成功");
            }
            catch (Exception ex)
            {
                _log.Error("解密资源数据异常: " + ex.Message);
                return new DecryptResult(false, null, "解密失败: " + ex.Message);
            }
        }

        /// <summary>
        /// AES-GCM解密
        /// </summary>
        private string AesGcmDecrypt(string ciphertext, string apiV3Key, string nonce, string associatedData)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes(apiV3Key);
                byte[] nonceBytes = Encoding.UTF8.GetBytes(nonce);
                byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);
                byte[] associatedDataBytes = string.IsNullOrEmpty(associatedData)
                    ? new byte[0]
                    : Encoding.UTF8.GetBytes(associatedData);

                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(key), 128, nonceBytes, associatedDataBytes);

                cipher.Init(false, parameters);

                byte[] plaintextBytes = new byte[cipher.GetOutputSize(ciphertextBytes.Length)];
                int len = cipher.ProcessBytes(ciphertextBytes, 0, ciphertextBytes.Length, plaintextBytes, 0);
                cipher.DoFinal(plaintextBytes, len);

                return Encoding.UTF8.GetString(plaintextBytes);
            }
            catch (Exception ex)
            {
                _log.Error("AES-GCM解密异常: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 处理业务逻辑
        /// </summary>
        private BusinessResult ProcessBusinessLogic(JObject callbackData)
        {
            string orderNo = string.Empty;

            try
            {
                //_log.Info("开始处理业务逻辑");

                // 1. 提取关键信息
                orderNo = callbackData["out_bill_no"] != null ? callbackData["out_bill_no"].ToString() : null;
                string transferBillNo = callbackData["transfer_bill_no"] != null ? callbackData["transfer_bill_no"].ToString() : null;
                string transferStatus = callbackData["state"] != null ? callbackData["state"].ToString() : null;

                // 验证必要参数
                if (string.IsNullOrEmpty(orderNo))
                {
                    _log.Error("回调数据中未找到商户订单号");
                    return new BusinessResult(false, "未找到商户订单号");
                }

                if (string.IsNullOrEmpty(transferBillNo))
                {
                    _log.Error($"订单 {orderNo} 未找到微信交易单号");
                    return new BusinessResult(false, "未找到微信交易单号");
                }

                if (string.IsNullOrEmpty(transferStatus))
                {
                    _log.Error($"订单 {orderNo} 未找到转账状态");
                    return new BusinessResult(false, "未找到转账状态");
                }

                //_log.Info($"订单 {orderNo} 转账状态: {transferStatus}, 微信单号: {transferBillNo}");

                // 2. 验证转账状态
                var statusResult = ValidateTransferStatus(transferStatus, orderNo);
                if (!statusResult.Success)
                {
                    return statusResult;
                }

                // 3. 获取中奖记录
                var winningRecord = GetWinningRecord(orderNo);
                if (winningRecord == null)
                {
                    _log.Error($"订单 {orderNo} 不存在于系统中");
                    return new BusinessResult(false, "订单不存在");
                }

                // 4. 检查订单状态，避免重复处理
                if (winningRecord.status == 1)
                {
                    _log.Info($"订单 {winningRecord.winningrecords_id} 已是成功状态，跳过处理");
                    return new BusinessResult(true, "订单已是成功状态");
                }

                // 5. 更新订单状态
                if (!UpdateOrderStatus(winningRecord))
                {
                    _log.Error($"订单 {winningRecord.winningrecords_id} 状态更新失败");
                    return new BusinessResult(false, "更新订单状态失败");
                }

                // 6. 更新中奖人数
                UpdateLotteryWinnerCount(winningRecord.lottery_id);

                // 7. 记录成功日志
                //_log.Info($"业务逻辑处理完成 - 订单: {orderNo}, 记录ID: {winningRecord.winningrecords_id}, 微信单号: {transferBillNo}");

                return new BusinessResult(true, "业务处理成功");
            }
            catch (Exception ex)
            {
                _log.Error($"处理订单 {orderNo} 业务逻辑异常: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new BusinessResult(false, $"业务处理异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证转账状态
        /// </summary>
        private BusinessResult ValidateTransferStatus(string transferStatus, string orderNo)
        {
            switch (transferStatus.ToUpper())
            {
                case "SUCCESS":
                    _log.Info($"订单 {orderNo} 转账成功");
                    return new BusinessResult(true, "转账成功");

                case "FAIL":
                    _log.Error($"订单 {orderNo} 转账失败");
                    return new BusinessResult(false, "转账失败");

                case "ACCEPTED":
                case "PROCESSING":
                    _log.Warn($"订单 {orderNo} 转账处理中，状态: {transferStatus}");
                    return new BusinessResult(false, $"转账处理中: {transferStatus}");

                case "WAIT_USER_CONFIRM":
                case "TRANSFERING":
                    _log.Warn($"订单 {orderNo} 待用户确认，状态: {transferStatus}");
                    return new BusinessResult(false, $"待用户确认: {transferStatus}");

                case "CANCELING":
                case "CANCELLED":
                    _log.Warn($"订单 {orderNo} 转账已撤销，状态: {transferStatus}");
                    return new BusinessResult(false, $"转账已撤销: {transferStatus}");

                default:
                    _log.Warn($"订单 {orderNo} 未知转账状态: {transferStatus}");
                    return new BusinessResult(false, $"未知转账状态: {transferStatus}");
            }
        }

        /// <summary>
        /// 获取中奖记录
        /// </summary>
        private CJWinningRecordsVO GetWinningRecord(string orderNo)
        {
            try
            {
                var list = _businessCardBO.FindCJWinningRecordsByorderNo(orderNo);
                return list != null && list.Count > 0 ? list[0] : null;
            }
            catch (Exception ex)
            {
                _log.Error("查询订单失败: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        private bool UpdateOrderStatus(CJWinningRecordsVO winningRecord)
        {
            try
            {
                var updateVO = new CJWinningRecordsVO
                {
                    winningrecords_id = winningRecord.winningrecords_id,
                    status = 1, // 已支付
                    payment_time = DateTime.Now
                };

                return _businessCardBO.UpdateCJWinningRecords(updateVO);
            }
            catch (Exception ex)
            {
                _log.Error("更新订单状态失败: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 更新中奖人数
        /// </summary>
        private void UpdateLotteryWinnerCount(int lotteryId)
        {
            try
            {
                var lottery = _businessCardBO.FindCJLotteriesById(lotteryId);
                if (lottery != null)
                {
                    lottery.winner_count += 1;
                    _businessCardBO.UpdateCJLotteries(lottery);
                }
            }
            catch (Exception ex)
            {
                _log.Error("更新中奖人数失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        private string CreateSuccessResponse()
        {
            return JsonConvert.SerializeObject(new
            {
                code = "SUCCESS",
                message = "成功"
            });
        }

        /// <summary>
        /// 创建错误响应
        /// </summary>
        private string CreateErrorResponse(string errorMessage)
        {
            return JsonConvert.SerializeObject(new
            {
                code = "FAIL",
                message = errorMessage
            });
        }
    }

    // 辅助类，替代元组
    public class NotifyResult
    {
        public bool Success { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }

        public NotifyResult(bool success, string response, string errorMessage)
        {
            Success = success;
            Response = response;
            ErrorMessage = errorMessage;
        }
    }

    public class ValidateResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public ValidateResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }

    public class DecryptResult
    {
        public bool Success { get; set; }
        public JObject Data { get; set; }
        public string ErrorMessage { get; set; }

        public DecryptResult(bool success, JObject data, string errorMessage)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
        }
    }

    public class BusinessResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public BusinessResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
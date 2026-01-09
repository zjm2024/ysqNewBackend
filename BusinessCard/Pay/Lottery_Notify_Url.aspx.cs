using BusinessCard.WX_Pay;
using Newtonsoft.Json;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using System;
using System.Web.UI;

namespace BusinessCard.Pay
{
    public partial class Lottery_Notify_Url : Page
    {
        private readonly LogBO _log = new LogBO(typeof(Lottery_Notify_Url));
        private WeChatPayV3Notify _notifyProcessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 设置为JSON响应
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";

            try
            {
                // 只处理POST请求
                if (Request.HttpMethod != "POST")
                {
                    _log.Warn("收到非POST请求，方法: " + Request.HttpMethod);
                    ReturnError("仅支持POST请求");
                    return;
                }

                //_log.Info("开始处理微信支付V3回调");

                // 获取API V3密钥
                string apiV3Key = GetApiV3Key();
                if (string.IsNullOrEmpty(apiV3Key))
                {
                    _log.Error("API V3密钥未配置");
                    ReturnError("系统配置错误");
                    return;
                }

                // 创建处理器实例
                var processor = new WeChatPayV3Notify();

                // 处理回调
                var result = processor.ProcessV3Notify(Request, apiV3Key);

                // 返回结果
                if (result.Success)
                {
                    _log.Info("微信支付回调处理成功");
                    Response.Write(result.Response);
                }
                else
                {
                    _log.Error("微信支付回调处理失败: " + result.ErrorMessage);
                    Response.Write(result.Response);
                }
            }
            catch (Exception ex)
            {
                _log.Error("处理微信支付回调异常: " + ex.Message + ", StackTrace: " + ex.StackTrace);
                ReturnError("系统异常: " + ex.Message);
            }
        }

        /// <summary>
        /// 获取API V3密钥
        /// </summary>
        private string GetApiV3Key()
        {
            try
            {
                // 从web.config的AppSettings中读取
                AppVO AppVO = AppBO.GetApp(30);

                _log.Info("成功获取API V3密钥");
                return AppVO.API_V3_KEY;
            }
            catch (Exception ex)
            {
                _log.Error("获取API V3密钥失败: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 返回错误响应
        /// </summary>
        private void ReturnError(string errorMessage)
        {
            var errorResponse = new
            {
                code = "FAIL",
                message = errorMessage
            };

            Response.Write(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
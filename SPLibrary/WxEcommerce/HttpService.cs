using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using SPLibrary.WxEcommerce;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WxEcommerce
{
    /// <summary>
    /// http连接基础类，负责底层的http通信
    /// </summary>
    public class HttpService
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="json"></param>
        /// <param name="url"></param>
        /// <param name="isWechatpaySerial"></param>
        /// <returns></returns>
        public static async Task<ResultCode>  Post(string json, string url, bool isWechatpaySerial = true)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            ResultCode result = new ResultCode();//返回结果
            try
            {
                HttpClient httpClient = new HttpClient(new HttpHandler(EConfig.GetConfig().GetMchID(), EConfig.GetConfig().GetSerial(), isWechatpaySerial));

                var buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    result.code = "SUCCESS";
                    result.message = "成功";
                    result.ResultStr = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    result.ResultStr = await response.Content.ReadAsStringAsync();
                    dynamic resultContent = JsonConvert.DeserializeObject(result.ResultStr, new { code = "", message = "" }.GetType());
                    result.code = resultContent.code;
                    result.message = resultContent.message;
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                //Log.Error("Exception message: {0}", e.Message);
                //result = e.Message;
                result.code = "Thread_ERROR";
                result.message = e.Message;
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                //result = e.ToString();
                result.code = "Web_ERROR";
                result.message = e.Message;
                //Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    //Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                //throw new WxPayException(e.ToString());
            }
            catch (Exception e)
            {
                //result = e.ToString();
                result.code = "EX_ERROR";
                result.message = e.Message;
                //Log.Error("HttpService", e.ToString());
                //throw new WxPayException(e.ToString());
            }
            finally
            {
                
            }
            return result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isWechatpaySerial"></param>
        /// <returns></returns>
        public static async Task<ResultCode> Get(string url, bool isWechatpaySerial = true)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            ResultCode result = new ResultCode();//返回结果
            try
            {
                HttpClient httpClient = new HttpClient(new HttpHandler(EConfig.GetConfig().GetMchID(), EConfig.GetConfig().GetSerial(), isWechatpaySerial));
                
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    result.code = "SUCCESS";
                    result.message = "成功";
                    result.ResultStr = await response.Content.ReadAsStringAsync();
                }else
                {
                    result.ResultStr = await response.Content.ReadAsStringAsync();
                    dynamic resultContent = JsonConvert.DeserializeObject(result.ResultStr, new { code = "", message = "" }.GetType());
                    result.code = resultContent.code;
                    result.message = resultContent.message;
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                //Log.Error("Exception message: {0}", e.Message);
                //result = e.Message;
                result.code = "Thread_ERROR";
                result.message = e.Message;
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                //result = e.ToString();
                result.code = "Web_ERROR";
                result.message = e.Message;
                //Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    //Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                //throw new WxPayException(e.ToString());
            }
            catch (Exception e)
            {
                //result = e.ToString();
                result.code = "EX_ERROR";
                result.message = e.Message;
                //Log.Error("HttpService", e.ToString());
                //throw new WxPayException(e.ToString());
            }
            finally
            {

            }
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadfile"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<ResultCode> UploadFile(string uploadfile)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            ResultCode result = new ResultCode();//返回结果
            try
            {

                string filePath = uploadfile;
                string boundary = string.Format("--{0}", DateTime.Now.Ticks.ToString("x"));
                var sha256 = SHAFile.SHA256File(filePath);//计算文件哈希值
                meta meta = new meta()
                {
                    sha256 = sha256,
                    filename = System.IO.Path.GetFileName(filePath)
                };
                var json = JsonConvert.SerializeObject(meta);
                HttpClient client = new HttpClient(new HttpHandler(EConfig.GetConfig().GetMchID(), EConfig.GetConfig().GetSerial(),false, json));

                using (var requestContent = new MultipartFormDataContent(boundary))
                {
                    requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data"); //这里必须添加
                    requestContent.Add(new StringContent(json, Encoding.UTF8, "application/json"), "\"meta\""); //这里主要必须要双引号
                    var fileInfo = new FileInfo(filePath);
                    using (var fileStream = fileInfo.OpenRead())
                    {
                        var content = new byte[fileStream.Length];
                        fileStream.Read(content, 0, content.Length);
                        var byteArrayContent = new ByteArrayContent(content);
                        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                        requestContent.Add(byteArrayContent, "\"file\"", "\"" + meta.filename + "\"");  //这里主要必须要双引号
                        HttpResponseMessage response = await client.PostAsync("https://api.mch.weixin.qq.com/v3/merchant/media/upload", requestContent);//上传
                        if (response.IsSuccessStatusCode)
                        {
                            result.code = "SUCCESS";
                            result.message = "成功";
                            result.ResultStr = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            result.ResultStr = await response.Content.ReadAsStringAsync();
                            dynamic resultContent = JsonConvert.DeserializeObject(result.ResultStr, new { code = "", message = "" }.GetType());
                            result.code = resultContent.code;
                            result.message = resultContent.message;
                        }
                    }
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                //Log.Error("Exception message: {0}", e.Message);
                //result = e.Message;
                result.code = "Thread_ERROR";
                result.message = e.Message;
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                //result = e.ToString();
                result.code = "Web_ERROR";
                result.message = e.Message;
                //Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    //Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                //throw new WxPayException(e.ToString());
            }
            catch (Exception e)
            {
                //result = e.ToString();
                result.code = "EX_ERROR";
                result.message = e.Message;
                //Log.Error("HttpService", e.ToString());
                //throw new WxPayException(e.ToString());
            }
            finally
            {

            }
            return result;
        }
    }
    public class meta
    {
        public string filename { get; set; }

        public string sha256 { get; set; }
    }
}
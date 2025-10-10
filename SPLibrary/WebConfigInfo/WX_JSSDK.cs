using CoreFramework.VO;
using Newtonsoft.Json;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPLibrary.WebConfigInfo
{
    /// <summary>
    /// 乐聊名片订阅号微信通用类
    /// </summary>
    public class WX_JSSDK
    {
        public string appId = "wx67216e7509d25ecc";
        public string appSecret = "a60d0b6b0987e80b95f4bc1110ea26a3";
        public WX_JSSDK()
        {
            
        }

        public WX_JSSDK(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        /// <summary>
        /// 获取分享数据包
        /// </summary>
        /// <returns></returns>
        public ViewBag getSignPackage(string url="")
        {
            string jsapiTicket = getJsApiTicket();
            string url_req = HttpContext.Current.Request.Url.ToString();
            string pageurl = "";
            if (url == "")
            {
                pageurl = HttpContext.Current.Request.Url.AbsoluteUri;
            }else
            {
                pageurl = url;
            }
            string timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));
            string nonceStr = createNonceStr();

            // 这里参数的顺序要按照 key 值 ASCII 码升序排序  
            string rawstring = "jsapi_ticket=" + jsapiTicket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + pageurl + "";
            string signature = SHA1_Hash(rawstring);

            ViewBag signPackage = new ViewBag();
            signPackage.appid = appId;
            signPackage.nonceStr = nonceStr;
            signPackage.timestamp = timestamp;
            signPackage.url = pageurl;
            signPackage.signature = signature;
            signPackage.jsapiTicket = jsapiTicket;
            return signPackage;
        }

        /// <summary>
        /// 创建随机字符串 
        /// </summary>
        /// <returns></returns>
        public string createNonceStr()
        {
            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }

        /// <summary>
        /// SHA1哈希加密算法
        /// </summary>
        /// <param name="str_sha1_in"></param>
        /// <returns></returns>
        public string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = System.Text.UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }

        /// <summary>
        /// 获取ticket
        /// </summary>
        /// <returns></returns>
        public string getJsApiTicket()
        {
            string accessToken = RequestAccessToken();  //获取系统的全局token 
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accessToken + "&type=jsapi";
            Jsapi api = JsonConvert.DeserializeObject<Jsapi>(httpGet(url));
            string ticket = api.ticket;
            return ticket;
        }

        /// <summary>
        /// 发起一个http请求，返回值 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string httpGet(string url)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  
                string pageHtml = System.Text.Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              
                return pageHtml;
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }

        /// <summary>
        /// 获取微信公众号全局唯一接口凭证
        /// </summary>
        /// <returns></returns>
        public string RequestAccessToken()
        {   // 设置参数
            string appid = appId;//第三方用户唯一凭证
            string appsecret = appSecret;//第三方用户唯一凭证密钥，即appsecret

            //请求接口获取
            string _url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;
            string method = "GET";
            HttpWebRequest request = WebRequest.Create(_url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method;
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");

            //发送请求并获取响应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            //获取返回过来的结果
            string content = sr.ReadToEnd();

            dynamic resultContent = JsonConvert.DeserializeObject(content, new { access_token = "", expires_in = "", errcode = "", errmsg = "" }.GetType());
            if (resultContent != null && !string.IsNullOrWhiteSpace(resultContent.access_token)) //注意：请求成功时是不会有errcode=0返回,判断access_token是否有值即可
            {
                return resultContent.access_token;//返回请求唯一凭证
            }
            else
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + content + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return "";
            }
        }

        /// <summary>
        /// 创建图文并群发
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int add_news(string title,string content,int CustomerId)
        {
            //使用众销乐公众号
            this.appId = "wx9a65d7becbbb017a";
            this.appSecret = "2f75313a696eac6cef7a393641ff7a68";

            content=content.Replace("\"","\\\"");
            content=content.Replace("'", "\\\"");

            string access_token = RequestAccessToken();
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=" + access_token;
            string madiaId = uploadimgGetmediaId("https://www.zhongxiaole.net/SPManager/Style/images/wxcard/logo.jpg");
            if (madiaId == "")
            {
                madiaId = "W2jBdPwlIJumqgUsisKKYUz0OAFUsCNpZFjkqjLrK6zJhPPCt1fHVjC_heRpVYD1";
            }

            string DataJson = string.Empty;
            DataJson = "{";
            DataJson += "\"articles\": [ {";
            DataJson += "\"title\": \""+ title + "\",";
            DataJson += "\"thumb_media_id\": \""+ madiaId + "\",";
            DataJson += "\"show_cover_pic\": \"0\",";
            DataJson += "\"content\": \""+ content + "\",";
            DataJson += "\"content_source_url\": \"\"";
            DataJson += "}]";
            DataJson += "}";

            try
            {
                
                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { media_id = "" }.GetType());
                if (resultContent != null && !string.IsNullOrWhiteSpace(resultContent.media_id))
                {
                    //群发
                    string wxaurl2 = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=" + access_token;
                    DataJson = "{";
                    DataJson += "\"filter\":{\"is_to_all\":false,\"tag_id\":102},";
                    DataJson += "\"mpnews\":{\"media_id\":\"" + resultContent.media_id + "\"},";
                    DataJson += "\"msgtype\":\"mpnews\",";
                    DataJson += "\"send_ignore_reprint\":0";
                    DataJson += "}";

                    string str2 = HttpHelper.HtmlFromUrlPost(wxaurl2, DataJson);
                    WX_Sendall resultContent2 = JsonConvert.DeserializeObject<WX_Sendall>(str2);

                    MediaVO MediaVO = new MediaVO();
                    MediaVO.MediaID = 0;
                    MediaVO.CustomerId = CustomerId;
                    MediaVO.title = title;
                    MediaVO.content = content;
                    MediaVO.media_id = resultContent.media_id;
                    MediaVO.msg_id = resultContent2.msg_id;
                    MediaVO.msg_data_id = resultContent2.msg_data_id;
                    CardBO cBO = new CardBO(new CustomerProfile());
                    int MediaID=cBO.AddMedia(MediaVO);

                    return MediaID;//返回media_id
                }
                else
                {
                    LogBO _log = new LogBO(typeof(WX_JSSDK));
                    string strErrorMsg = "Message:" + str + "\r\n "+ resultContent;
                    _log.Error(strErrorMsg);
                    //请求失败，返回为空
                    return -1;
                }
            }
            catch(Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_JSSDK));
                string strErrorMsg = "Message:" + ex.Message + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return -1;
            }
        }

        /// <summary>
        /// 上传图文消息内的图片获取URL
        /// </summary>
        /// <param name="imgurl"></param>
        /// <returns></returns>
        public string uploadimg(string imgurl)
        {
            //使用众销乐公众号
            this.appId = "wx9a65d7becbbb017a";
            this.appSecret = "2f75313a696eac6cef7a393641ff7a68";

            string access_token = RequestAccessToken();
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=" + access_token;

            try
            {
                if (imgurl.IndexOf("www.zhongxiaole.net") < 0)
                {
                    CardBO cBO = new CardBO(new CustomerProfile());
                    imgurl = cBO.downloadImages(imgurl);
                }
                imgurl = imgurl.Replace("https://www.zhongxiaole.net/SPManager", ConfigInfo.Instance.UploadFolder);

                string str = HttpHelper.UploadFileEx(imgurl, wxaurl,new System.Collections.Specialized.NameValueCollection());
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { url = "" }.GetType());
                if (resultContent != null && !string.IsNullOrWhiteSpace(resultContent.url))
                {
                    return resultContent.url;//返回media_id
                }
                else
                {
                    LogBO _log = new LogBO(typeof(WX_JSSDK));
                    string strErrorMsg = "Message:" + str + "\r\n " + resultContent;
                    _log.Error(strErrorMsg);
                    //请求失败，返回为空
                    return "";
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_JSSDK));
                string strErrorMsg = "Message:" + ex.Message + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return "";
            }
        }

        /// <summary>
        /// 上传客服消息内的图片获取mediaId
        /// </summary>
        /// <param name="imgurl"></param>
        /// <returns></returns>
        public string uploadimgGetmediaId(string imgurl)
        {
            string access_token = RequestAccessToken();
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + access_token+ "&type=image";

            try
            {
                imgurl = imgurl.Replace("https://www.zhongxiaole.net/SPManager", ConfigInfo.Instance.UploadFolder);

                string str = HttpHelper.UploadFileEx(imgurl, wxaurl, new System.Collections.Specialized.NameValueCollection());
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { media_id = "" }.GetType());
                if (resultContent != null && !string.IsNullOrWhiteSpace(resultContent.media_id))
                {
                    return resultContent.media_id;//返回media_id
                }
                else
                {
                    LogBO _log = new LogBO(typeof(WX_JSSDK));
                    string strErrorMsg = "Message:" + str + "\r\n " + resultContent;
                    _log.Error(strErrorMsg);
                    //请求失败，返回为空
                    return "";
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_JSSDK));
                string strErrorMsg = "Message:" + ex.Message + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return "";
            }
        }

        /// <summary>
        /// 发送客服消息（文本）
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public bool sendText(string Text,string OpenID)
        {
            string access_token = RequestAccessToken();
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access_token;

            try
            {
                string DataJson = string.Empty;
                DataJson = "{";
                DataJson += "\"access_token\":\"" + access_token + "\",";
                DataJson += "\"touser\":\"" + OpenID + "\",";
                DataJson += "\"msgtype\":\"text\",";
                DataJson += "\"text\":{\"content\":\"" + Text + "\"}";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                LogBO _log = new LogBO(typeof(WX_JSSDK));
                string strErrorMsg = "Message:" + str;
                _log.Error(strErrorMsg);

                dynamic resultContent = JsonConvert.DeserializeObject(str, new { errcode=0, errmsg = "" }.GetType());
                if (resultContent != null && resultContent.errcode==0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_JSSDK));
                string strErrorMsg = "Message:" + ex.Message + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return false;
            }
        }

        /// <summary>
        /// 发送客服消息（图片）
        /// </summary>
        /// <param name="ImgUrl"></param>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public string sendImg(string ImgUrl, string OpenID)
        {
            string access_token = RequestAccessToken();
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access_token;
            string media_id = uploadimgGetmediaId(ImgUrl);
            try
            {
                string DataJson = string.Empty;
                DataJson = "{";
                DataJson += "\"access_token\":\"" + access_token + "\",";
                DataJson += "\"touser\":\"" + OpenID + "\",";
                DataJson += "\"msgtype\":\"image\",";
                DataJson += "\"image\":{\"media_id\":\"" + media_id + "\"}";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                return str;
                /*
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { errcode = 0, errmsg = "" }.GetType());
                if (resultContent != null && resultContent.errcode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }*/
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_JSSDK));
                string strErrorMsg = "Message:" + ex.Message + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return "";
            }
        }

        /// <summary>
        /// 创建用户标签
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string add_Tags(string title)
        {
            string access_token = RequestAccessToken();
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/tags/create?access_token=" + access_token;

            string DataJson = string.Empty;
            DataJson = "{";
            DataJson += "\"tag\": {";
            DataJson += "\"name\": \"" + title + "\"";
            DataJson += "}";
            DataJson += "}";

            try
            {
                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                return str;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_JSSDK));
                string strErrorMsg = "Message:" + ex.Message + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return null;
            }
        }

        /// <summary>
        /// 获取微信登录链接
        /// </summary>
        /// <param name="returnURL"></param>
        /// <returns></returns>
        public string GetThirdPartURL(string returnURL)
        {
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appId + "&redirect_uri=" + returnURL + "&response_type=code&scope=snsapi_userinfo&state=2#wechat_redirect";
            return url;
        }

        #region 创建Json序列化 及反序列化类目  
        //
        /// <summary>
        /// 创建JSon类 保存文件 jsapi_ticket.json  
        /// </summary>
        public class JSTicket
        {
            public string jsapi_ticket { get; set; }

            public double expire_time { get; set; }
        }

        /// <summary>
        /// 创建 JSon类 保存文件 access_token.json  
        /// </summary>
        public class AccToken
        {
            public string access_token { get; set; }

            public double expires_in { get; set; }
        }

        /// <summary>
        /// 创建从微信返回结果的一个类 用于获取ticket  
        /// </summary>

        public class Jsapi
        {
            public int errcode { get; set; }

            public string errmsg { get; set; }

            public string ticket { get; set; }

            public string expires_in { get; set; }
        }


        
        #endregion
    }
    /// <summary>
    /// 分享数据包  
    /// </summary>
    public class ViewBag
    {
        public string appid { get; set; }

        public string nonceStr { get; set; }

        public string timestamp { get; set; }

        public string url { get; set; }

        public string signature { get; set; }

        public string jsapiTicket { get; set; }
    }

    /// <summary>
    /// 图文素材列表  
    /// </summary>
    public class WX_News_list
    {
        public List<WX_News> news_item { get; set; }
    }

    /// <summary>
    /// 图文素材  
    /// </summary>
    public class WX_News
    {
        public string title { get; set; }

        public string thumb_media_id { get; set; }

        public int show_cover_pic { get; set; }

        public string author { get; set; }

        public string digest { get; set; }

        public string content { get; set; }

        public string url { get; set; }

        public string content_source_url { get; set; }
    }

    /// <summary>
    /// 客服消息事件
    /// </summary>
    public class WX_Msg
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public string CreateTime { get; set; }

        public string MsgType { get; set; }

        public string MsgId { get; set; }

        public string Title { get; set; }

        public string AppId { get; set; }

        public string PagePath { get; set; }

        public string ThumbUrl { get; set; }

        public string ThumbMediaId { get; set; }
    }

    /// <summary>
    /// 群发结果  
    /// </summary>
    public class WX_Sendall
    {
        public string errcode { get; set; }

        public string errmsg { get; set; }

        public string msg_id { get; set; }

        public string msg_data_id { get; set; }

    }
}

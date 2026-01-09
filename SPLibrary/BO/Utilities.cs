using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using CoreFramework;
using CoreFramework.VO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework.Logging.BO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using System.Security.Authentication;

namespace SPLibrary.CoreFramework.BO
{
    public class Utilities
    {
        public const string SQL_ESCAPE_CHAR = "\\";
        public static void RegisterJs(Page page, String strKey, string JS)
        {
            string strDefaultKey = string.Empty;
            string strRandKey = string.Empty;

            Random ran = new Random();
            int intTemp = new int();
            for (int i = 0; i < 9; i++)
            {
                intTemp = ran.Next(10);
                strRandKey += intTemp.ToString();
            }

            if (!string.IsNullOrEmpty(strKey))
            {
                strDefaultKey = strKey;
            }
            else
            {
                strDefaultKey = strRandKey;
            }
            JS = JS + ";";
            page.ClientScript.RegisterStartupScript(typeof(Page), strDefaultKey, JS, true);

        }

        public static string GetMD5(string input)
        {
            byte[] result = Encoding.Default.GetBytes(input);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }
        public static string MakePassword(int intlen)
        {
            string strPwChar = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strRe = "";
            int iRandNum;
            Random rnd = new Random();
            for (int i = 0; i < intlen; i++)
            {
                iRandNum = rnd.Next(strPwChar.Length);
                strRe += strPwChar[iRandNum];
            }
            return strRe;
        }
        public static string ReplaceSQLStrForQuote(string queryStr)
        {
            string sReturn = (queryStr == null) ? "" : queryStr;
            sReturn = sReturn.Replace("'", "''");
            return sReturn;
        }

        public static string ReplaceSQLStrForLike(string queryStr, string escapeChar)
        {
            string sReturn = (queryStr == null) ? "" : queryStr;
            if (DBConfig.ProviderType == EProviderType.MySQL)
                sReturn = sReturn.Replace(escapeChar.ToString(), string.Format("{0}{0}{0}", escapeChar));
            else
                sReturn = sReturn.Replace(escapeChar.ToString(), string.Format("{0}{0}", escapeChar));
            sReturn = sReturn.Replace("%", escapeChar + "%");
            sReturn = sReturn.Replace("_", escapeChar + "_");
            sReturn = sReturn.Replace("[", escapeChar + "[");
            sReturn = sReturn.Replace("]", escapeChar + "]");
            sReturn = sReturn.Replace("^", escapeChar + "^");
            sReturn = sReturn.Replace("'", escapeChar + "''");
            return sReturn;
        }

        public static string ReplaceSQLStrForLike(string queryStr)
        {
            return ReplaceSQLStrForLike(queryStr, SQL_ESCAPE_CHAR);
        }
        public static void PushMessageToSingle(string messagestr, string clientId)
        {
            IGtPush push = new IGtPush(ConfigInfo.Instance.GTHOST, ConfigInfo.Instance.GTAPPKEY, ConfigInfo.Instance.GTMASTERSECRET);

            //消息模版：TransmissionTemplate:透传模板

            NotificationTemplate template = NotificationTemplateDemo(messagestr);
            //TransmissionTemplate template = TransmissionTemplateDemo(messagestr);


            // 单推消息模型
            SingleMessage message = new SingleMessage();
            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;
            //判断是否客户端是否wifi环境下推送，2为4G/3G/2G，1为在WIFI环境下，0为不限制环境
            message.PushNetWorkType = 0;
            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
            target.appId = ConfigInfo.Instance.GTAPPID;
            target.clientId = clientId;
            //target.alias = ALIAS;
            try
            {
                String pushResult = push.pushMessageToSingle(message, target);

                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("----------------服务端返回结果：" + pushResult);
                //LogBO _log = new LogBO(typeof(Utilities));
                //string strErrorMsg = "Message Push Result:" + pushResult;
                //_log.Error(strErrorMsg);
            }
            catch (RequestException e)
            {
                String requestId = e.RequestId;
                //发送失败后的重发
                String pushResult = push.pushMessageToSingle(message, target, requestId);
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("----------------服务端返回结果：" + pushResult);
                LogBO _log = new LogBO(typeof(Utilities));
                string strErrorMsg = "Message Push Result:" + pushResult;
                _log.Error(strErrorMsg);
            }
        }
        public static void pushMessageToApp(string messagestr)
        {
            IGtPush push = new IGtPush(ConfigInfo.Instance.GTHOST, ConfigInfo.Instance.GTAPPKEY, ConfigInfo.Instance.GTMASTERSECRET);
            // 定义"AppMessage"类型消息对象，设置消息内容模板、发送的目标App列表、是否支持离线发送、以及离线消息有效期(单位毫秒)
            AppMessage message = new AppMessage();

            NotificationTemplate template = NotificationTemplateDemo(messagestr);

            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;     // 离线有效时间，单位为毫秒，可选
            message.Data = template;
            //判断是否客户端是否wifi环境下推送，2:4G/3G/2G,1为在WIFI环境下，0为无限制环境
            //message.PushNetWorkType = 0; 
            //message.Speed = 1000;

            List<String> appIdList = new List<String>();
            appIdList.Add(ConfigInfo.Instance.GTAPPID);

            List<String> phoneTypeList = new List<String>();   //通知接收者的手机操作系统类型
            //phoneTypeList.Add("ANDROID");
            //phoneTypeList.Add("IOS");

            List<String> provinceList = new List<String>();    //通知接收者所在省份
            //provinceList.Add("浙江");
            //provinceList.Add("上海");
            //provinceList.Add("北京");

            List<String> tagList = new List<String>();
            //tagList.Add("中文");

            message.AppIdList = appIdList;
            //message.PhoneTypeList = phoneTypeList;
            //message.ProvinceList = provinceList;
            //message.TagList = tagList;


            String pushResult = push.pushMessageToApp(message);
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服务端返回结果：" + pushResult);
        }

        public static TransmissionTemplate TransmissionTemplateDemo(string messagestr)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = ConfigInfo.Instance.GTAPPID;
            template.AppKey = ConfigInfo.Instance.GTAPPKEY;
            template.TransmissionType = "1";            //应用启动类型，1：强制应用启动 2：等待应用启动
            template.TransmissionContent = messagestr;  //透传内容

            //设置客户端展示时间
            String begin = DateTime.Now.ToString();
            String end = DateTime.Now.AddMinutes(10).ToString();
            template.setDuration(begin, end);

            return template;
        }
        //通知透传模板动作内容
        public static NotificationTemplate NotificationTemplateDemo(string messagestr)
        {
            NotificationTemplate template = new NotificationTemplate();
            template.AppId = ConfigInfo.Instance.GTAPPID;
            template.AppKey = ConfigInfo.Instance.GTAPPKEY;
            //通知栏标题
            template.Title = "众销乐";
            //通知栏内容     
            template.Text = messagestr;
            //通知栏显示本地图片
            template.Logo = "";
            //通知栏显示网络图标
            template.LogoURL = "";
            //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionType = "1";
            //透传内容  
            template.TransmissionContent = messagestr;// "请填写透传内容";
            //接收到消息是否响铃，true：响铃 false：不响铃   
            template.IsRing = true;
            //接收到消息是否震动，true：震动 false：不震动   
            template.IsVibrate = true;
            //接收到消息是否可清除，true：可清除 false：不可清除    
            template.IsClearable = true;
            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
            String begin = DateTime.Now.ToString();
            String end = DateTime.Now.AddMinutes(10).ToString();
            template.setDuration(begin, end);

            return template;
        }
    }
    public class HttpHelper
    {
        public static string HtmlFromUrlGet(string url)
        {
            return HtmlFromUrlGet(url, null);
        }
        public static string HtmlFromUrlGet(string url, List<string> header)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0";
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                request.MaximumAutomaticRedirections = 3;
                request.KeepAlive = true;
                request.Timeout = 30000;

                if (header != null)
                {
                    foreach (string headerStr in header)
                    {
                        //req.Headers.Add("Authorization: Bearer ")
                        request.Headers.Add("Authorization: " + headerStr);
                    }

                }

                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    ResultObject result = new ResultObject() { Flag = 0, Message = "Net work error", Result = null };
                    return JsonConvert.SerializeObject(result);
                }
                else if (res.StatusCode != HttpStatusCode.OK)
                {
                    ResultObject result = new ResultObject() { Flag = (int)res.StatusCode, Message = "error", Result = null };
                    return JsonConvert.SerializeObject(result);
                }
                Stream responseStream = res.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                return str;
            }
            catch (Exception ex)
            {
                ResultObject result = new ResultObject() { Flag = 0, Message = "", Result = null };
                return JsonConvert.SerializeObject(result);
            }
        }
        public static string HtmlFromUrlPost(string url, string postData)
        {
            return HtmlFromUrlPost(url, postData, null);
        }
        public static string HtmlFromUrlPost(string url, string postData, List<string> header)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    ResultObject result = new ResultObject() { Flag = 0, Message = "Net work error", Result = null };
                    return JsonConvert.SerializeObject(result);
                }
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/json";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;

                if (header != null)
                {
                    foreach (string str in header)
                    {
                        //req.Headers.Add("Authorization: Bearer ")
                        req.Headers.Add("Authorization: " + str);
                    }

                }

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    ResultObject result = new ResultObject() { Flag = 0, Message = "Net work error", Result = null };
                    return JsonConvert.SerializeObject(result);
                }
                else if (res.StatusCode != HttpStatusCode.OK)
                {
                    ResultObject result = new ResultObject() { Flag = (int)res.StatusCode, Message = "error", Result = null };
                    return JsonConvert.SerializeObject(result);
                }
                Stream inStream = res.GetResponseStream();
                var sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();

                return htmlResult;
            }
            catch (Exception ex)
            {
                ResultObject result = new ResultObject() { Flag = 0, Message = ex.Message, Result = postData,Subsidiary= url };
                return JsonConvert.SerializeObject(result);
            }
        }
        

        public static Stream HtmlFromUrlPostByStream(string url, string postData)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return null;
                }
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/json";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;

                if (res == null)
                {
                    return null;
                }
                else if (res.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                Stream inStream = res.GetResponseStream();

                return inStream;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(Utilities));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        public static Stream HtmlFromUrlPostByStreamNew(string url, string postData)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return null;
                }
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/json";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;

                if (res == null || res.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                // 读取响应内容到MemoryStream（支持多次读取）
                using (Stream inStream = res.GetResponseStream())
                {
                    MemoryStream ms = new MemoryStream();
                    inStream.CopyTo(ms);
                    ms.Position = 0; // 重置流位置

                    // 调试：记录响应内容（如日志或文件）
                    string responseText = Encoding.UTF8.GetString(ms.ToArray());
                    // File.WriteAllBytes("response.jpg", ms.ToArray());
                    return ms;
                }
            }
            catch (Exception ex)
            {
                // ...（原有错误处理）
                LogBO _log = new LogBO(typeof(Utilities));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
		/// 上传文件的方法
		/// </summary>
		/// <param name="uploadfile">单个文件名（上传多个文件的方法自己修改）</param>
		/// <param name="url">post请求的url</param>
		/// <param name="poststring">post的字符串 键值对，相当于表单上的文本框里的字符</param>
		/// <returns></returns>
		public static string UploadFileEx(string uploadfile, string url, NameValueCollection poststring)
        {
            Uri uri = new Uri(url);

            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";


            // Build up the post message header 
            StringBuilder sb = new StringBuilder();

            //加文本
            foreach (string key in poststring.Keys)
            {
                sb.Append("--");
                sb.Append(boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append(key);
                sb.Append("\"");
                sb.Append("\r\n");
                sb.Append("\r\n");
                sb.Append(poststring.Get(key));
                sb.Append("\r\n");
            }

            //加文件
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"filename\";");
            sb.Append("filename=\"");
            sb.Append(Path.GetFileName(uploadfile));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: multipart/form-data");
            sb.Append("\r\n");
            sb.Append("\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array 
            // ensuring the boundary appears on a line by itself 
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            FileStream fileStream = new FileStream(uploadfile, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // Write out our post header 
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // Write out the file contents 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // Write out the trailing boundary 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

            webrequest.Timeout = 1000000;

            //System.Windows.Forms.MessageBox.Show(webrequest.Timeout.ToString()); 

            WebResponse responce = webrequest.GetResponse();

            Stream s = responce.GetResponseStream();

            StreamReader sr = new StreamReader(s);

            string str = sr.ReadToEnd();


            fileStream.Close();

            requestStream.Close();

            sr.Close();

            s.Close();

            responce.Close();

            return str;
        }

        public static string CrawlerGetHtml(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.MaximumAutomaticRedirections = 3;
                request.KeepAlive = true;
                request.Timeout = 30000;

                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    return null;
                }
                else if (res.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                Stream responseStream = res.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                return str;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// get请求带重试
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr, int sum)
        {
            string temp = null;
            for (int i = 0; i < sum; i++)
            {
                if (string.IsNullOrEmpty(temp) || string.IsNullOrEmpty(temp.Trim()))
                {
                    temp = HttpGet(Url, postDataStr);
                }
                else
                {
                    break;
                }
            }
            return temp;
        }

        /// <summary>
        /// GET请求与获取结果
        /// </summary>
        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.AllowAutoRedirect = false;//禁止自动跳转
                //设置User-Agent，伪装成Google Chrome浏览器
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                request.Timeout = 5000;//定义请求超时时间为5秒
                request.KeepAlive = true;//启用长连接
                request.Method = "GET";//定义请求方式为GET        
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }

    public class HtmlParser
    {
        private string[] htmlcode; //把html转为数组形式用于分析  
        private StringBuilder result = new StringBuilder();  //输出的结果  
        private int seek; //分析文本时候的指针位置  
        private string[] keepTag;  //用于保存要保留的尖括号内容  
        private bool _inTag;  //标记现在的指针是不是在尖括号内  
        private bool needContent = true;  //是否要提取正文  
        private string tagName;  //当前尖括号的名字  
        private string[] specialTag = new string[] { "script", "style", "!--" };  //特殊的尖括号内容，一般这些标签的正文是不要的  

        /// <summary>  
        /// 当指针进入尖括号内，就会触发这个属性。这里主要逻辑是提取尖括号里的标签名字  
        /// </summary>  
        public bool inTag
        {
            get { return _inTag; }
            set
            {
                _inTag = value;
                if (!value)
                    return;
                bool ok = true;
                tagName = "";
                while (ok)
                {
                    string word = read();
                    if (word != " " && word != ">")
                    {
                        tagName += word;
                    }
                    else if (word == " " && tagName.Length > 0)
                    {
                        ok = false;
                    }
                    else if (word == ">")
                    {
                        ok = false;
                        inTag = false;
                        seek -= 1;
                    }
                }
            }
        }
        /// <summary>  
        /// 初始化类  
        /// </summary>  
        /// <param name="html">  
        ///  要分析的html代码  
        /// </param>  
        public HtmlParser(string html)
        {
            htmlcode = new string[html.Length];
            for (int i = 0; i < html.Length; i++)
            {
                htmlcode[i] = html[i].ToString();
            }
            KeepTag(new string[] { });
        }
        /// <summary>  
        /// 设置要保存那些标签不要被过滤掉  
        /// </summary>  
        /// <param name="tags">  
        ///  
        /// </param>  
        public void KeepTag(string[] tags)
        {
            keepTag = tags;
        }

        /// <summary>  
        ///   
        /// </summary>  
        /// <returns>  
        /// 输出处理后的文本  
        /// </returns>  
        public string Text()
        {
            int startTag = 0;
            int endTag = 0;
            while (seek < htmlcode.Length)
            {
                string word = read();
                if (word.ToLower() == "<")
                {
                    startTag = seek;
                    inTag = true;
                }
                else if (word.ToLower() == ">")
                {
                    endTag = seek;
                    inTag = false;
                    if (iskeepTag(tagName.Replace("/", "")))
                    {
                        for (int i = startTag - 1; i < endTag; i++)
                        {
                            result.Append(htmlcode[i].ToString());
                        }
                    }
                    else if (tagName.StartsWith("!--"))
                    {
                        bool ok = true;
                        while (ok)
                        {
                            if (read() == "-")
                            {
                                if (read() == "-")
                                {
                                    if (read() == ">")
                                    {
                                        ok = false;
                                    }
                                    else
                                    {
                                        seek -= 1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in specialTag)
                        {
                            if (tagName == str)
                            {
                                needContent = false;
                                break;
                            }
                            else
                                needContent = true;
                        }
                    }
                }
                else if (!inTag && needContent)
                {
                    result.Append(word);
                }

            }
            return result.ToString();
        }
        /// <summary>  
        /// 判断是否要保存这个标签  
        /// </summary>  
        /// <param name="tag">  
        /// A <see cref="System.String"/>  
        /// </param>  
        /// <returns>  
        /// A <see cref="System.Boolean"/>  
        /// </returns>  
        private bool iskeepTag(string tag)
        {
            foreach (string ta in keepTag)
            {
                if (tag.ToLower() == ta.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        private string read()
        {
            return htmlcode[seek++];
        }
    }
    public class WebSnapshotsHelper
    {
        Bitmap m_Bitmap;
        string m_Url;
        int m_BrowserWidth, m_BrowserHeight, m_ThumbnailWidth, m_ThumbnailHeight;
        public WebSnapshotsHelper(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            m_Url = Url;
            m_BrowserHeight = BrowserHeight;
            m_BrowserWidth = BrowserWidth;
            m_ThumbnailWidth = ThumbnailWidth;
            m_ThumbnailHeight = ThumbnailHeight;
        }
        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            try
            {
                WebSnapshotsHelper thumbnailGenerator = new WebSnapshotsHelper(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
                return thumbnailGenerator.GenerateWebSiteThumbnailImage();
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WebSnapshotsHelper));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
            
        }
        public Bitmap GenerateWebSiteThumbnailImage()
        {
            try
            {
                Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
                m_thread.SetApartmentState(ApartmentState.STA);
                m_thread.Start();
                m_thread.Join();
                return m_Bitmap;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WebSnapshotsHelper));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }
        private void _GenerateWebSiteThumbnailImage()
        {
            try
            {
                WebBrowser m_WebBrowser = new WebBrowser();
                m_WebBrowser.ScrollBarsEnabled = false;
                m_WebBrowser.Navigate(m_Url);
                m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                    Application.DoEvents();
                m_WebBrowser.Dispose();
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WebSnapshotsHelper));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                WebBrowser m_WebBrowser = (WebBrowser)sender;
                m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                m_WebBrowser.ScrollBarsEnabled = false;
                m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
                m_WebBrowser.BringToFront();
                m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
                m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WebSnapshotsHelper));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }
        }
    }
}

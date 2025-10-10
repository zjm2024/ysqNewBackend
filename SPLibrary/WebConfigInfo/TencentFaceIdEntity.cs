using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Web;
using Newtonsoft.Json;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;

namespace SPLibrary.WebConfigInfo
{
    /// <summary>
    /// 人脸身份证比对验证
    /// </summary>
    public class TencentFaceIdEntity
    {
        public static String HMACSHA1Text(String EncryptText, String EncryptKey)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(EncryptKey);

            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(EncryptText);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 识别人脸和身份证
        /// </summary>
        /// <param name="imgPath">人脸图片绝对路径</param>
        /// <param name="name">姓名</param>
        /// <param name="number">身份证</param>
        public static EntityVO Main(string imgPath, string name, string number)
        {
            try
            {
                String url = "http://service-1o4x1xk1-1253495967.ap-beijing.apigateway.myqcloud.com/release/efficient/idfaceIdentity";
                String method = "POST";
                String querys = "";
                String postData = "base64Str=" + decodeImageToBase64(imgPath) + "&liveChk=1&name="+ HttpUtility.UrlEncode(name) + "&number=" + HttpUtility.UrlEncode(number);

                //云市场分配的密钥Id
                String secretId = "AKID6W52fJOmUfg57XQUafCtW3Ir8C08hG5Z5R0f";
                //云市场分配的密钥Key
                String secretKey = "5R10rgo87243XpQUvfAvAo4wWB55QuainWVyqN4t";
                String source = "market";

                String dt = DateTime.UtcNow.GetDateTimeFormats('r')[0];
                url = url + "?" + querys;

                String signStr = "x-date: " + dt + "\n" + "x-source: " + source;
                String sign = HMACSHA1Text(signStr, secretKey);

                String auth = "hmac id=\"" + secretId + "\", algorithm=\"hmac-sha1\", headers=\"x-date x-source\", signature=\"";
                auth = auth + sign + "\"";

                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                if (url.Contains("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                }

                httpRequest.Method = method;
                httpRequest.ContentLength = postData.Length;
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.Headers.Add("Authorization", auth);
                httpRequest.Headers.Add("X-Source", source);
                httpRequest.Headers.Add("X-Date", dt);
                httpRequest.GetRequestStream().Write(System.Text.Encoding.ASCII.GetBytes(postData), 0, postData.Length);

                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpResponse = (HttpWebResponse)ex.Response;
                    LogBO _log = new LogBO(typeof(TencentFaceIdEntity));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                }

                Console.WriteLine(httpResponse.StatusCode);
                Console.WriteLine(httpResponse.Headers);
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

                var model = JsonConvert.DeserializeObject<EntityVO>(reader.ReadToEnd());
                return model;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #region 图片转base64
        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="path">图片路径</param>      
        /// <returns>返回一个base64字符串</returns>
        public static string decodeImageToBase64(string path)
        {
            string base64str = "";

            //站点文件目录
            string fileDir = HttpContext.Current.Server.MapPath("/");
            string[] arrfileDir = fileDir.Split('\\');
            fileDir = arrfileDir[0] + "\\" + arrfileDir[1] + "\\" + arrfileDir[2];
            try
            {
                //读图片转为Base64String
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);
                //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                bmp.Dispose();
                base64str = Convert.ToBase64String(arr);
            }
            catch (Exception e)
            {
                string mss = e.Message;
            }
            //return "data:image/jpg;base64," + base64str;
            return base64str;
        }
        #endregion
    }
    public class EntityVO
    {
        public int error_code { get; set; }
        public string reason { get; set; }
        public Eresult result { get; set; }
    }
    public class Eresult
    {
        public string IdCardNo { get; set; }
        public string Name { get; set; }
        public int Validate_Result { get; set; } //验证结果	-1,身份证和姓名不一致 -2,公安库中无此身份证记录 -3,公安身份证库中没有此号码的照片-4 照片参数不合格 -5 照片相片体积过大 -6,请检查图片编码 -7,照片相片体积过小 1,系统分析为同一人 ，2,系统分析可能为同一人 3, 系统分析为不是同人 4,没检测到人脸 5,疑似非活体 6,出现多张脸 7,身份证和姓名一致，官方人脸比对失败
        public double Similarity { get; set; } //相似度 1~100 (当validate_result>0时，本值才有效(相似度>=45 为同一人； 40<=相似度<45 不确定为同一人； 相似度<40 确定为不同人))
    }

}

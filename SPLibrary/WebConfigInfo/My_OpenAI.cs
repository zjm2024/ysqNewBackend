using CoreFramework.VO;
using Newtonsoft.Json;
using RestSharp;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Cdn.V20180606.Models;
using TencentCloud.Es.V20180416;
using TencentCloud.Tbp.V20190311.Models;

namespace SPLibrary.WebConfigInfo
{
    public class My_OpenAI
    {
        private const string API_ENDPOINT = "https://api.openai.com/v1/engines/davinci-codex/completions";
        public static string API_KEY = "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";

        public static string GetCompletionAsync(string prompt)
        {
            var requestBody = new
            {
                engine = "text-davinci-003",
                prompt = prompt,
                max_tokens =2048,
                n = 1,
                stop = "None",
            };

            List<string> header = new List<string>();
            header.Add($"Bearer {API_KEY}");
            var response = HtmlFromUrlPost(API_ENDPOINT, JsonConvert.SerializeObject(requestBody), header);
            return response;
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
                ResultObject result = new ResultObject() { Flag = 0, Message = ex.Message, Result = postData, Subsidiary = url };
                return JsonConvert.SerializeObject(result);
            }
        }
    }
}

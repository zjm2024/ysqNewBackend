using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using SPLibrary.CoreFramework.Logging.BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SPLibrary.CustomerManagement.BO
{
    public class MessageTool
    {
        //阿里云接口密钥
        public static string accessKeyId = "LTAI4GFVPQ8TmBSqGDD4zV3N";
        public static string accessSecret = "6Z3Vuf5KczJtbfXkgcQ5k4Ah6m5dO9";

        /// <summary>
        /// 阿里云发送短信
        /// </summary>
        /// <param name="PhoneNumbers">手机号</param>
        /// <param name="TemplateCode">短信模板</param>
        /// <param name="TemplateParam">变量值</param>
        /// <returns></returns>
        public static bool ALSendSms(string PhoneNumbers,string TemplateCode,string TemplateParam)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", PhoneNumbers);
            request.AddQueryParameters("SignName", "乐聊名片小程序");
            request.AddQueryParameters("TemplateCode", TemplateCode);
            request.AddQueryParameters("TemplateParam", TemplateParam);
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                //Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public MessageTool()
        { }
        public static string SendMobileMsg(string msgContent, string Phone)
        {
            if (Phone == "13800138000")
                return "2";
            string returns = "2";
            string Url = string.Format("action=send&userid=1054&account={0}&password={1}&mobile={2}&content={3}&sendTime=&extno=", "悦尔电子", "yueer", Phone, msgContent);


            string Url1 = "http://123.196.122.28:8812/sms.aspx";
            if (ConfigurationManager.AppSettings["MessageAPIURL"] != null)
                Url1 = ConfigurationManager.AppSettings["MessageAPIURL"].ToString();
            string urlPath = Url1 + "?" + Url;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlPath); 
            request.Method = "POST";
            request.Accept = @"gzip,deflate";
            request.ContentType = @"text/xml;charset=utf-8";
            request.UserAgent = @"Jakarta Commons-HttpClient/3.1";
            request.ContentLength = 0;
            int timeout = 100000;
            request.Timeout = timeout;
            XmlDocument doc = new XmlDocument();
            try
            {

                doc = ReadXmlResponse(request.GetResponse());
                if (doc.GetElementsByTagName("returnstatus")[0].InnerXml == "Success")
                    return "1";
                else
                {
                    LogBO _log = new LogBO((new MessageTool()).GetType());
                    if (doc.GetElementsByTagName("message")[0].InnerXml == "对不起，您当前要发送的量大于您当前余额")
                    {
                        //SendMails("短信额度不够用，请充值", "Jeffrey.peng@5fang1.com");
                    }
                    else
                    {
                        _log.Error("短信发送异常，详细信息:" + doc.GetElementsByTagName("message")[0].InnerXml);
                    }
                }

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO((new MessageTool()).GetType());
                _log.Error("短信发送异常,异常：" + ex.Message + "\r\n --StackTrace" + ex.StackTrace);

            }
            return returns;
        }
        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string retXml = sr.ReadToEnd();
            sr.Close();
            //retXml = ReplaceInvalidChars(retXml, "");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }


        public bool SendMails(string fromAddress, string fromDisplayName, string toAddress, string cc, string subjectNm, string bodyAll, List<Attachment> attachments)
        {
            string _SmtpServer = "";
            //= WebConfigInfo.Instance.SmtpServer;
            string SendFailReason = String.Empty;
            LogBO logBO = new LogBO(typeof(MessageTool));
            if (String.IsNullOrEmpty(_SmtpServer))
            {
                #region if smtpServer is null throw
                SendFailReason = "Not set Smtp Server";
                logBO.Error(SendFailReason);
                #endregion
                return false;
            }
            else
            {
                MailMessage mailMessage = new MailMessage();
                try
                {

                    if (Equals(toAddress, null) || toAddress == "" || (string.IsNullOrEmpty(fromAddress)))
                    {
                        SendFailReason = "Invalid Email Address.";
                    }
                    else
                    {
                        mailMessage.From = new MailAddress(fromAddress, fromDisplayName, Encoding.UTF8);
                        string[] toAddressArr = toAddress.Split((";").ToCharArray());
                        for (int i = 0; i < toAddressArr.Length; i++)
                        {
                            mailMessage.To.Add(toAddressArr[i].ToString().Trim());
                        }

                    }
                    string[] listc = null;

                    if (cc != null && cc != "")
                    {
                        listc = cc.Split(',');
                        for (int i = 0; i < listc.Length; i++)
                        {
                            mailMessage.CC.Add(listc[i].ToString());
                        }

                    }
                }
                catch (Exception e)
                {
                    logBO.Error(e.Message);
                }
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (Attachment att in attachments)
                    {
                        att.Name = att.Name.Split(("\\").ToCharArray())[att.Name.Split(("\\").ToCharArray()).Length - 1].ToString();
                        mailMessage.Attachments.Add(att);
                    }
                }
                mailMessage.Subject = subjectNm;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Body = bodyAll;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;

                // send message.
                SmtpClient smtpClient = new SmtpClient(_SmtpServer);
                if (!string.IsNullOrEmpty(_SmtpServer))
                {
                    smtpClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                #region set time out
                string smtpTimeOut = ConfigurationManager.AppSettings["SmtpTimeout"];
                if (string.IsNullOrEmpty(smtpTimeOut))
                {
                    smtpClient.Timeout = 20000;// default: 2s
                }
                else
                {
                    int intTimeout = 0;
                    int.TryParse(smtpTimeOut, out intTimeout);
                    if (intTimeout > 0)
                    {
                        smtpClient.Timeout = intTimeout;
                    }
                }
                #endregion
                try
                {
                    smtpClient.Send(mailMessage);
                    return true;
                }
                catch (SmtpFailedRecipientsException exSmptFail)
                {
                    if (!string.IsNullOrEmpty(exSmptFail.Message))
                    {
                        SendFailReason = exSmptFail.Message;
                    }
                    logBO.Error(SendFailReason);
                    return false;
                }
                catch (InvalidOperationException exInvalid)
                {
                    if (!string.IsNullOrEmpty(exInvalid.Message))
                    {
                        SendFailReason = exInvalid.Message;
                    }
                    logBO.Error(SendFailReason);
                    return false;
                }
                catch (SmtpException exSmpt)
                {
                    if (!string.IsNullOrEmpty(exSmpt.Message))
                    {
                        SendFailReason = exSmpt.Message;
                    }
                    logBO.Error(SendFailReason);
                    return false;
                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(ex.Message))
                    {
                        SendFailReason = ex.Message;
                    }
                    logBO.Error(SendFailReason);
                    return false;
                }
            }
        }
    }
}

using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SPlatformService.Controllers
{
    /// <summary>
    /// 系统 API
    /// </summary>
    [RoutePrefix("SPWebAPI/System")]
    [TokenProjector]
    public class SystemController : ApiController
    {
        /// <summary>
        /// 获取文档类型列表，匿名
        /// </summary>
        /// <param name="parentHelpDocTypeId">父文档类型ID</param>
        /// <param name="enabele"></param>
        /// <returns></returns>
        [Route("GetHelpDocTypeList"), HttpGet, Anonymous]
        public ResultObject GetHelpDocTypeList(int parentHelpDocTypeId, bool enabele)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            List<HelpDocTypeVO> voList = sBO.FindHelpDocTypeList(parentHelpDocTypeId, enabele);            
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = voList };
        }

        /// <summary>
        /// 根据文档类型获取具体文档内容
        /// </summary>
        /// <param name="helpDocTypeId">文档类型</param>
        /// <param name="enabele"></param>
        /// <returns></returns>
        [Route("GetHelpDocByType"), HttpGet, Anonymous]
        public ResultObject GetHelpDocByType(int helpDocTypeId, bool enabele)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            HelpDocViewVO vo = sBO.FindHelpDocByType(helpDocTypeId, enabele);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "没有数据!", Result = null };
        }

        /// <summary>
        /// 根据文档类型名称获取文档内容
        /// </summary>
        /// <param name="helpDocTypeName">文档类型名称</param>
        /// <returns></returns>
        [Route("GetHelpDocByTypeName"), HttpGet, Anonymous]
        public ResultObject GetHelpDocByTypeName(string helpDocTypeName)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            HelpDocViewVO vo = sBO.FindHelpDocByTypeName(helpDocTypeName);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "没有数据!", Result = null };
        }

        /// <summary>
        /// 添加或更新文档内容
        /// </summary>
        /// <param name="helpDocVO">文档VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateHelpDoc"), HttpPost]
        public ResultObject UpdateHelpDoc([FromBody] HelpDocVO helpDocVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            SystemBO sBO = new SystemBO(uProfile);

            if (helpDocVO.HelpDocId < 1)
            {
                int hdId = sBO.HelpDocAdd(helpDocVO);
                if (hdId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = hdId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = sBO.HelpDocUpdate(helpDocVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="configVO">系统配置VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateConfig"), HttpPost]
        public ResultObject UpdateConfig([FromBody] ConfigVO configVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            SystemBO sBO = new SystemBO(uProfile);

            if (configVO != null)
            {
                int cId = sBO.ConfigUpdate(configVO);
                if (cId > 0)
                {
                    //更新 Config Cache
                    CacheSystemConfig.UpdateSystemConfig(configVO);
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {                
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        [Route("GetConfig"), HttpGet, Anonymous]
        public ResultObject GetConfig()
        {           
            SystemBO sBO = new SystemBO(new UserProfile());
            ConfigVO vo = sBO.FindConfig();
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "没有数据!", Result = null };
        }

        /// <summary>
        /// 添加投诉建议
        /// </summary>
        /// <param name="suggestionVO">投诉建议VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSuggestion"), HttpPost]
        public ResultObject UpdateSuggestion([FromBody] SuggestionVO suggestionVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            SystemBO sBO = new SystemBO(uProfile);

            if (suggestionVO != null)
            {
                suggestionVO.CreatedAt = DateTime.Now;
                suggestionVO.CustomerId = cProfile.CustomerId;
                int cId = sBO.SuggestionAdd(suggestionVO);
                if (cId > 0)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cId };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 回复投诉建议
        /// </summary>
        /// <param name="SuggestionVO">回复内容</param>
        /// <param name="SuggestionId">投诉建议ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ReplySuggestion"), HttpPost]
        public ResultObject ReplySuggestion([FromBody]SuggestionVO SuggestionVO, int SuggestionId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            SystemBO sBO = new SystemBO(new UserProfile());
            SuggestionVO vo = sBO.FindSuggestionById(SuggestionId);
            if (vo != null)
            {
                if (vo.CustomerId == 0)
                {
                    return new ResultObject() { Flag = 0, Message = "没有会员数据，无法回复!", Result = null };
                }
                MessageBO mBO = new MessageBO(new CustomerProfile());
                if (SuggestionVO.Description.Trim() != "")
                {
                    mBO.SendMessage("意见反馈回复", SuggestionVO.Description, vo.CustomerId, MessageType.SYS);
                    return new ResultObject() { Flag = 1, Message = "发送成功!", Result = null };
                }
                else {
                    return new ResultObject() { Flag = 0, Message = "请输入回复内容!", Result = null };
                }
            } 
            else
                return new ResultObject() { Flag = 0, Message = "没有数据!", Result = null };
        }

        /// <summary>
        /// 获取投诉建议信息
        /// </summary>
        /// <param name="suggestionId">投诉建议ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetSuggestion"), HttpGet]
        public ResultObject GetSuggestion(int suggestionId,string token)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            SuggestionVO vo = sBO.FindSuggestionById(suggestionId);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "没有数据!", Result = null };
        }

        /// <summary>
        /// 删除投诉建议
        /// </summary>
        /// <param name="suggestionId">投诉建议ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteSuggestion"), HttpGet]
        public ResultObject DeleteSuggestion(string suggestionId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(suggestionId))
                {
                    string[] sIdArr = suggestionId.Split(',');
                    bool isAllDelete = true;
                    SystemBO sBO = new SystemBO(new UserProfile());
                    for (int i = 0; i < sIdArr.Length; i++)
                    {
                        try
                        {
                            sBO.SuggestionDelete(Convert.ToInt32(sIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 上传图片，匿名
        /// </summary>
        /// <returns></returns>
        [Route("Upload"), HttpPost, Anonymous]
        public ResultObject Upload()
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1 || ext.IndexOf("exe") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //可以修改为网络路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    
                    hfc[0].SaveAs(PhysicalPath);


                    /*审核图片*/
                    try {
                        CardBO cBO = new CardBO(new CustomerProfile());
                        NameValueCollection poststring = new NameValueCollection();
                        string jsonStr = HttpHelper.UploadFileEx(PhysicalPath, "https://api.weixin.qq.com/wxa/img_sec_check?access_token=" + cBO.GetAccess_token(), poststring);
                        var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);

                        if (errorResult.errcode == 87014)
                        {
                            //删除非法图片
                            File.Delete(PhysicalPath);
                            imgPath = "~/UploadFolder/illegal.jpg";
                        }
                        else
                        {
                            imgPath = "~" + folder + newFileName;
                        }
                    } catch (Exception ex)
                    {
                        LogBO _log = new LogBO(typeof(SystemController));
                        string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                        _log.Error(strErrorMsg);
                    }
                    

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = imgPath };
                }
                catch (Exception ex)
                {
                    LogBO _log = new LogBO(typeof(SystemController));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = ex.Message };
                }
            }
            else
            {
                LogBO _log = new LogBO(typeof(SystemController));
                string strErrorMsg = "上传失败：文件为空";
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
            //string folder = ConfigInfo.Instance.UploadFolder + "Image/";
            //string imgPath = "";
            //if (hfc.Count > 0)
            //{
            //    FileInfo fi = new FileInfo(hfc[0].FileName);
            //    string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + fi.Extension;
            //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(folder)))
            //    {
            //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folder));
            //    }
            //    //imgPath = folder + hfc[0].FileName;
            //    imgPath = folder + newFileName;
            //    string PhysicalPath = HttpContext.Current.Server.MapPath(imgPath);
            //    hfc[0].SaveAs(PhysicalPath);
            //}
            //return new ResultObject() { Flag = 1, Message = "上传成功", Result = imgPath };
        }


        /// <summary>
        /// 上传视频，匿名
        /// </summary>
        /// <returns></returns>
        [Route("UploadVideo"), HttpPost, Anonymous]
        public ResultObject UploadVideo(int duration)
        {
            if (duration > 300)
            {
                return new ResultObject() { Flag = 0, Message = "最大只能上传5分钟长度的视频!", Result = null };
            }

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/Video/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //可以修改为网络路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;

                    hfc[0].SaveAs(PhysicalPath);

                    string ThumbnailImg = "";
                    try
                    {
                        //封面路径
                        string ThumbnailImgfolder = "/UploadFolder/VideoThumbnail/";
                        string ThumbnailImglocalPath = ConfigInfo.Instance.UploadFolder + ThumbnailImgfolder;
                        string ThumbnailImgFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";

                        //截取封面
                        Thumbnail Thumbnail = new Thumbnail();
                        Thumbnail.GenerateThumbnail(PhysicalPath, ThumbnailImglocalPath + ThumbnailImgFileName);
                        ThumbnailImg = ConfigInfo.Instance.APIURL + ThumbnailImgfolder + ThumbnailImgFileName;
                    }
                    catch
                    {

                    }

                    //网络路径
                    string url = ConfigInfo.Instance.APIURL + folder + newFileName;

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = url, Subsidiary = ThumbnailImg };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        // <summary>
        /// 微信公众号开发配置接收接口，匿名
        /// </summary>
        /// <returns></returns>
        [Route("CallbackURL"), HttpPost, Anonymous]
        public HttpResponseMessage CallbackURL()
        {
            try
            {
                System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();

                string xml = builder.ToString();
                string MsgID = "";
                string ArticleUrl = "";

                LogBO _log = new LogBO(this.GetType());
                _log.Info(xml);

                Match match = Regex.Match(xml, @"<MsgID>(?<text>[^\f\n\r\t\v]*)</MsgID>");
                MsgID = match.Groups["text"].Value.Trim();
                match = Regex.Match(xml, @"<ArticleUrl><!\[CDATA\[(?<text>[^\f\n\r\t\v]*)\]\]></ArticleUrl>");
                ArticleUrl = match.Groups["text"].Value.Trim();

                ArticleUrl = ArticleUrl.Replace("http:", "https:");

                /*
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Name == "MsgID")
                    {
                        MsgID = xe.InnerText;
                    }
                    if (xe.Name == "ArticleUrl")
                    {
                        ArticleUrl = xe.InnerText;
                    }
                    if (xe.Name == "ArticleUrlResult")
                    {
                        XmlNodeList ResultList = xe.ChildNodes;
                        foreach (XmlNode xr in ResultList)
                        {
                            XmlElement List = (XmlElement)xr;
                            XmlNodeList item = List.ChildNodes;
                            foreach (XmlNode xi in item)
                            {
                                XmlText it = (XmlText)xi;
                                if (it.Name == "ArticleUrl")
                                {
                                    ArticleUrl = it.InnerText;
                                }
                            }
                        }
                    }
                }*/

                if (MsgID != "")
                {
                    CardBO cBO = new CardBO(new CustomerProfile());
                    List<MediaVO> MediaVO = cBO.FindMediaByCondition("msg_id='" + MsgID + "'");
                    if (MediaVO.Count > 0)
                    {
                        MediaVO[0].Status = 1;
                        MediaVO[0].ArticleUrl = ArticleUrl;
                        cBO.UpdateMedia(MediaVO[0]);
                    }
                }

                _log.Info(MsgID + ":" + ArticleUrl);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_CallbackURL));
                string strErrorMsg = ex.Message;
                _log.Error(strErrorMsg);
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage { Content = new StringContent("", Encoding.GetEncoding("UTF-8"), "text/plain") };
            return responseMessage;
        }

        // <summary>
        /// 活动星选客服消息接口，匿名
        /// </summary>
        /// <returns></returns>
        [Route("WX_MessageURL"), HttpPost, Anonymous]
        public HttpResponseMessage WX_MessageURL()
        {
            //string content = System.Web.HttpContext.Current.Request["echostr"];
            string content = "";
            try
            {
                LogBO _log = new LogBO(this.GetType());

                System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();

                string jsonstr = builder.ToString();
                _log.Info("乐聊名片客服消息:" + jsonstr);

                dynamic resultContent = JsonConvert.DeserializeObject(jsonstr, new WX_Msg().GetType());
                _log.Info("乐聊名片客服消息:"+ resultContent.MsgType);

                if (resultContent.MsgType == "miniprogrampage")
                {
                    WX_JSSDK jssdk = new WX_JSSDK();
                    jssdk.appId = "wx83bf84d3847abf2f";
                    jssdk.appSecret = "dcdddcd1f79943500e2fb210b1684185";

                    string PagePath = resultContent.PagePath;
                    Match match = Regex.Match(PagePath, @"PartyID=(?<text>[\d]*)&CustomerId=(?<text2>[\d]*)&Type=(?<text3>[\d]*)");
                    int PartyID = Convert.ToInt32(match.Groups["text"].Value.Trim());
                    int CustomerId = Convert.ToInt32(match.Groups["text2"].Value.Trim());
                    int Type = Convert.ToInt32(match.Groups["text3"].Value.Trim());

                    string img = "";
                    string text = "";
                    if (Type == 0)
                    {
                        //关注公众号
                        CardBO cBO = new CardBO(new CustomerProfile(), 4);
                        CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                        CardPartyVO cvo = cBO.FindPartyById(PartyID);

                        if (cvo != null && cvo.SignupConditions == 1 && cvo.AuthorizationID > 0)
                        {
                            WxThirdPartyBO wBO = new WxThirdPartyBO(new CustomerProfile());
                            AuthorizationVO aVO = wBO.FindAuthorizationByID(cvo.AuthorizationID);
                            if (aVO != null && aVO.qrcode_url != "")
                            {
                                img = aVO.qrcode_url;
                                text = "长按二维码关注公众号，并在公众号回复【" + cvo.SignupKeyWord + "】！";
                                content += "发送图片：" + jssdk.sendImg(img, resultContent.FromUserName);
                                jssdk.sendText(text, resultContent.FromUserName);
                            }
                        }
                    }

                    if (Type == 1)
                    {
                        //咨询客服
                        img = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/qiyekefu1.jpg";
                        text = "请长按二维码进行识别,添加我们客服的企业微信进行咨询";
                        content += "发送图片：" + jssdk.sendImg(img, resultContent.FromUserName);
                        jssdk.sendText(text, resultContent.FromUserName);
                    }

                    if (Type == 2)
                    {
                        //授权公众号
                        WxThirdPartyBO wBO = new WxThirdPartyBO(new CustomerProfile());
                        string url = wBO.GetAuthorizationUrl(CustomerId);
                        jssdk.sendText("<a href=\\\"" + url + "\\\">点击授权公众号（链接有效期10分钟）</a>", resultContent.FromUserName);
                    }

                    if (Type == 3)
                    {
                        //厂拉拉app下载
                        jssdk.sendText("<a href=\\\"https://send.api.fiaohi.com.cn/download/index/register?invite_code=587353\\\">点击以下链接完成注册既能开始抽奖：https://send.api.fiaohi.com.cn/download/index/register?invite_code=587353</a>", resultContent.FromUserName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_CallbackURL));
                string strErrorMsg = ex.Message;
                _log.Error(strErrorMsg);
                content += "错误：" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage { Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "text/plain") };
            return responseMessage;
        }

        // <summary>
        /// 活动星选第三方授权事件接收URL，匿名
        /// </summary>
        /// <returns></returns>
        [Route("WX_ThirdPartyURL"), HttpPost, Anonymous]
        public HttpResponseMessage WX_ThirdPartyURL(string msg_signature = "", string timestamp = "", string nonce = "")
        {
            string content = "success";

            LogBO _log = new LogBO(this.GetType());

            System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            string jsonstr = builder.ToString();

            //公众平台上开发者设置的token, appID, EncodingAESKey
            string sToken = "choujiang2021";
            string sAppID = "wxdaa115a1853a4f00";
            string sEncodingAESKey = "HXuVgieWATD756gj318A1BaYSVHvxWH76QJgTLQRVSN";

            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

            string sReqMsgSig = msg_signature;
            string sReqTimeStamp = timestamp;
            string sReqNonce = nonce;
            string sReqData = jsonstr;
            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            if (ret != 0)
            {
                _log.Info("第三方授权事件(错误):" + ret);
            }

            if(sMsg.IndexOf("ComponentVerifyTicket") > -1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sMsg);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                string xAppId = "";
                string xCreateTime = "";
                string xComponentVerifyTicket = "";

                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Name == "AppId")
                    {
                        xAppId = xe.InnerText;
                    }
                    if (xe.Name == "CreateTime")
                    {
                        xCreateTime = xe.InnerText;
                    }
                    if (xe.Name == "ComponentVerifyTicket")
                    {
                        xComponentVerifyTicket = xe.InnerText;
                    }
                }
                if (xAppId != "" && xComponentVerifyTicket != "")
                {
                    WxThirdPartyBO cBO = new WxThirdPartyBO(new CustomerProfile());
                    List<TicketVO> tVO = cBO.FindTicketByCondition("AppId = '" + xAppId + "'");
                    if (tVO.Count == 0)
                    {
                        TicketVO TicketVO = new TicketVO();
                        TicketVO.TicketID = 0;
                        TicketVO.AppId = xAppId;
                        TicketVO.CreateTime = xCreateTime;
                        TicketVO.ComponentVerifyTicket = xComponentVerifyTicket;
                        TicketVO.CreatedAt = DateTime.Now;
                        cBO.AddTicket(TicketVO);
                    }
                    else
                    {
                        tVO[0].ComponentVerifyTicket = xComponentVerifyTicket;
                        tVO[0].CreateTime = xCreateTime;
                        tVO[0].CreatedAt = DateTime.Now;
                        cBO.UpdateTicket(tVO[0]);
                    }
                }
            }

            if (sMsg.IndexOf("InfoType") > -1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sMsg);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                string AppId = "";
                string InfoType = "";

                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Name == "AppId")
                    {
                        AppId = xe.InnerText;
                    }
                    if (xe.Name == "InfoType")
                    {
                        InfoType = xe.InnerText;
                    }
                }

                if(InfoType== "unauthorized")//取消授权
                {
                    string AuthorizerAppid = "";
                    foreach (XmlNode xn in nodes)
                    {
                        XmlElement xe = (XmlElement)xn;
                        if (xe.Name == "AuthorizerAppid")
                        {
                            AuthorizerAppid = xe.InnerText;
                        }
                    }
                    WxThirdPartyBO wBO = new WxThirdPartyBO(new CustomerProfile());
                    List<AuthorizationVO> aVO = wBO.FindAuthorizationByCondition("authorizer_appid='" + AuthorizerAppid + "' and AppId='" + AppId + "'");
                    if (aVO.Count > 0)
                    {
                        aVO[0].Status = 0;
                        wBO.UpdateAuthorization(aVO[0]);
                    }
                }
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage { Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "text/plain") };
            return responseMessage;
        }


        // <summary>
        /// 活动星选第三方消息与事件接收URL，匿名
        /// </summary>
        /// <returns></returns>
        [Route("WX_ThirdPartyMessageURL"), HttpPost, Anonymous]
        public HttpResponseMessage WX_ThirdPartyMessageURL(string APPID = "", string msg_signature = "", string timestamp = "", string nonce = "")
        {
            string content = "success";
            LogBO _log = new LogBO(this.GetType());

            System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            string jsonstr = builder.ToString();

            _log.Info("第三方消息事件:" + APPID);
            _log.Info("第三方消息事件（密文）:" + jsonstr);

            //公众平台上开发者设置的token, appID, EncodingAESKey
            string sToken = "choujiang2021";
            string sAppID = "wxdaa115a1853a4f00";
            string sEncodingAESKey = "HXuVgieWATD756gj318A1BaYSVHvxWH76QJgTLQRVSN";

            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

            string sReqMsgSig = msg_signature;
            string sReqTimeStamp = timestamp;
            string sReqNonce = nonce;
            string sReqData = jsonstr;
            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            if (ret != 0)
            {
                _log.Info("第三方消息事件(错误):" + ret);
            }
            _log.Info("第三方消息事件（明文）:" + sMsg);

            if (sMsg.IndexOf("MsgId") > -1&& sMsg.IndexOf("Content") > -1 && sMsg.IndexOf("MsgType") > -1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sMsg);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                string ToUserName = "";
                string FromUserName = "";
                string CreateTime = "";
                string MsgType = "";
                string Content = "";
                string MsgId = "";

                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Name == "MsgType")
                    {
                        MsgType = xe.InnerText;
                    }
                }
                if(MsgType== "text")
                {
                    foreach (XmlNode xn in nodes)
                    {
                        XmlElement xe = (XmlElement)xn;
                        if (xe.Name == "ToUserName")
                        {
                            ToUserName = xe.InnerText;
                        }
                        if (xe.Name == "FromUserName")
                        {
                            FromUserName = xe.InnerText;
                        }
                        if (xe.Name == "CreateTime")
                        {
                            CreateTime = xe.InnerText;
                        }
                        if (xe.Name == "Content")
                        {
                            Content = xe.InnerText;
                        }
                        if (xe.Name == "MsgId")
                        {
                            MsgId = xe.InnerText;
                        }
                    }
                    string xml = "";
                    try
                    {
                        //回复消息
                        WxThirdPartyBO wBO = new WxThirdPartyBO(new CustomerProfile());
                        List<AuthorizationVO> aVO = wBO.FindAuthorizationByCondition("authorizer_appid='" + APPID + "' and AppId='" + sAppID + "'");
                        if (aVO.Count > 0)
                        {
                            CardBO cBO = new CardBO(new CustomerProfile(), 4);
                            List<CardPartyVO> pVO = cBO.FindPartybycondtion("AuthorizationID="+ aVO[0].AuthorizationID+ " and Status=1 and SignupConditions=1 and ConditionsQR<>'' and SignUpTime>now() and PartyLuckDrawStatus=0");
                            foreach(CardPartyVO item in pVO){
                                if(item.SignupKeyWord== Content)
                                {
                                    xml = "<xml>";
                                    xml += "<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>";
                                    xml += "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";
                                    xml += "<CreateTime>" + (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + "</CreateTime>";
                                    xml += "<MsgType><![CDATA[image]]></MsgType>";
                                    xml += "<Image>";
                                    xml += "<MediaId><![CDATA[" + wBO.uploadimgGetmediaId(item.ConditionsQR, APPID) + "]]></MediaId>";
                                    xml += "</Image>";
                                    xml += "</xml>";

                                    string eMsg = "";  //加密之后的密文
                                    int eret = 0;
                                    eret = wxcpt.EncryptMsg(xml, sReqTimeStamp, sReqNonce, ref eMsg);
                                    if (eret != 0)
                                    {
                                        _log.Info("第三方消息事件(回复错误):" + eret);
                                    }
                                    else
                                    {
                                        content = eMsg;
                                        _log.Info("第三方消息事件（回复）:" + eMsg);
                                    }
                                }
                            }
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        _log.Info("第三方消息事件（回复错误）:" + ex);
                    }
                    
                }
                
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage { Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "text/plain") };
            return responseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">保存路径 例如customer 1的合同："CustomerFile/1/ContractBox</param>
        /// <returns></returns>
        [Route("UploadWithPath"), HttpPost, Anonymous]
        public ResultObject UploadWithPath(string path)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/";
            if (!string.IsNullOrEmpty(path))
            {
                folder += path + "/";
            }
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //可以修改为网络路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    imgPath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);
                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = imgPath };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
            //string folder = ConfigInfo.Instance.UploadFolder + "Image/";
            //string imgPath = "";
            //if (hfc.Count > 0)
            //{
            //    FileInfo fi = new FileInfo(hfc[0].FileName);
            //    string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + fi.Extension;
            //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(folder)))
            //    {
            //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folder));
            //    }
            //    //imgPath = folder + hfc[0].FileName;
            //    imgPath = folder + newFileName;
            //    string PhysicalPath = HttpContext.Current.Server.MapPath(imgPath);
            //    hfc[0].SaveAs(PhysicalPath);
            //}
            //return new ResultObject() { Flag = 1, Message = "上传成功", Result = imgPath };
        }
        /// <summary>
        /// 发送验证码，匿名
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        [Route("SendPassCodeMsg"), HttpGet, Anonymous]
        public ResultObject SendPassCodeMsg(string phone)
        {

            MessageTool uBO = new MessageTool();
            if (string.IsNullOrEmpty(phone))
                return new ResultObject() { Flag = 0, Message = "手机号码不能为空!", Result = null };


            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int tmp = ra.Next(1000001, 9999999);
            string code = tmp.ToString().Substring(1, 6);
            
            string msgContent = "尊敬的客户：您的验证码为：" + code + "，请尽快校验。温馨提示：请妥善保管，不要随意泄露给他人。【众销乐-资源共享众包销售平台】";

            string result = MessageTool.SendMobileMsg(msgContent, phone);
            //code = "111111";
            //string result = "1";

            if (result == "1")
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = code };
            else
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };

        }
        /// <summary>
        /// 新发送验证码，匿名
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        [Route("NewSendPassCodeMsg"), HttpGet, Anonymous]
        public ResultObject NewSendPassCodeMsg(string phone)
        {

            MessageTool uBO = new MessageTool();
            if (string.IsNullOrEmpty(phone))
                return new ResultObject() { Flag = 0, Message = "手机号码不能为空!", Result = null };


            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int tmp = ra.Next(1000001, 9999999);
            string code = tmp.ToString().Substring(1, 6);

            string msgContent = "尊敬的客户：您的验证码为：" + code + "，请尽快校验。温馨提示：请妥善保管，不要随意泄露给他人。【众销乐-资源共享众包销售平台】";

            string result = MessageTool.SendMobileMsg(msgContent, phone);

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            context.Session["code"] = code;
            context.Session["phone"] = phone;
            //code = "111111";
            //string result = "1";

            if (result == "1")
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = context.Session.SessionID };
            else
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = context.Session.SessionID };

        }
        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="msgContent">发送内容</param>
        /// <param name="phone">手机号码</param>
        /// <param name="title">短信主题</param>
        /// <param name="customerId">会员Id</param>
        /// <param name="IsSave">是否使用保存，验证码等敏感短信就不保存吧</param>
        /// <returns></returns>        
        [Route("SendMobileMsg"), HttpGet, Anonymous]
        public ResultObject SendMobileMsg(string msgContent, string phone, string title, int customerId, bool IsSave)
        {

            MessageTool uBO = new MessageTool();
            if (string.IsNullOrEmpty(phone))
                return new ResultObject() { Flag = 0, Message = "手机号码不能为空!", Result = null };
            if (string.IsNullOrEmpty(phone))
                return new ResultObject() { Flag = 0, Message = "短信内容不能为空!", Result = null };

            string result = MessageTool.SendMobileMsg(msgContent, phone);

            if (customerId > 0 && IsSave)
            {
                MessageVO mvo = new MessageVO();
                mvo.Message = msgContent;
                mvo.SendTo = customerId;
                mvo.SendAt = DateTime.Now;
                mvo.MessageTypeId = 1;
                if (result == "1")
                    mvo.Status = 1;
                else
                    mvo.Status = 0;
                mvo.Title = title;

                MessageBO mBO = new MessageBO(new CustomerProfile());
                mBO.AddMessage(mvo);
            }

            if (result == "1")
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };

        }

        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="systemMessageVO">系统消息VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddSystemMessage"), HttpPost]
        public ResultObject AddSystemMessage([FromBody] SystemMessageVO systemMessageVO, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            SystemBO uBO = new SystemBO(uProfile);

            systemMessageVO.SendAt = DateTime.Now;

            int systemMessageId = uBO.AddSystemMessage(systemMessageVO);
            if (systemMessageId > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = systemMessageId };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

        }

        /// <summary>
        /// 获取系统消息
        /// </summary>
        /// <param name="systemMessageId">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetSystemMessage"), HttpGet]
        public ResultObject GetSystemMessage(int systemMessageId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            SystemBO uBO = new SystemBO(uProfile);

            SystemMessageViewVO vo = uBO.FindSystemMessageById(systemMessageId);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 删除系统消息
        /// </summary>
        /// <param name="systemMessageId">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteSystemMessage"), HttpGet]
        public ResultObject DeleteSystemMessage(string systemMessageId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;

            SystemBO uBO = new SystemBO(uProfile);

            try
            {
                if (!string.IsNullOrEmpty(systemMessageId))
                {
                    string[] messageIdArr = systemMessageId.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.DeleteSystemMessage(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }
        /// <summary>
        /// 个推
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="clientId">目标cid</param>
        /// <returns></returns>
        [Route("SendPushMessage"), HttpGet, Anonymous]
        public ResultObject SendPushMessage(string message, string clientId)
        {
            Utilities.PushMessageToSingle(message, clientId);
            return new ResultObject() { Flag = 1, Message = "", Result = null };
        }

        /// <summary>
        /// 导入采集的1688数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="Password">导入密码</param>
        /// <param name="isimg">是否导入图片</param>
        /// <param name="isStatus">是否通过资料认证</param>
        /// <returns></returns>
        [Route("ImportData1688"), HttpPost, Anonymous]
        public string ImportData1688([FromBody] List<C1688VO> condition,string Password,string isimg,string isStatus)
        {
            CustomerProfile uProfile = new CustomerProfile();
            string truepass = Utilities.GetMD5("yue-er8");
            int SuccessNum = 0;
            int FailNum = 0;
            BusinessBO bBO = new BusinessBO(new CustomerProfile());
            CityBO cBO = new CityBO(new CustomerProfile());

            if (truepass == Password)
            {
                if (condition.Count > 0)
                {
                    for(int i=0;i< condition.Count; i++)
                    {
                        string account = condition[i].MobilePhone;
                        string password = Utilities.MakePassword(8);
                        C1688VO c1688 = condition[i];


                        if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
                        {
                            CustomerBO uBO = new CustomerBO(uProfile);
                            //判断LoginName 和 CustomerName是否重复
                            CustomerVO customerVO = new CustomerVO();
                            customerVO.CustomerCode = uBO.GetCustomerCode();
                            customerVO.CustomerAccount = account;
                            //customerVO.CustomerName = account.Substring(0,7)+"****";
                            customerVO.CustomerName = c1688.Contacts;
                            customerVO.Phone = account;
                            customerVO.Password = Utilities.GetMD5(password);
                            customerVO.Status = 1;
                            customerVO.CreatedAt = DateTime.Now;
                            if (uBO.IsCustomerExist(customerVO))
                            {
                                FailNum++;
                                continue;
                            }
                            int customerId = uBO.Add(customerVO);
                            if (customerId > 0)
                            {
                                //通过认证，IM注册，存在则不添加，不存在则添加
                                IMBO imBO = new IMBO(new CustomerProfile());
                                imBO.RegisterIMUser(customerId, customerVO.CustomerCode, "$" + customerVO.CustomerCode, customerVO.CustomerName);

                                BusinessVO businessVO = new BusinessVO();
                                List<BusinessCategoryVO> businessCategoryVOList = new List<BusinessCategoryVO>();
                                List<TargetCategoryVO> targetCategoryVOList = new List<TargetCategoryVO>();
                                List<TargetCityVO> targetCityVOList = new List<TargetCityVO>();
                                List<BusinessIdcardVO> businessIdcardVOList = new List<BusinessIdcardVO>();

                                businessVO.BusinessId = 0;
                                businessVO.CustomerId = customerId;
                                businessVO.CompanyName = c1688.CorporateName;
                                businessVO.CompanyTel = c1688.Telephone;
                                if (isimg == "Y") {
                                    try
                                    {
                                        string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                                        string imgPath = "";
                                        string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                                        //可以修改为网络路径
                                        string localPath = ConfigInfo.Instance.UploadFolder + folder;
                                        if (!Directory.Exists(localPath))
                                        {
                                            Directory.CreateDirectory(localPath);
                                        }
                                        string PhysicalPath = localPath + newFileName;
                                        imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                                        WebRequest wreq = WebRequest.Create(c1688.Pic);
                                        HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                                        Stream s = wresp.GetResponseStream();
                                        System.Drawing.Image img;
                                        img = System.Drawing.Image.FromStream(s);
                                        img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存
                                        businessVO.CompanyLogo = imgPath;
                                    }
                                    catch
                                    {

                                    }
                                    
                                }
                                businessVO.CreatedAt = DateTime.Now;
                                businessVO.MainProducts = c1688.Details;
                                businessVO.Address = c1688.Address;
                                businessVO.CompanyType = c1688.Model;


                                List<CityVO> CVO= cBO.FindCityByName(c1688.City);
                                if (CVO.Count > 0) {
                                    businessVO.CityId = CVO[0].CityId;
                                    TargetCityVO tVO = new TargetCityVO();
                                    tVO.CityId = CVO[0].CityId;
                                    tVO.BusinessId = 0;
                                    tVO.TargetCityId = 0;
                                    targetCityVOList.Add(tVO);
                                }
                                businessVO.ProductDescription = c1688.Products;
                                if (isStatus == "Y")
                                {
                                    businessVO.Status = 1;
                                }
                                else {
                                    businessVO.Status = 0;
                                }
                                
                                bBO.Add(businessVO, businessCategoryVOList, targetCategoryVOList, targetCityVOList, businessIdcardVOList);
                                SuccessNum++;
                            }
                            else
                                FailNum++;
                        }
                        else
                        {
                            FailNum++;
                        }
                    }
                }
                else {
                    return "不能提交空数据，请先采集数据";
                }
                return "成功添加了"+ SuccessNum + "个会员，因重复数据或空手机号被拒绝导入数据"+ FailNum+"条";
            }
            else {
                return "密码错误";
            }
        }
    }
}

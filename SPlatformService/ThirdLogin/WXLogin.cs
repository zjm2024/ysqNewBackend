using Newtonsoft.Json;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.WebConfigInfo;
using System;
using System.Configuration;

namespace SPlatformService.ThirdLogin
{
    public class WXLogin
    {
        //开放平台
        public string AppID = ConfigurationManager.AppSettings["WXAppId"].ToString();
        public string AppSecret = ConfigurationManager.AppSettings["WXAppSecret"].ToString();

        //众销乐公众号
        public string AppID_gzh = "wx9a65d7becbbb017a";
        public string AppSecret_gzh = "2f75313a696eac6cef7a393641ff7a68";

        //乐聊名片订阅号
        public string AppID_leliao = "wx67216e7509d25ecc";
        public string AppSecret_leliao = "a60d0b6b0987e80b95f4bc1110ea26a3";

        /// <summary>
        /// 获取微信Code
        /// </summary>
        /// <param name="redirectUrl">返回的登录地址，要进行Server.Un编码</param>
        /// <param name="type">1 扫码登陆，2 公众号登陆</param>
        public string GetWeiXinCode(int type,string redirectUrl)
        {
            var r = new Random();
            //微信登录授权
            //string url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + appId + "&redirect_uri=" + redirectUrl +"&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
            //微信OpenId授权
            //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appId + "&redirect_uri=" + redirectUrl +"&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
            //微信用户信息授权
            var url = "";
            //if (isWap)
            //{
            //    url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appId + "&redirect_uri=" +
            //          redirectUrl + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
            //}
            //else
            //{


            // https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxa8b1f365a57c5633&redirect_uri=http%3a%2f%2fwww.zhongxiaole.net%2fThirdLogin%2fWXLoginPage.aspx&response_type=code&scope=snsapi_login&state=STATE&connect_redirect=1#wechat_redirect
            if (type == 1)
            {
                url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + AppID + "&redirect_uri=" + redirectUrl +
                        "&response_type=code&scope=snsapi_login&state=1#wechat_redirect";
            }
            else {
                url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppID_gzh + "&redirect_uri=" + redirectUrl + "&response_type=code&scope=snsapi_userinfo&state=2#wechat_redirect";
            }
            
            //}
            return url;

        }

        /// <summary>
        /// 直接获取普通access_token
        /// </summary>
        public WeiXinAccessTokenResultDYH GetWeiXinAccessTokenDYH()
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppID + "&secret=" + AppSecret;

            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            return result;
        }

        /// <summary>
        /// 通过code获取access_token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public WeiXinAccessTokenResult GetWeiXinAccessToken(int type,string code)
        {
            string url;
            if (type==1)
            {
                url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppID + "&secret=" + AppSecret +
                 "&code=" + code + "&grant_type=authorization_code";
            }else if(type == 2)
            {
                url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppID_gzh + "&secret=" + AppSecret_gzh +
                 "&code=" + code + "&grant_type=authorization_code";
            }else
            {
                url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + AppID_leliao + "&secret=" + AppSecret_leliao +
                 "&code=" + code + "&grant_type=authorization_code";
            }
            
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResult();
            try
            {
                
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
            }
            catch (Exception err)
            {
                LogBO _log = new LogBO(typeof(WXLogin));
                string strErrorMsg = "Message:" + err.Message.ToString() + "\r\n  Stack :" + err.StackTrace + " \r\n Source :" + err.Source + " \r\n jsonStr :" + jsonStr;
                _log.Error(strErrorMsg);
            }
            return result;
        }
        /// <summary>
        /// 拉取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WeiXinHelper.WeiXinUserInfoResult GetWeiXinUserInfo(string accessToken, string openId)
        {
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + accessToken + "&openid=" + openId + "⟨=zh_CN";
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinHelper.WeiXinUserInfoResult();

            try
            {
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorMsg = errorResult;
                    result.Result = false;
                }
                else
                {
                    var userInfo = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinUserInfo>(jsonStr);
                    result.UserInfo = userInfo;
                    result.Result = true;
                }
            }
            catch (Exception err)
            {
                LogBO _log = new LogBO(typeof(WXLogin));
                string strErrorMsg = "Message:" + err.Message.ToString() + "\r\n  Stack :" + err.StackTrace + " \r\n Source :" + err.Source + " \r\n jsonStr :" + jsonStr;
                _log.Error(strErrorMsg);
            }

            
            return result;
        }
    }

    
}
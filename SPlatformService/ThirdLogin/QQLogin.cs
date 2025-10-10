using Newtonsoft.Json;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace SPlatformService.ThirdLogin
{
    public class QQLogin
    {
        string AppID = ConfigurationManager.AppSettings["QQAppId"].ToString();
        string AppKey = ConfigurationManager.AppSettings["QQAppKey"].ToString();
        //string Return_url = "QQLoginSuccess.aspx";
        public string Authorize(string returnURL)
        {
            string state = new Random(100000).Next(99, 99999).ToString();//随机数            
            string url = string.Format("https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}", AppID, returnURL, state);
            string str = "<script> location.href='" + url + "'</script>";
            return str;
        }

        public User_info Get_User(string code, string returnURL)
        {
            LogBO _bo = new LogBO(this.GetType());
            User_info ui = new User_info();
            try
            {
                string state = new Random(100000).Next(99, 99999).ToString();//随机数

                string url = string.Format("https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}&state={4}", AppID, AppKey, code, returnURL, state);
                string uu = HttpHelper.HtmlFromUrlGet(url);//处理http请求帮助类
                string code1 = uu.Split('&')[0].Split('=')[1].ToString();//获得access_token

                //根基access_token获取用户唯一OpenID
                string url_me = string.Format("https://graph.qq.com/oauth2.0/me?access_token={0}", code1);
                string callback = HttpHelper.HtmlFromUrlGet(url_me);//这里获取的
                //_bo.Info("Get_User QQ callback 1" + callback);
                callback = callback.Substring(callback.IndexOf('(') + 1, (callback.IndexOf(')') - callback.IndexOf('(') - 1)).Trim();
                //_bo.Info("Get_User QQ callback 2" + callback);
                QQOpen jsonP = JsonConvert.DeserializeObject<QQOpen>(callback);//Newtonsoft.Json.dll 4.0或4.5版本
                string OpenID = jsonP.openid;//获取用户唯一的OpenID  

                ui.OpenID = OpenID;
                try
                {
                    //根据OpenID获取用户信息 可以显示更多 用的就几个 需要的可以自己在下面加
                    string getinfo = string.Format("https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}", code1, AppID, OpenID);

                    string user = HttpHelper.HtmlFromUrlGet(getinfo);
                    //_bo.Info("Get_User QQ user info " + user);
                    QQInfo info = JsonConvert.DeserializeObject<QQInfo>(user);
                    ui.Name = info.nickname;
                    ui.img_qq100 = info.figureurl_qq_1;
                    ui.img_qq50 = info.figureurl_qq_2;
                    ui.city = info.city;
                    ui.year = info.year;
                }
                catch (Exception e)
                {
                    //_bo.Error("Get_User QQ messs" + e.Message + "\r\n" + e.StackTrace);
                }
            }
            catch (Exception e)
            {

                //_bo.Error("Get_User QQ messs" + e.Message + "\r\n" + e.StackTrace);
                return null;

            }
            //ui.Type = 1;
            return ui;

        }

    }
}
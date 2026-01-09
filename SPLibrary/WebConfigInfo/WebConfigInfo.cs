using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;

namespace SPLibrary.CoreFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigInfo
    {
        #region Field
        /// <summary>
        /// Name of the DataBase
        /// </summary>
        private string _DbName;
        /// <summary>
        /// String of the connection.
        /// </summary>
        private string _DefaultConnectionString;
        /// <summary>
        /// Folder of the sys.
        /// </summary>
        private string _ServerDirectory;
        /// <summary>
        /// Folder of the upload files.
        /// </summary>
        private string _UploadFolder;
        /// <summary>
        /// AsyncPostBackTimeout for long running page
        /// </summary>
        private int _AsyncPostBackTimeout;

        private int _DBConnectionTimeOut;

        private string _APIURL;
        private string _APIURLByHttp;
        private string _BCAPIURL;
        private string _BCAPIURLByHttp;
        private string _UploadDomain;
        private string _NoImg;
        private string _LOGO;

        //环信Key信息
        private string _IMOrgName;
        private string _IMAppName;
        private string _IMClientId;
        private string _IMClientSecret;
        private string _IMHXURL;

        //企业支付到零钱
        private string _PayAppId;
        private string _PayMCHID;
        private string _PayKEY;
        private string _PayAPPSECRET;
        private string _PaySSLCERT_PASSWORD;
        



        //个推Key信息
        private string _GTAPPID;
        private string _GTAPPKEY;
        private string _GTMASTERSECRET;
        private string _GTHOST;

        private static ConfigInfo _Instance = null;
        private static readonly object _Padlock = new object();

        #endregion

        #region Properties      

        //openai的apikey
        private string _OpenApiKey;

        /// <summary>
        /// Gets the name of the db.
        /// </summary>
        /// <value>The name of the db.</value>
        public string DbName
        {
            get { return _DbName; }
            set { _DbName = value; }
        }

        /// <summary>
        /// Gets the default connection string.
        /// </summary>
        /// <value>The default connection string.</value>
        public string DefaultConnectionString
        {
            set { _DefaultConnectionString = value; }
            get
            {
                return _DefaultConnectionString;
            }
        }
        /// <summary>
        /// Gets the server directory.
        /// </summary>
        /// <value>The server directory.</value>
        public string ServerDirectory
        {
            get
            {
                return _ServerDirectory;
            }
            set
            {
                _ServerDirectory = value;
            }
        }

        /// <summary>
        /// Gets the upload folder.
        /// </summary>
        /// <value>The upload folder.</value>
        public string UploadFolder
        {
            get
            {
                return _UploadFolder;
            }
        }

        /// <summary>
        /// AsyncPostBackTimeout for long running page
        /// </summary>
        public int AsyncPostBackTimeout
        {
            get
            {
                return _AsyncPostBackTimeout;
            }
        }

        public int DBConnectionTimeOut
        {
            get
            {
                return _DBConnectionTimeOut;
            }
        }

        public string APIURL
        {
            get
            {
                return _APIURL;
            }
        }
        public string APIURLByHttp
        {
            get
            {
                return _APIURLByHttp;
            }
        }

        public string BCAPIURL
        {
            get
            {
                return _BCAPIURL;
            }
        }

        public string BCAPIURLByHttp
        {
            get
            {
                return _BCAPIURLByHttp;
            }
        }

        private string _SITEURL;
        public string SITEURL
        {
            get
            {
                return _SITEURL;
            }
        }

        private string _SITEURLByHttp;
        public string SITEURLByHttp
        {
            get
            {
                return _SITEURLByHttp;
            }
        }

        public string UploadDomain
        {
            get
            {
                return _UploadDomain;
            }
        }

        public string NoImg
        {
            get
            {
                return _NoImg;
            }
        }

        public string LOGO
        {
            get
            {
                return _LOGO;
            }
        }

        public string IMOrgName
        {
            get
            {
                return _IMOrgName;
            }
        }
        public string IMAppName
        {
            get
            {
                return _IMAppName;
            }
        }
        public string IMClientId
        {
            get
            {
                return _IMClientId;
            }
        }
        public string IMClientSecret
        {
            get
            {
                return _IMClientSecret;
            }
        }
        public string IMHXURL
        {
            get
            {
                return _IMHXURL;
            }
        }


        public string PayAppId
        {
            get
            {
                return _PayAppId;
            }
        }
        public string PayMCHID
        {
            get
            {
                return _PayMCHID;
            }
        }
        public string PayKEY
        {
            get
            {
                return _PayKEY;
            }
        }
        public string PayAPPSECRET
        {
            get
            {
                return _PayAPPSECRET;
            }
        }
               public string PaySSLCERT_PASSWORD
        {
            get
            {
                return _PaySSLCERT_PASSWORD;
            }
        }
   


        //个推
        public string GTAPPID
        {
            get
            {
                return _GTAPPID;
            }
        }
        public string GTAPPKEY
        {
            get
            {
                return _GTAPPKEY;
            }
        }
        public string GTMASTERSECRET
        {
            get
            {
                return _GTMASTERSECRET;
            }
        }
        public string GTHOST
        {
            get
            {
                return _GTHOST;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WebConfigInfo"/> class.
        /// </summary>
        private ConfigInfo()
        {

        }

        #endregion

        public string OpenApiKey
        {
            get
            {
                return _OpenApiKey;
            }
        }


        /// <summary>
        /// Initials this instance.
        /// </summary>
        internal static void Initial()
        {
            _Instance = new ConfigInfo();

            if (ConfigurationManager.ConnectionStrings["defaultConnectionString"] != null)
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["defaultConnectionString"].Name))
                {
                    _Instance._DbName = ConfigurationManager.ConnectionStrings["defaultConnectionString"].Name;
                }
                if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString))
                {
                    _Instance._DefaultConnectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
                }
            }

            _Instance._UploadFolder = GetAppSettings("UploadFolder", "~/UploadFolder/");
            _Instance._APIURL = GetAppSettings("RESTAPIURL", "");
            _Instance._APIURLByHttp = GetAppSettings("RESTAPIURLByHttp", "");
            _Instance._BCAPIURL = GetAppSettings("BusinessCardAPIURL", "");
            _Instance._BCAPIURLByHttp = GetAppSettings("BusinessCardAPIURLByHttp", "");
            _Instance._UploadDomain = GetAppSettings("UploadDomain", "localhost");
            _Instance._NoImg = GetAppSettings("NoImg", "");
            _Instance._LOGO = GetAppSettings("LOGO", "");
            _Instance._AsyncPostBackTimeout = GetAppSettings("AsyncPostBackTimeout", 90);
            _Instance._DBConnectionTimeOut = GetAppSettings("DBConnectionTimeOut", 3600);// DB Connection TimeOut (DbCommand TimeOut) 

            _Instance._IMOrgName = GetAppSettings("OrgName", "");
            _Instance._IMAppName = GetAppSettings("AppName", "");
            _Instance._IMClientId = GetAppSettings("ClientId", "");
            _Instance._IMClientSecret = GetAppSettings("ClientSecret", "");
            _Instance._IMHXURL = GetAppSettings("HXURL", "");

            _Instance._PayAppId = GetAppSettings("AppId", "");
            _Instance._PayMCHID = GetAppSettings("MCHID", "");
            _Instance._PayKEY = GetAppSettings("KEY", "");
            _Instance._PayAPPSECRET = GetAppSettings("APPSECRET", "");
            _Instance._PaySSLCERT_PASSWORD = GetAppSettings("SSLCERT_PASSWORD", "");
            _Instance._OpenApiKey = GetAppSettings("OpenApiKey", "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");

            _Instance._GTAPPID = GetAppSettings("GTAPPID", "");
            _Instance._GTAPPKEY = GetAppSettings("GTAPPKEY", "");
            _Instance._GTMASTERSECRET = GetAppSettings("GTMASTERSECRET", "");

            _Instance._SITEURL = GetAppSettings("SITEURL", "");
            _Instance._SITEURLByHttp = GetAppSettings("SITEURLByHttp", "");


        }

        /// <summary>
        /// Gets the app settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private static int GetAppSettings(string key, int defaultValue)
        {
            // get the application setting.
            int value = defaultValue;
            try
            {
                string text = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrEmpty(text))
                {
                    value = int.Parse(text);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            // return value found.
            return value;
        }

        /// <summary>
        /// Gets the app settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private static string GetAppSettings(string key, string defaultValue)
        {
            // get the application setting.
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            //if (key == "UploadFolder" && !Equals(HttpContext.Current, null))
            //{
            //    if (!System.IO.Path.IsPathRooted(value))
            //    {
            //        value = System.Web.HttpContext.Current.Server.MapPath(value);
            //    }
            //    if (!value.Trim().EndsWith(@"\"))
            //    {
            //        value = string.Format(@"{0}\", value.Trim());
            //    }
            //}
            //else if (key == "UploadFolder" && Equals(HttpContext.Current, null))
            //{
            //    value = ConfigurationManager.AppSettings["UploadFolder"];
            //}
            return value;

        }

        /// <summary>
        /// Gets the app settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns>bool</returns>
        private static bool GetAppSettings(string key, bool defaultValue)
        {
            // get the application setting.
            string value = ConfigurationManager.AppSettings[key];
            bool result = true;
            if (string.IsNullOrEmpty(value))
            {
                result = defaultValue;
            }
            else
            {
                result = value.ToLower().Equals(Boolean.TrueString.ToLower());
            }
            return result;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ConfigInfo Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_Padlock)
                    {
                        if (_Instance == null)
                        {
                            Initial();
                        }
                    }
                }
                return _Instance;
            }
        }

    }
    public static class webString
    {
        public static string UrnHtml(string strHtml)
        {
            //Emoji表情 过滤
            strHtml = filterEmoji(strHtml);
            return strHtml;
        }
        public static Boolean containsEmoji(String source)
        {
            char[] item = source.ToCharArray();
            for (int i = 0; i < source.Length; i++)
            {
                if (isEmojiCharacter(item[i]))
                    return true; //do nothing，判断到了这里表明，确认有表情字符
            }
            return false;
        }
        private static Boolean isEmojiCharacter(char codePoint)
        {
            return (codePoint == 0x0) ||
                    (codePoint == 0x9) ||
                    (codePoint == 0xA) ||
                    (codePoint == 0xD) ||
                    ((codePoint >= 0x20) && (codePoint <= 0xD7FF)) ||
                    ((codePoint >= 0xE000) && (codePoint <= 0xFFFD)) ||
                    ((codePoint >= 0x10000) && (codePoint <= 0x10FFFF));
        }
        /**
         * 过滤emoji 或者 其他非文字类型的字符
         * @param source
         * @return
         */
        public static String filterEmoji(String source)
        {
            if (!containsEmoji(source))
                return source;//如果不包含，直接返回
            //到这里铁定包含
            StringBuilder buf = null;
            char[] item = source.ToCharArray();
            for (int i = 0; i < source.Length; i++)
            {
                char codePoint = item[i];
                if (isEmojiCharacter(codePoint))
                {
                    if (buf == null)
                        buf = new StringBuilder(source.Length);
                    buf.Append(codePoint);
                }
            }
            if (buf == null)
                return source;//如果没有找到 emoji表情，则返回源字符串
            else
            {
                if (buf.Length == source.Length)
                {
                    buf = null;//这里的意义在于尽可能少的toString，因为会重新生成字符串
                    return source;
                }
                else
                    return buf.ToString();
            }

        }
    }
}

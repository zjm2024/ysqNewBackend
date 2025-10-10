using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.VO;
using System.Globalization;
using ImportEXCEL;
using System.Data;
using static CsharpTest_GetYan;

namespace SPlatformService.CustomerManagement
{
    public partial class CardIMG3 : System.Web.UI.Page
    {
        public CardDataVO CVO;
        public int Style = 0;
        public int AppType;
        public string Posterback { set; get; }
        public GetWeatherVO Weather { set; get; }
        public bool WeatherStatus = false;
        public string city { set; get; }
        public string temp { set; get; }
        public string weather { set; get; }
        public string WeatherImg = "../Style/images/Weather/duoyun.png";

        public GetYanVO Yan { set; get; }
        public bool YanStatus = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Int64 CardID = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["CardID"]) ? "0" : Request.QueryString["CardID"]);
            Posterback = string.IsNullOrEmpty(Request.QueryString["Posterback"]) ? "" : Server.UrlDecode(Request.QueryString["Posterback"]);
            string IP= string.IsNullOrEmpty(Request.QueryString["IP"]) ? "" : Server.UrlDecode(Request.QueryString["IP"]);
            AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "1" : Request.QueryString["AppType"]);

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardDataVO uVO = cBO.FindCardById(CardID);
            if (uVO != null)
            {
                CVO = uVO;
                CVO.Headimg = CVO.Headimg.Replace("https", "http");
                CVO.CardImg = CVO.CardImg.Replace("https", "http");
            }
            else
            {
                CVO = new CardDataVO();
            }

            try
            {
                GetYanVO GetYanVO = CsharpTest_GetYan.Main();

                if (GetYanVO != null)
                {
                    YanStatus = true;
                    Yan=GetYanVO;
                }

                //获取天气
                /*
                Weather = CsharpTest_GetWeather.Main(IP);
                if (Weather != null && Weather.status == 0)
                {


                    bool isday = true;
                    if (Weather.result.daily.Count > 0)
                    {
                        string sunrise = Weather.result.daily[0].sunrise;//日出时间
                        string sunset = Weather.result.daily[0].sunset;//日洛时间

                        DateTime sunrisedt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + sunrise + ":00");
                        DateTime sunsetdt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + sunset + ":00");

                        if (DateTime.Now < sunrisedt || DateTime.Now > sunsetdt)
                        {
                            isday = false;
                        }
                    }
                    if (Weather.result.weather.IndexOf("多云") > -1)
                    {
                        if (isday)
                        {
                            WeatherImg = "../Style/images/Weather/duoyun.png";
                        }
                        else
                        {
                            WeatherImg = "../Style/images/Weather/duoyun_ye.png";
                        }
                    };
                    if (Weather.result.weather == "晴")
                    {
                        if (isday)
                        {
                            WeatherImg = "../Style/images/Weather/qing.png";
                        }
                        else
                        {
                            WeatherImg = "../Style/images/Weather/qing_ye.png";
                        }
                    }
                    if (Weather.result.weather.IndexOf("扬沙") > -1) WeatherImg = "../Style/images/Weather/yangsha.png";
                    if (Weather.result.weather.IndexOf("雨夹雪") > -1) WeatherImg = "../Style/images/Weather/yujiaxue.png";
                    if (Weather.result.weather.IndexOf("小雨") > -1) WeatherImg = "../Style/images/Weather/xiaoyu.png";
                    if (Weather.result.weather.IndexOf("浮尘") > -1) WeatherImg = "../Style/images/Weather/fuchen.png";
                    if (Weather.result.weather.IndexOf("中雨") > -1 || Weather.result.weather.IndexOf("大雨") > -1) WeatherImg = "../Style/images/Weather/dayu.png";
                    if (Weather.result.weather.IndexOf("中雪") > -1 || Weather.result.weather.IndexOf("大雪") > -1) WeatherImg = "../Style/images/Weather/daxue.png";
                    if (Weather.result.weather.IndexOf("霾") > -1) WeatherImg = "../Style/images/Weather/mai.png";
                    if (Weather.result.weather.IndexOf("小雪") > -1) WeatherImg = "../Style/images/Weather/xiaoxue.png";
                    if (Weather.result.weather.IndexOf("雾") > -1) WeatherImg = "../Style/images/Weather/wu.png";
                    if (Weather.result.weather.IndexOf("阴") > -1) WeatherImg = "../Style/images/Weather/yin.png";

                    city = Weather.result.city;
                    temp = Weather.result.temp;
                    weather = Weather.result.weather;
                    WeatherStatus = true;
                }
                */
            }
            catch
            {

            }
        }  
    }
}
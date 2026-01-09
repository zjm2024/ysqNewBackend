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

namespace SPlatformService.CustomerManagement
{
    public partial class CardIMG2 : System.Web.UI.Page
    {
        public CardDataVO CVO;
        public int Style = 0;
        public string Cyear;
        public string Cmonth;
        public string Emonth;
        public string ChineseDay;
        public string background="";
        public int AppType;

        enum 天干
        {
            甲 = 1,
            乙,
            丙,
            丁,
            戊,
            己,
            庚,
            辛,
            壬,
            癸
        }

        enum 地支
        {
            子 = 1,
            丑,
            寅,
            卯,
            辰,
            巳,
            午,
            未,
            申,
            酉,
            戌,
            亥
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Int64 CardID = Convert.ToInt64(string.IsNullOrEmpty(Request.QueryString["CardID"]) ? "0" : Request.QueryString["CardID"]);
            Style = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["Style"]) ? "0" : Request.QueryString["Style"]);
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

            //如果Style超过1000，则从数据库查询图片地址
            if (Style >= 1000)
            {
                CardPoterVO pVO = cBO.FindCardPoterById(Style);
                if (pVO != null)
                {
                    background = pVO.Url;
                }
               
            }

            ChineseLunisolarCalendar cncal = new ChineseLunisolarCalendar();

            // 获取干支纪年值
            int a = cncal.GetSexagenaryYear(DateTime.Now);

            // 获取天干、地支
            天干 tg = (天干)cncal.GetCelestialStem(a);
            地支 dz = (地支)cncal.GetTerrestrialBranch(a);
            Cyear = $"{tg}{dz}"+"年";

            //获取英文月份
            Emonth = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("en-GB"));

            //获取农历日期
            ChineseDay = GetChineseDateTime(DateTime.Now);
    }

        

        ///<summary>
        /// 实例化一个 ChineseLunisolarCalendar
        ///</summary>
        private static ChineseLunisolarCalendar ChineseCalendar = new ChineseLunisolarCalendar();

        ///<summary>
        /// 十天干
        ///</summary>
        private static string[] tg = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        ///<summary>
        /// 十二地支
        ///</summary>
        private static string[] dz = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        ///<summary>
        /// 十二生肖
        ///</summary>
        private static string[] sx = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        ///<summary>
        /// 返回农历天干地支年
        ///</summary>
        ///<param name="year">农历年</param>
        ///<return s></return s>
        public static string GetLunisolarYear(int year)
        {
            if (year > 3)
            {
                int tgIndex = (year - 4) % 10;
                int dzIndex = (year - 4) % 12;

                return string.Concat(tg[tgIndex], dz[dzIndex], "[", sx[dzIndex], "]");
            }

            throw new ArgumentOutOfRangeException("无效的年份!");
        }

        ///<summary>
        /// 农历月
        ///</summary>

        ///<return s></return s>
        private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };

        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days1 = { "初", "十", "廿", "三" };
        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };


        ///<summary>
        /// 返回农历月
        ///</summary>
        ///<param name="month">月份</param>
        ///<return s></return s>
        public static string GetLunisolarMonth(int month)
        {
            if (month < 13 && month > 0)
            {
                return months[month - 1];
            }

            throw new ArgumentOutOfRangeException("无效的月份!");
        }

        ///<summary>
        /// 返回农历日
        ///</summary>
        ///<param name="day">天</param>
        ///<return s></return s>
        public static string GetLunisolarDay(int day)
        {
            if (day > 0 && day < 32)
            {
                if (day != 20 && day != 30)
                {
                    return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
                }
                else
                {
                    return string.Concat(days[(day - 1) / 10], days1[1]);
                }
            }

            throw new ArgumentOutOfRangeException("无效的日!");
        }



        ///<summary>
        /// 根据公历获取农历日期
        ///</summary>
        ///<param name="datetime">公历日期</param>
        ///<return s></return s>
        public static string GetChineseDateTime(DateTime datetime)
        {
            int year = ChineseCalendar.GetYear(datetime);
            int month = ChineseCalendar.GetMonth(datetime);
            int day = ChineseCalendar.GetDayOfMonth(datetime);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = ChineseCalendar.GetLeapMonth(year);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }

            //返回完整日期
            //return string.Concat(GetLunisolarYear(year), "年", isleap ? "闰" : string.Empty, GetLunisolarMonth(month), "月", GetLunisolarDay(day));


            return string.Concat(GetLunisolarMonth(month), "月", GetLunisolarDay(day));
        }
    }
}
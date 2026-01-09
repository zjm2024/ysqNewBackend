using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SPLibrary.CustomerManagement.BO
{
    public class CompanyBO
    {
        public static bool isAddDummy = false;
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public CompanyBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        /// <summary>
        /// 添加企业
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCompany(CompanyVO vo)
        {
            try
            {
                ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int CompanyID = rDAO.Insert(vo);
                    return CompanyID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CompanyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新企业
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCompany(CompanyVO vo)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CompanyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除企业
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public int DeleteCompany(int CompanyID)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("CompanyID = " + CompanyID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取企业数量
        /// </summary>
        /// <returns></returns>
        public int FindCompanyCount(string condition)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取企业详情
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public CompanyVO FindCompanyByCompanyID(int CompanyID)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);
            CompanyVO cVO = rDAO.FindById(CompanyID);

            try
            {
                CardBO ccBO = new CardBO(new CustomerProfile());
                if (cVO.Address != "" && (cVO.latitude == 0 || cVO.longitude == 0))
                {
                    WeiXinGeocoder Geocoder = ccBO.getLatitudeAndLongitude(cVO.Address);
                    if (Geocoder != null)
                    {
                        cVO.latitude = Geocoder.result.location.lat;
                        cVO.longitude = Geocoder.result.location.lng;
                    }
                    UpdateCompany(cVO);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CompanyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }

            return cVO;
        }

        /// <summary>
        /// 根据IP获取你当前城市
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public IpLocation getIpLocation(string IP)
        {
            string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + IP);
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
            string ipProvince = jo["result"]["ad_info"]["province"].ToString();
            string ipCity = jo["result"]["ad_info"]["city"].ToString();

            string location = "";

            if (ipProvince.Contains("北京"))
            {
                location = "北京";
            }
            else if (ipProvince.Contains("上海"))
            {
                location = "上海";
            }
            else if (ipProvince.Contains("天津"))
            {
                location = "天津";
            }
            else if (ipProvince.Contains("重庆"))
            {
                location = "重庆";
            }
            else if (ipProvince.Contains("香港"))
            {
                location = "香港";
            }
            else if (ipProvince.Contains("澳门"))
            {
                location = "澳门";
            }
            else
            {
                ipProvince = ipProvince.Replace("省", "");
                if (ipProvince.Contains("内蒙古"))
                {
                    ipProvince = "内蒙古";
                }
                if (ipProvince.Contains("广西"))
                {
                    ipProvince = "广西";
                }
                if (ipProvince.Contains("西藏"))
                {
                    ipProvince = "西藏";
                }
                if (ipProvince.Contains("宁夏"))
                {
                    ipProvince = "宁夏";
                }
                if (ipProvince.Contains("新疆"))
                {
                    ipProvince = "新疆";
                }
                location = ipProvince + ipCity;
            }

            IpLocation IpLocation = new IpLocation();
            IpLocation.location = location;
            IpLocation.ipCity = ipCity;
            IpLocation.ipProvince = ipProvince;
            return IpLocation;
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CompanyVO> FindCompanyByCondtion(string condtion)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取企业地区列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CompanyLocationViewVO> FindCompanyLocationByCondtion(string condtion)
        {
            ICompanyLocationViewDAO rDAO = CustomerManagementDAOFactory.CompanyLocationViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CompanyVO> FindCompanyByCondtion(string condtion,int limit)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(condtion, limit);
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CompanyVO> FindCompanyUPByCondtion(string condtion)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyUPDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取精选企业列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CompanyVO> FindCompanyUPByCondtion(string condtion, int limit)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyUPDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(condtion, limit);
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CompanyVO> FindCompanyAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICompanyDAO rDAO = CustomerManagementDAOFactory.CompanyDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 根据搜索企业列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <param name="Industry">1，IT互联网；2，农林牧渔；3，政府/非盈利；4，能源矿产；5，文化传媒；6，服务业；7，交通物流；8，加工制造；9，工艺美术；10，文化/教育</param>
        /// <returns></returns>
        public CompanyList GetIndustryList(int Industry, string location, int PageCount = 50, int PageIndex = 1)
        {
            List<string> key = new List<string>();
            switch (Industry)
            {
                case 1: //IT互联网
                    key.Add("信息科技");
                    key.Add("信息技术");
                    key.Add("网络科技");
                    key.Add("网络技术");
                    key.Add("电子商务");
                    key.Add("软件");
                    key.Add("计算机");
                    break;
                case 2: //农林牧渔
                    key.Add("花卉");
                    key.Add("食品");
                    key.Add("土地");
                    key.Add("养殖");
                    key.Add("农民");
                    key.Add("养殖");
                    key.Add("种植");
                    key.Add("农业");
                    key.Add("农产品");
                    key.Add("农牧");
                    key.Add("畜牧");
                    break;
                case 3: //政府/非盈利
                    key.Add("协会");
                    key.Add("工会");
                    key.Add("校友会");
                    key.Add("学校");
                    key.Add("医院");
                    key.Add("图书馆");
                    key.Add("出版社");
                    key.Add("合作社");
                    key.Add("银行");
                    key.Add("铁路");
                    break;
                case 4: //能源矿产
                    key.Add("能源");
                    key.Add("发电");
                    key.Add("风电");
                    key.Add("电力");
                    key.Add("开采");
                    key.Add("石业");
                    key.Add("石油");
                    key.Add("钻井");
                    key.Add("矿");
                    key.Add("石料");
                    key.Add("天然气");
                    break;
                case 5: //文化传媒
                    key.Add("文化");
                    key.Add("传媒");
                    key.Add("传播");
                    key.Add("广告");
                    key.Add("新闻");
                    break;
                case 6: //服务业
                    key.Add("酒店");
                    key.Add("茶楼");
                    key.Add("餐饮");
                    key.Add("旅游");
                    key.Add("宾馆");
                    key.Add("服务");
                    key.Add("事务");
                    key.Add("劳务");
                    key.Add("教育");
                    key.Add("咨询");
                    key.Add("人力");
                    key.Add("设计");
                    break;
                case 7: //交通物流
                    key.Add("港口");
                    key.Add("货运");
                    key.Add("物流");
                    key.Add("邮政");
                    key.Add("快递");
                    key.Add("船");
                    key.Add("供应链");
                    key.Add("航空");
                    key.Add("铁路");
                    break;
                case 8: //加工制造
                    key.Add("实业");
                    key.Add("加工");
                    key.Add("机械");
                    key.Add("工业");
                    key.Add("贸易");
                    key.Add("厂");
                    key.Add("化工");
                    break;
                case 9: //工艺美术
                    key.Add("工艺");
                    key.Add("动漫");
                    key.Add("饰");
                    key.Add("雕");
                    break;
                case 10: //文化/教育
                    key.Add("文化");
                    key.Add("学");
                    key.Add("艺术");
                    key.Add("培训");
                    key.Add("教育");
                    key.Add("幼儿园");
                    key.Add("学校");
                    break;
                default:
                    key.Add("");
                    break;
            }

            string sql = "Location like '" + location + "%' and (";
            for(int i=0;i< key.Count; i++)
            {
                sql += "locate('" + key[i] + "', CompanyName) > 0 ";
                if(i< key.Count - 1)
                {
                    sql += " or ";
                }
            }

            sql += ")";

            List<CompanyVO> List = FindCompanyAllByPageIndex(sql, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CompanyID", "desc");
            int count = FindCompanyCount(sql);

            CompanyList CompanyList = new CompanyList();
            CompanyList.List = List;
            CompanyList.Count = count;
            return CompanyList;
        }
        /// <summary>
        /// 搜索企业列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CompanyVO> FindCompanyBySearch(string key, string location,int AppType=1,decimal latitude=0, decimal longitude=0)
        {
            //List<CompanyVO> CVO = FindCompanyByCondtion("Location like '" + location + "%' and (locate('" + key + "', CompanyName) > 0 or locate('" + key + "', Description) > 0)");
            string sql = "Location like '" + location + "%'";

            if(latitude>0&& longitude > 0)
            {
                sql = "Location like '" + location + "%' order by power(ABS(latitude-" + latitude + "),2)+power(ABS(longitude-" + longitude + "),2) asc";
            }else
            {
                sql = "Location like '" + location + "%' order by RAND()";
            }

            if (key+"" != "")
            {
                sql = "locate('" + key + "', CompanyName) > 0 and " + sql;
            }else
            {
                sql = "Tel like '1%' and " + sql;
            }

            List<CompanyVO> CVO = FindCompanyByCondtion(sql, 1000);
            List<CompanyVO> CardCompanyVO = new List<CompanyVO>();

            if (AppType == 3)
            {
                location = location.Replace("上海", "");
                location = location.Replace("广东", "");
                location = location.Replace("吉林", "");
                location = location.Replace("四川", "");
                location = location.Replace("宁夏", "");
                location = location.Replace("安徽", "");
                location = location.Replace("山东", "");
                location = location.Replace("山西", "");
                location = location.Replace("广西", "");
                location = location.Replace("新疆", "");
                location = location.Replace("江苏", "");
                location = location.Replace("江西", "");
                location = location.Replace("河北", "");
                location = location.Replace("河南", "");
                location = location.Replace("浙江", "");
                location = location.Replace("海南", "");
                location = location.Replace("湖北", "");
                location = location.Replace("湖南", "");
                location = location.Replace("甘肃", "");
                location = location.Replace("福建", "");
                location = location.Replace("西藏", "");
                location = location.Replace("贵州", "");
                location = location.Replace("辽宁", "");
                location = location.Replace("陕西", "");
                location = location.Replace("青海", "");
                location = location.Replace("黑龙江", "");
                location = location.Replace("内蒙古", "");
                location = location.Replace("云南", "");
                location = location.Replace("台湾", "");
                location = location.Replace("市", "");
                location = location.Replace("自治县", "");
                location = location.Replace("自治州", "");
                location = location.Replace("盟", "");
                location = location.Replace("地区", "");
                location = location.Replace("县", "");
                location = location.Replace("区", "");

                string conditionStr = "(locate('" + key + "', Name) > 0 or locate('" + key + "', Position) > 0 or locate('" + key + "', CorporateName) > 0 or locate('" + key + "', Business) > 0) and locate('" + location + "', Address) >  0 and Phone IS NOT NULL";
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);

                List<CardDataVO> list = cBO.FindAllByPageIndex(conditionStr, 1, 50, "CreatedAt", "desc");
                foreach(CardDataVO item in list)
                {
                    CompanyVO CompanyVO = new CompanyVO();
                    CompanyVO.Contacts = item.Name;
                    CompanyVO.CompanyName = item.CorporateName;
                    CompanyVO.Address = item.Address;
                    CompanyVO.Headimg = item.Headimg;
                    CompanyVO.Phone = item.Phone;
                    CompanyVO.Tel = item.Tel;
                    CompanyVO.CardID = item.CardID;
                    CompanyVO.CompanyID = 0;
                    CardCompanyVO.Add(CompanyVO);
                }
            }
            if(CardCompanyVO.Count+ CVO.Count<=50)
            {
                CVO = FindCompanyByCondtion(sql, 1000);
            }
            CardCompanyVO.AddRange(CVO);

            int sum = 50; //随机抽取数量
            Random ran = new Random();
            sum = ran.Next(50, 100);

            if (CardCompanyVO.Count > sum)
            {
                Random random = new Random();
                List<CompanyVO> newList = new List<CompanyVO>();
                foreach (CompanyVO item in CardCompanyVO)
                {
                    newList.Insert(random.Next(newList.Count + 1), item);
                }
                var list = newList.Take(sum);
                CardCompanyVO = new List<CompanyVO>(list);
            }

            List<myFileInfo> HeadimgVO = GetFileJson();
            for (int i=0;i< CardCompanyVO.Count&&i< HeadimgVO.Count; i++)
            {
                if (CardCompanyVO[i].CardID == 0&& CardCompanyVO[i].Headimg=="")
                {
                    CardCompanyVO[i].Headimg = HeadimgVO[i].Url;
                }

                if (CardCompanyVO[i].MarkerHeadimg == "")
                {
                    CardCompanyVO[i].MarkerHeadimg = GetAddressIMG(CardCompanyVO[i].Headimg);
                    if (CardCompanyVO[i].CompanyID > 0)
                    {
                        UpdateCompany(CardCompanyVO[i]);
                    }
                }
            }
            return CardCompanyVO;
        }

        public List<myFileInfo> GetFileJson(int sum=50)
        {
            List<myFileInfo> FileList = new List<myFileInfo>();
            //目录
            string path = "C:/web/ServicesPlatform/UploadFolder/Headimg/Default";
            DirectoryInfo di = new DirectoryInfo(path);
            //找到该目录下的文件 
            FileInfo[] fis = di.GetFiles();

            foreach (FileInfo fi in fis)
            {
                myFileInfo fVO = new myFileInfo();

                fVO.FileName = fi.Name.Split('.')[0];
                fVO.Url = "https://www.zhongxiaole.net/SPManager/UploadFolder/Headimg/Default/" + HttpUtility.UrlEncode(fi.Name);

                FileList.Add(fVO);
            }

            if (FileList.Count > sum)
            {
                Random random = new Random();
                List<myFileInfo> newList = new List<myFileInfo>();
                foreach (myFileInfo item in FileList)
                {
                    newList.Insert(random.Next(newList.Count + 1), item);
                }
                var list = newList.Take(sum);
                FileList = new List<myFileInfo>(list);
            }

            return FileList;
        }

        /// <summary>
        /// 搜索企业列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public void FindCompanyBySearch(string location)
        {
            List<CompanyVO> CVO = FindCompanyByCondtion("Location like '" + location + "%'");
            for (int i = 0; i < CVO.Count; i++)
            {
                //通过ParameterizedThreadStart创建线程
                Thread thread = new Thread(new ParameterizedThreadStart(UpdataLocation));
                //给方法传值
                thread.Start(CVO[i]);
            }
            return;
        }

        public void UpdataLocation(object obj)
        {
            CompanyVO cVO = (CompanyVO)obj;
            if (cVO.Location != "上海")
            {
                return;
            }
            try
            {
                if (cVO.Address.Contains("黄浦区"))
                {
                    cVO.Location = "上海黄浦区";
                }
                else if (cVO.Address.Contains("徐汇区"))
                {
                    cVO.Location = "上海徐汇区";
                }
                else if (cVO.Address.Contains("长宁区"))
                {
                    cVO.Location = "上海长宁区";
                }
                else if (cVO.Address.Contains("静安区"))
                {
                    cVO.Location = "上海静安区";
                }
                else if (cVO.Address.Contains("普陀区"))
                {
                    cVO.Location = "上海普陀区";
                }
                else if (cVO.Address.Contains("虹口区"))
                {
                    cVO.Location = "上海虹口区";
                }
                else if (cVO.Address.Contains("杨浦区"))
                {
                    cVO.Location = "上海杨浦区";
                }
                else if (cVO.Address.Contains("浦东新区"))
                {
                    cVO.Location = "上海浦东新区";
                }
                else if (cVO.Address.Contains("闵行区"))
                {
                    cVO.Location = "上海闵行区";
                }
                else if (cVO.Address.Contains("宝山区"))
                {
                    cVO.Location = "上海宝山区";
                }
                else if (cVO.Address.Contains("嘉定区"))
                {
                    cVO.Location = "上海嘉定区";
                }
                else if (cVO.Address.Contains("金山区"))
                {
                    cVO.Location = "上海金山区";
                }
                else if (cVO.Address.Contains("松江区"))
                {
                    cVO.Location = "上海松江区";
                }
                else if (cVO.Address.Contains("青浦区"))
                {
                    cVO.Location = "上海青浦区";
                }
                else if (cVO.Address.Contains("奉贤区"))
                {
                    cVO.Location = "上海奉贤区";
                }
                else if (cVO.Address.Contains("崇明区"))
                {
                    cVO.Location = "上海崇明区";
                }
                UpdateCompany(cVO);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 更新企业经纬度
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public List<CompanyVO> UpdateCompanyLongitude(List<CompanyVO> List)
        {
            CardBO ccBO = new CardBO(new CustomerProfile());
            for (int i=0;i< List.Count; i++)
            {
                try
                {
                    if(List[i].Address!=""&&(List[i].latitude==0|| List[i].longitude == 0))
                    {
                        WeiXinGeocoder Geocoder = ccBO.getLatitudeAndLongitude(List[i].Location + List[i].Address);
                        if (Geocoder != null)
                        {
                            List[i].latitude = Geocoder.result.location.lat;
                            List[i].longitude = Geocoder.result.location.lng;
                        }
                        UpdateCompany(List[i]);
                    }
                }
                catch (Exception ex)
                {
                    LogBO _log = new LogBO(typeof(CompanyBO));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                }
                Thread.Sleep(50);
            }
            return List;
        }


        /// <summary>
        /// 更新企业经纬度 (多线程)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void UpdateCompanyLongitudeByThread(List<CompanyVO> List)
        {
            try
            {
                for (int i = 0; i <= List.Count; i=i+100)
                {

                    List<CompanyVO> CompanyVO = List.GetRange(i,500);
                    //通过ParameterizedThreadStart创建线程
                    Thread thread = new Thread(new ParameterizedThreadStart(GetLongitude));
                    //给方法传值
                    thread.Start(CompanyVO);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 更新企业经纬度 (多线程)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void GetLongitude(object obj)
        {
            List<CompanyVO> CompanyVO = (List<CompanyVO>)obj;
            try
            {
                UpdateCompanyLongitude(CompanyVO);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 更新企业头像 (多线程)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void UpdateCompanyHeadimgByThread(List<CompanyVO> List, List<myFileInfo> List2)
        {
            try
            {
                for (int i = 0; i <= List.Count; i++)
                {

                    List<CompanyVO> CompanyVO = List.GetRange(i, 1);
                    List<myFileInfo> myFileInfo = List2.GetRange(i, 1);

                    myFileInfoAndCompanyVO mac = new myFileInfoAndCompanyVO();
                    mac.CompanyVO = CompanyVO;
                    mac.myFileInfo = myFileInfo;
                    //通过ParameterizedThreadStart创建线程
                    Thread thread = new Thread(new ParameterizedThreadStart(GetHeadimg));
                    //给方法传值
                    thread.Start(mac);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 更新企业头像 (多线程)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void GetHeadimg(object obj)
        {
            myFileInfoAndCompanyVO mac = (myFileInfoAndCompanyVO)obj;
            try
            {
                for (int i = 0; i <= mac.CompanyVO.Count; i++)
                {
                    mac.CompanyVO[i].Headimg = mac.myFileInfo[i].Url;
                    UpdateCompany(mac.CompanyVO[i]);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 保存地图坐标头像
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetAddressIMG(string Headimg)
        {
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/CompanyIMG.aspx?Headimg=" + Headimg, 500, 500, 500, 500);

            //保存
            string filePath = "";
            string folder = "/UploadFolder/CompanyMarkerFile/";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;

            //使图片变透明
            m_Bitmap.MakeTransparent(Color.FromArgb(0, 0, 0));
            Bitmap objNewPic = new Bitmap(m_Bitmap, 100, 100);
            objNewPic.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string CompanyIMG = ConfigInfo.Instance.APIURL + filePath;
            return CompanyIMG;
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void Start(int Start,int End)
        {
            try
            {
                for(int i= Start; i<= End; i++)
                {
                    string url= "https://www.71ab.com/view_"+i+".html";
                    //通过ParameterizedThreadStart创建线程
                    Thread thread = new Thread(new ParameterizedThreadStart(GetHtml));
                    //给方法传值
                    thread.Start(url);
                }
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 获取采集网页数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public  void GetHtml(object obj)
        {
            string url = (string)obj;
            try
            {
                string Html = HttpHelper.HttpGet(url, "", 5);
                CompanyVO cVO = GetCompanyVOByHtml(Html);
                if (cVO != null)
                {
                    AddCompany(cVO);
                }
            }
            catch
            {
                
            }
        }
        /// <summary>
        /// 从网页数据提取出企业信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public CompanyVO GetCompanyVOByHtml(string Html)
        {
            try
            {
                CompanyVO cVO = new CompanyVO();
                cVO.CompanyID = 0;

                //公司名称
                Match match = Regex.Match(Html, @"<h1>(?<text>[^\f\n\r\t\v]*)</h1>");
                cVO.CompanyName = match.Groups["text"].Value.Trim();

                if (cVO.CompanyName == "")
                {
                    return null;
                }

                //公司类型
                match = Regex.Match(Html, @"<span>公司类型：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.CompanyType = match.Groups["text"].Value.Trim();

                //所 在 地
                match = Regex.Match(Html, @"<span>所 在 地：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.Location = match.Groups["text"].Value.Trim();
                cVO.Location = cVO.Location.Replace("[", "");
                cVO.Location = cVO.Location.Replace("]", "");

                //公司规模
                match = Regex.Match(Html, @"<span>公司规模：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.CompanySize = match.Groups["text"].Value.Trim();

                //注册资本
                match = Regex.Match(Html, @"<span>注册资本：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.RegisteredCapital = match.Groups["text"].Value.Trim();

                //注册年份
                match = Regex.Match(Html, @"<span>注册年份：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.YearOfRegistration = match.Groups["text"].Value.Trim();

                //公司地址
                match = Regex.Match(Html, @"<span>公司地址：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.Address = match.Groups["text"].Value.Trim();

                //邮政编码
                match = Regex.Match(Html, @"<span>邮政编码：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.PostalCode = match.Groups["text"].Value.Trim();

                //公司电话
                match = Regex.Match(Html, @"<span>公司电话：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.Tel = match.Groups["text"].Value.Trim();

                //联系人
                match = Regex.Match(Html, @"<span>联系人：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.Contacts = match.Groups["text"].Value.Trim();

                //手机号码
                match = Regex.Match(Html, @"<span>手机号码：</span>(?<text>[^\f\n\r\t\v]*)</li>");
                cVO.Phone = match.Groups["text"].Value.Trim();

                //简介
                match = Regex.Match(Html, @"<p class=""cont-p"">(?<text>[\S\s\n]*)</p>");
                cVO.Description = new HtmlParser(match.Groups["text"].Value.Trim()).Text();
                cVO.Description = cVO.Description.Replace("\r","");
                cVO.Description = cVO.Description.Replace("\n", "");
                cVO.Description = cVO.Description.Replace("\r\n","");
                cVO.Description = cVO.Description.Trim();

                return cVO;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CompanyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }
    }
    public class IpLocation
    {
        public string location { get; set; }
        public string ipProvince { get; set; }
        public string ipCity { get; set; }
    }
}



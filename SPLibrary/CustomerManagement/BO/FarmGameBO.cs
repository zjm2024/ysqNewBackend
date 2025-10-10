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
    public class FarmGameBO
    {
        public static bool isAddDummy = false;
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public FarmGameBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        /// <summary>
        /// 添加游戏档案
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddFarmGame(FarmGameVO vo)
        {
            try
            {
                IFarmGameDAO rDAO = CustomerManagementDAOFactory.FarmGameDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int FarmGameID = rDAO.Insert(vo);
                    return FarmGameID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新游戏档案
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateFarmGame(FarmGameVO vo)
        {
            IFarmGameDAO rDAO = CustomerManagementDAOFactory.FarmGameDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除游戏档案
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public int DeleteFarmGame(int FarmGameID)
        {
            IFarmGameDAO rDAO = CustomerManagementDAOFactory.FarmGameDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("FarmGameID = " + FarmGameID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取游戏档案列表（视图）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<FarmGameViewVO> FindFarmGameViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IFarmGameViewDAO rDAO = CustomerManagementDAOFactory.FarmGameViewDAO(this.CurrentCustomerProfile);
            List<FarmGameViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取游戏档案数量（视图）
        /// </summary>
        /// <returns></returns>
        public int FindFarmGameViewCount(string condition)
        {
            IFarmGameViewDAO rDAO = CustomerManagementDAOFactory.FarmGameViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取游戏档案数量
        /// </summary>
        /// <returns></returns>
        public int FindFarmGameCount(string condition)
        {
            IFarmGameDAO rDAO = CustomerManagementDAOFactory.FarmGameDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取游戏档案
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public FarmGameVO FindFarmGameByFarmGameID(int FarmGameID)
        {
            IFarmGameDAO rDAO = CustomerManagementDAOFactory.FarmGameDAO(this.CurrentCustomerProfile);
            FarmGameVO cVO = rDAO.FindById(FarmGameID);
            return cVO;
        }

        /// <summary>
        /// 获取游戏档案列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<FarmGameVO> FindFarmGameByCondition(string condition, params object[] parameters)
        {
            IFarmGameDAO rDAO = CustomerManagementDAOFactory.FarmGameDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }

        /// <summary>
        /// 获取游戏档案
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public FarmGameVO FindFarmGameByCustomerId(int CustomerId)
        {
            List<FarmGameVO> fgVO = FindFarmGameByCondition("CustomerId="+ CustomerId);
            if (fgVO.Count > 0)
            {
                return fgVO[0];
            }else
            {
                FarmGameVO newFGVO = new FarmGameVO();
                newFGVO.FarmGameID = 0;
                newFGVO.CustomerId = CustomerId;
                newFGVO.CreatedAt = DateTime.Now;
                newFGVO.Gold = FGConfig.Gold;
                newFGVO.Water = FGConfig.Water;
                newFGVO.Fertilizer = FGConfig.Fertilizer;

                int FarmGameID = AddFarmGame(newFGVO);
                if (FarmGameID > 0)
                {
                    FarmGameVO FVO = FindFarmGameByFarmGameID(FarmGameID);
                    return FVO;
                }
                return null;
            }
        }

        /// <summary>
        /// 添加游戏任务
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddFarmGameTask(FarmGameTaskVO vo)
        {
            try
            {
                IFarmGameTaskDAO rDAO = CustomerManagementDAOFactory.FarmGameTaskDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int TaskID = rDAO.Insert(vo);
                    return TaskID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新游戏任务
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateFarmGameTask(FarmGameTaskVO vo)
        {
            IFarmGameTaskDAO rDAO = CustomerManagementDAOFactory.FarmGameTaskDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除游戏任务
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public int DeleteFarmGameTask(int TaskID)
        {
            IFarmGameTaskDAO rDAO = CustomerManagementDAOFactory.FarmGameTaskDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("TaskID = " + TaskID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取游戏任务数量
        /// </summary>
        /// <returns></returns>
        public int FindFarmGameTaskCount(string condition)
        {
            IFarmGameTaskDAO rDAO = CustomerManagementDAOFactory.FarmGameTaskDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取游戏任务
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public FarmGameTaskVO FindFarmGameTaskByTaskID(int TaskID)
        {
            IFarmGameTaskDAO rDAO = CustomerManagementDAOFactory.FarmGameTaskDAO(this.CurrentCustomerProfile);
            FarmGameTaskVO cVO = rDAO.FindById(TaskID);
            return cVO;
        }

        /// <summary>
        /// 获取游戏任务列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<FarmGameTaskVO> FindFarmGameTaskByCondition(string condition, params object[] parameters)
        {
            IFarmGameTaskDAO rDAO = CustomerManagementDAOFactory.FarmGameTaskDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }

        /// <summary>
        /// 获取今天的游戏任务
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<FarmGameTaskVO> FindFarmGameTaskByCustomerId(int CustomerId)
        {
            string sql = "CustomerId=" + CustomerId + " and DATE_FORMAT(CreatedAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')";
            List<FarmGameTaskVO> TaskVO = new List<FarmGameTaskVO>();

            //每日登录
            List<FarmGameTaskVO> SignIn = FindFarmGameTaskByCondition("Type='SignIn' and " + sql);
            if (SignIn.Count > 0)
            {
                TaskVO.Add(SignIn[0]);
            }else
            {
                FarmGameTaskVO newTask = new FarmGameTaskVO();
                newTask.CreatedAt = DateTime.Now;
                newTask.Title = "每日登录";
                newTask.Type = "SignIn";
                newTask.Gold = FGConfig.SignIn_Gold;
                newTask.Status = 1;
                newTask.CustomerId = CustomerId;
                TaskVO.Add(newTask);
                AddFarmGameTask(newTask);
            }

            //参与一次抽奖
            List<FarmGameTaskVO> SignLuckDraw = FindFarmGameTaskByCondition("Type='SignLuckDraw' and " + sql);
            if (SignLuckDraw.Count > 0)
            {
                TaskVO.Add(SignLuckDraw[0]);
            }
            else
            {
                FarmGameTaskVO newTask = new FarmGameTaskVO();
                newTask.CreatedAt = DateTime.Now;
                newTask.Title = "参与一次抽奖";
                newTask.Type = "SignLuckDraw";
                newTask.Water = FGConfig.SignLuckDraw_Water;
                newTask.CustomerId = CustomerId;
                newTask.Status = 0;
                TaskVO.Add(newTask);
            }

            //参与三次抽奖
            List<FarmGameTaskVO> SignThreeLuckDraw = FindFarmGameTaskByCondition("Type='SignThreeLuckDraw' and " + sql);
            if (SignThreeLuckDraw.Count > 0)
            {
                TaskVO.Add(SignThreeLuckDraw[0]);
            }
            else
            {
                FarmGameTaskVO newTask = new FarmGameTaskVO();
                newTask.CreatedAt = DateTime.Now;
                newTask.Title = "参与三次抽奖";
                newTask.Type = "SignThreeLuckDraw";
                newTask.Water = FGConfig.SignThreeLuckDraw_Water;
                newTask.Status = 0;
                newTask.CustomerId = CustomerId;
                TaskVO.Add(newTask);
            }

            //分享一次抽奖
            List<FarmGameTaskVO> ShareLuckDraw = FindFarmGameTaskByCondition("Type='ShareLuckDraw' and " + sql);
            if (ShareLuckDraw.Count > 0)
            {
                TaskVO.Add(ShareLuckDraw[0]);
            }
            else
            {
                FarmGameTaskVO newTask = new FarmGameTaskVO();
                newTask.CreatedAt = DateTime.Now;
                newTask.Title = "分享一次抽奖";
                newTask.Type = "ShareLuckDraw";
                newTask.Water = FGConfig.ShareLuckDraw_Water;
                newTask.Status = 0;
                newTask.CustomerId = CustomerId;
                TaskVO.Add(newTask);
            }

            //邀请一次助力
            List<FarmGameTaskVO> HelpLuckDraw = FindFarmGameTaskByCondition("Type='HelpLuckDraw' and " + sql);
            if (HelpLuckDraw.Count > 0)
            {
                TaskVO.Add(HelpLuckDraw[0]);
            }
            else
            {
                FarmGameTaskVO newTask = new FarmGameTaskVO();
                newTask.CreatedAt = DateTime.Now;
                newTask.Title = "邀请一次助力";
                newTask.Type = "HelpLuckDraw";
                newTask.Fertilizer = FGConfig.HelpLuckDraw_Fertilizer;
                newTask.Status = 0;
                newTask.CustomerId = CustomerId;
                TaskVO.Add(newTask);
            }

            //观看广告
            List<FarmGameTaskVO> WatchAd = FindFarmGameTaskByCondition("Type='WatchAd' and " + sql);
            if (WatchAd.Count > 0)
            {
                TaskVO.Add(WatchAd[0]);
            }
            else
            {
                FarmGameTaskVO newTask = new FarmGameTaskVO();
                newTask.CreatedAt = DateTime.Now;
                newTask.Title = "观看一次广告";
                newTask.Type = "WatchAd";
                newTask.Fertilizer = FGConfig.WatchAd_Fertilizer;
                newTask.Status = 0;
                newTask.CustomerId = CustomerId;
                TaskVO.Add(newTask);
            }

            return TaskVO;
        }

        /// <summary>
        /// 发放游戏奖励
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool IssueTaskReward(int CustomerId,string Type)
        {
            string sql = "CustomerId=" + CustomerId + " and DATE_FORMAT(CreatedAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')";
            List<FarmGameTaskVO> TaskVO = FindFarmGameTaskByCondition("Type='"+ Type + "' and " + sql);
            if (TaskVO.Count > 0)
            {
                return false;
            }
            else {

                string Title = "";
                int Water=0;
                int Fertilizer=0;
                int Gold=0;

                if (Type == "SignIn") { Title = "每日登录"; Water = FGConfig.SignIn_Water;}
                if (Type == "SignLuckDraw") { Title = "参与一次抽奖"; Water = FGConfig.SignLuckDraw_Water; }
                if (Type == "SignThreeLuckDraw") { Title = "参与三次抽奖"; Water = FGConfig.SignThreeLuckDraw_Water; }
                if (Type == "ShareLuckDraw") { Title = "分享一次抽奖"; Water = FGConfig.ShareLuckDraw_Water; }
                if (Type == "HelpLuckDraw") { Title = "邀请一次助力"; Fertilizer = FGConfig.HelpLuckDraw_Fertilizer; }
                if (Type == "WatchAd") { Title = "观看一次广告"; Fertilizer = FGConfig.WatchAd_Fertilizer; }

                FarmGameTaskVO newTask = new FarmGameTaskVO();
                newTask.CreatedAt = DateTime.Now;
                newTask.Title = Title;
                newTask.Type = Type;
                newTask.Water = Water;
                newTask.Fertilizer = Fertilizer;
                newTask.Gold = Gold;
                newTask.Status = 1;
                newTask.CustomerId = CustomerId;
                AddFarmGameTask(newTask);

                return true;
            }
        }

        /// <summary>
        /// 添加游戏奖品
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddFarmgamePrize(FarmgamePrizeVO vo)
        {
            try
            {
                IFarmgamePrizeDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int PrizeID = rDAO.Insert(vo);
                    return PrizeID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新游戏奖品
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateFarmgamePrize(FarmgamePrizeVO vo)
        {
            IFarmgamePrizeDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除游戏奖品
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public int DeleteFarmgamePrize(int PrizeID)
        {
            IFarmgamePrizeDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("PrizeID = " + PrizeID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取游戏奖品列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<FarmgamePrizeVO> FindFarmgamePrizeAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IFarmgamePrizeDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeDAO(this.CurrentCustomerProfile);
            List<FarmgamePrizeVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取游戏奖品列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<FarmgamePrizeVO> FindFarmGamePrizeByCondition(string condition, params object[] parameters)
        {
            IFarmgamePrizeDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }

        /// <summary>
        /// 获取游戏奖品数量
        /// </summary>
        /// <returns></returns>
        public int FindFarmgamePrizeCount(string condition)
        {
            IFarmgamePrizeDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取游戏奖品
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public FarmgamePrizeVO FindFarmgamePrizeByPrizeID(int PrizeID)
        {
            IFarmgamePrizeDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeDAO(this.CurrentCustomerProfile);
            FarmgamePrizeVO cVO = rDAO.FindById(PrizeID);
            return cVO;
        }

        /// <summary>
        /// 添加奖品兑换订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddFarmgamePrizeOrder(FarmgamePrizeOrderVO vo)
        {
            try
            {
                IFarmgamePrizeOrderDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int PrizeOrderID = rDAO.Insert(vo);
                    return PrizeOrderID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新奖品兑换订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateFarmgamePrizeOrder(FarmgamePrizeOrderVO vo)
        {
            IFarmgamePrizeOrderDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(FarmGameBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除奖品兑换订单
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public int DeleteFarmgamePrizeOrder(int PrizeOrderID)
        {
            IFarmgamePrizeOrderDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("PrizeOrderID = " + PrizeOrderID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取奖品兑换订单列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<FarmgamePrizeOrderVO> FindFarmgamePrizeOrderAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IFarmgamePrizeOrderDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderDAO(this.CurrentCustomerProfile);
            List<FarmgamePrizeOrderVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取奖品兑换订单列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<FarmgamePrizeOrderVO> FindFarmGamePrizeOrderByCondition(string condition, params object[] parameters)
        {
            IFarmgamePrizeOrderDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }

        /// <summary>
        /// 获取奖品兑换订单数量
        /// </summary>
        /// <returns></returns>
        public int FindFarmgamePrizeOrderCount(string condition)
        {
            IFarmgamePrizeOrderDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取奖品兑换订单
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public FarmgamePrizeOrderVO FindFarmgamePrizeOrderByPrizeOrderID(int PrizeOrderID)
        {
            IFarmgamePrizeOrderDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderDAO(this.CurrentCustomerProfile);
            FarmgamePrizeOrderVO cVO = rDAO.FindById(PrizeOrderID);
            return cVO;
        }


        /// <summary>
        /// 获取奖品兑换订单列表（视图）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<FarmgamePrizeOrderViewVO> FindFarmgamePrizeOrderViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IFarmgamePrizeOrderViewDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderViewDAO(this.CurrentCustomerProfile);
            List<FarmgamePrizeOrderViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取奖品兑换订单数量（视图）
        /// </summary>
        /// <returns></returns>
        public int FindFarmgamePrizeOrderViewCount(string condition)
        {
            IFarmgamePrizeOrderViewDAO rDAO = CustomerManagementDAOFactory.FarmgamePrizeOrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }
    }
}



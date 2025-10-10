using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace SPLibrary.CustomerManagement.BO
{
    public class ZXTBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public ZXTBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        public int AddZXTMessage(ZXTMessageVO vo)//添加聊天信息
        {
            try
            {
                IZXTMessageDAO rDAO = CustomerManagementDAOFactory.CreateZXTMessageDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int demandId = rDAO.Insert(vo);
                    return demandId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ZXTBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public int AddZXTFriend(ZXTFriendVO vo)//添加联系人
        {
            try
            {
                IZXTFriendDAO rDAO = CustomerManagementDAOFactory.CreateZXTFriendDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int demandId = rDAO.Insert(vo);
                    return demandId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ZXTBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public int DelZXTFriend(List<ZXTFriendVO> voList, string condition)//删除联系人
        {
            try
            {
                IZXTFriendDAO rDAO = CustomerManagementDAOFactory.CreateZXTFriendDAO(this.CurrentCustomerProfile);
                rDAO.DeleteListByParams(voList, condition);
                return 1;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ZXTBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public int FindZXTFriendCount(string condition, params object[] parameters)//获取联系人数量
        {
            IZXTFriendDAO rDAO = CustomerManagementDAOFactory.CreateZXTFriendDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }
        public List<ZXTFriendViewVO> FindZXTFriendViewByCustomer(string condition)//获取联系人列表(视图)
        {
            IZXTFriendViewDAO rDAO = CustomerManagementDAOFactory.CreateZXTFriendViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }
        public List<ZXTFriendVO> FindZXTFriendByCustomer(string condition)//获取联系人列表
        {
            IZXTFriendDAO rDAO = CustomerManagementDAOFactory.CreateZXTFriendDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }
        //获取聊天信息分页列表
        public List<ZXTMessageViewVO> FindZXTMessageAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IZXTMessageViewDAO rDAO = CustomerManagementDAOFactory.CreateZXTMessageViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }
        public int FindZXTMessageCount(string condition, params object[] parameters)//获取聊天信息数量
        {
            IZXTMessageViewDAO rDAO = CustomerManagementDAOFactory.CreateZXTMessageViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }
        public ZXTMessageViewVO FindZXTMessageByMessageID(int MessageID)//获取聊天信息
        {
            IZXTMessageViewDAO rDAO = CustomerManagementDAOFactory.CreateZXTMessageViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(MessageID);
        }
        public int setMessageStatus(int MessageFrom,int MessageTo)//已读所有聊天信息
        {
            IZXTMessageDAO rDAO = CustomerManagementDAOFactory.CreateZXTMessageDAO(this.CurrentCustomerProfile);
            try
            {
                return rDAO.Update("Status = 1", "MessageFrom = " + MessageFrom+ " and MessageTo = " + MessageTo);
            }
            catch
            {
                return -1;
            }
        }
        //获取最近聊天信息分页列表
        public List<ZXTMessageViewVO> FindLatelyMessagaeByPageIndex(string conditionStr, int limit, string sortcolname, string asc, params object[] parameters)
        {
            IZXTMessageViewDAO rDAO = CustomerManagementDAOFactory.CreateZXTMessageViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindLatelyMessagaeByPageIndex(conditionStr, limit, sortcolname, asc, parameters);
        }

        public bool UpdateZXTMessage(ZXTMessageVO vo)
        {
            IZXTMessageDAO rDAO = CustomerManagementDAOFactory.CreateZXTMessageDAO(this.CurrentCustomerProfile);

            try
            {
                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ZXTBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
    }
}

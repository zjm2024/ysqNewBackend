using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.CustomerManagement.BO
{
    public class MessageBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public MessageBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }

        public List<MessageTypeVO> FindAllMessageType()
        {
            IMessageTypeDAO mtDAO = CustomerManagementDAOFactory.CreateMessageTypeDAO(this.CurrentCustomerProfile);
            return mtDAO.FindByParams("1=1");
        }
        public List<MessageViewVO> FindAllMessageByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IMessageViewDAO uDAO = CustomerManagementDAOFactory.CreateMessageViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMessageTotalCount(string condition, params object[] parameters)
        {
            IMessageViewDAO uDAO = CustomerManagementDAOFactory.CreateMessageViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public List<MessageViewVO> FindMessageByCustomerId(int customerId, int messageType)
        {
            IMessageViewDAO uDAO = CustomerManagementDAOFactory.CreateMessageViewDAO(this.CurrentCustomerProfile);

            if (messageType > 0)
            {
                return uDAO.FindByParams("SendTo = " + customerId + " and MessageTypeId = " + messageType);
            }
            else
            {
                return uDAO.FindByParams("SendTo = " + customerId);
            }
        }

        public List<MessageViewVO> FindMessageByCustomerId(int customerId, int messageType, string sortcolname, string asc)
        {
            IMessageViewDAO uDAO = CustomerManagementDAOFactory.CreateMessageViewDAO(this.CurrentCustomerProfile);

            if (messageType > 0)
            {
                return uDAO.FindAllByPageIndex("SendTo = " + customerId + " and MessageTypeId = " + messageType, sortcolname, asc);
            }
            else
            {
                return uDAO.FindAllByPageIndex("SendTo = " + customerId, sortcolname, asc);
            }
        }

        public MessageViewVO FindMessageId(int messageId)
        {
            IMessageViewDAO uDAO = CustomerManagementDAOFactory.CreateMessageViewDAO(this.CurrentCustomerProfile);
            
            return uDAO.FindById(messageId);
        }

        public int AddMessage(MessageVO vo)
        {
            try
            {
                IMessageDAO aDAO = CustomerManagementDAOFactory.CreateMessageDAO(this.CurrentCustomerProfile);

                return aDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                strErrorMsg += "\r\n Message:" + vo.Message;
                strErrorMsg += "\r\n MessageId:" + vo.MessageId;
                strErrorMsg += "\r\n MessageTypeId:" + vo.MessageTypeId;
                strErrorMsg += "\r\n SendAt:" + vo.SendAt.ToString("yyyy-MM-dd HH:mm:ss");
                strErrorMsg += "\r\n SendTo:" + vo.SendTo;
                strErrorMsg += "\r\n Title:" + vo.Title;                
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateMessage(MessageVO vo)
        {
            IMessageDAO aDAO = CustomerManagementDAOFactory.CreateMessageDAO(this.CurrentCustomerProfile);

            try
            {
                aDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool DeleteMessage(int messageId)
        {
            IMessageDAO aDAO = CustomerManagementDAOFactory.CreateMessageDAO(this.CurrentCustomerProfile);

            try
            {
                aDAO.DeleteById(messageId);
                return true;               
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public int AddAlias(AliasVO vo)
        {
            try
            {
                IAliasDAO aDAO = CustomerManagementDAOFactory.CreateAliasDAO(this.CurrentCustomerProfile);
                //判断是否重复
                List<AliasVO> voList = aDAO.FindByParams("Alias = '" + vo.Alias + "'");
                if (voList.Count > 0)
                {
                    return voList[0].AliasId;
                }
                else
                    return aDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public int AddAliasMapping(string alias, int customerId)
        {
            IAliasDAO aDAO = CustomerManagementDAOFactory.CreateAliasDAO(this.CurrentCustomerProfile);

            try
            {
                //查找Alias是否存在，存在则更新，不存在则新增
                List<AliasVO> voList = aDAO.FindByParams("Alias = @Alias", new object[] { DbHelper.CreateParameter("@Alias", alias) });
                if(voList.Count > 0)
                {
                    //update
                    AliasVO vo = voList[0];
                    vo.CustomerId = customerId;
                    vo.IsAllowReceive = true;                    
                    aDAO.UpdateById(vo);
                    return vo.AliasId;
                }else
                {
                    //add
                    AliasVO vo = new AliasVO();
                    vo.Alias = alias;
                    vo.CustomerId = customerId;
                    vo.IsAllowReceive = true;
                    return AddAlias(vo);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }           
        }

        public bool DeleteAliasMapping(string alias, int customerId)
        {
            IAliasDAO aDAO = CustomerManagementDAOFactory.CreateAliasDAO(this.CurrentCustomerProfile);

            try
            {
                DbHelper.ExecuteNonQuery("Update T_CSC_Alias set CustomerId = null where Alias = @Alias", new object[] { DbHelper.CreateParameter("@Alias", alias) });
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool UpdateMessageStatusByMessageTypeId(int MessageTypeId, int customerId)
        {
            IAliasDAO aDAO = CustomerManagementDAOFactory.CreateAliasDAO(this.CurrentCustomerProfile);

            try
            {
                DbHelper.ExecuteNonQuery("Update t_csc_message set Status = 1 where SendTo = @customerId and MessageTypeId=@MessageTypeId", new object[] { DbHelper.CreateParameter("@MessageTypeId", MessageTypeId), DbHelper.CreateParameter("@customerId", customerId) });
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool PushMessage(MessageVO vo)
        {
            IAliasDAO aDAO = CustomerManagementDAOFactory.CreateAliasDAO(this.CurrentCustomerProfile);
            try
            {
                //查找Alias是否存在，存在则更新，不存在则新增
                List<AliasVO> voList = aDAO.FindByParams("CustomerId = @CustomerId", new object[] { DbHelper.CreateParameter("@CustomerId", vo.SendTo) });
                if (voList.Count > 0)
                {
                    //update
                    AliasVO avo = voList[0];
                    if (avo.IsAllowReceive)
                    {
                        Utilities.PushMessageToSingle(vo.Message, avo.Alias);
                        return true;
                    }
                    else return false;
                }
                else
                {
                    //add

                    return false;
               
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
            return true;
        }

        public void SendMessage(string title,string content,int customerId,MessageType messageType)
        {
            MessageVO mVO = new MessageVO();
            mVO.Title = title;
            mVO.Message = content;
            mVO.MessageTypeId = (int)messageType;
            mVO.SendTo = customerId;
            mVO.SendAt = DateTime.Now;
            mVO.Status = 0;

            int messageId = AddMessage(mVO);
            mVO.MessageId = messageId;

            //发送推送
            PushMessage(mVO);
        }
    }

    public enum MessageType
    {
        SYS = 1, Register = 2, Project = 3, RZSQ = 4, Tender = 5
    }
}

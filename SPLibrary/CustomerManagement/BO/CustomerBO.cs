using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.DAO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.CustomerManagement.BO
{
    public class CustomerBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public CustomerBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }

        public CustomerVO FindCustomerByLoginInfo(string loginName, string password)
        {
            //登录时判断账号或手机号码，并且是启用状态的会员。
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(new CustomerProfile());

            List<CustomerVO> voList = uDAO.FindByParams("CustomerAccount = @CustomerAccount and Password = @Password and Status = 1", new object[] { DbHelper.CreateParameter("@CustomerAccount", loginName), DbHelper.CreateParameter("@Password", password) });
            if (voList.Count > 0)
            {
                return voList[0];
            }
            else
            {
                voList = uDAO.FindByParams("Phone = @Phone and Password = @Password and Status = 1", new object[] { DbHelper.CreateParameter("@Phone", loginName), DbHelper.CreateParameter("@Password", password) });
                if (voList.Count > 0)
                {
                    return voList[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public int Add(CustomerVO vo)
        {
            try
            {
                ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
                return uDAO.Insert(vo);

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool Update(CustomerVO vo)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                //如果有环信账号，需要更新环信账号的昵称和头像
                IMBO imBO = new IMBO(this.CurrentCustomerProfile);
                CustomerIMVO cIMVO = imBO.GetIMCustomerByCustomer(vo.CustomerId);
                if (cIMVO.CustomerIMId > 0)
                {
                    cIMVO.NickName = vo.CustomerName;
                    cIMVO.HeaderLogo = vo.HeaderLogo;

                    imBO.UpdateIMCustomer(cIMVO);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool Delete(int customerId)
        {

            //不真正的删除，只是改状态

            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);

            try
            {
                CustomerVO vo = new CustomerVO();
                vo.CustomerId = customerId;
                vo.Status = -1;
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public CustomerViewVO FindById(int customerId)
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(customerId);
        }

        public CustomerVO FindCustomenById(Int64 customerId)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(customerId);
        }

        public int FindCustomerCount()
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount("1=1");
        }
        public int FindCustomerCount(string condition)
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition);
        }
        public int GetCustomerCount(string condition)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition);
        }
        public List<CustomerViewVO> FindByKeyword(string Keyword)
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindWhereByPageIndex("CustomerName LIKE '%"+ Keyword + "%' or AgencyName LIKE '%"+ Keyword + "%' or CompanyName LIKE '%" + Keyword + "%' limit 50");
        }

        public List<CustomerViewVO> FindByCondition(string condition)
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindWhereByPageIndex(condition);
        }

        public List<CustomerViewVO> FindByPhone(string Phone)
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindWhereByPageIndex("Phone = "+ Phone);
        }

        public BalanceVO FindBalanceByCustomerId(int customerId)
        {
            IBalanceDAO uDAO = CustomerManagementDAOFactory.CreateBalanceDAO(this.CurrentCustomerProfile);
            List<BalanceVO> voList = uDAO.FindByParams("CustomerId=" + customerId);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public CustomerVO FindByParams(string condition, params object[] parameters)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            List<CustomerVO> voList = uDAO.FindByParams(condition, parameters);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public List<CustomerVO> FindListByParams(string condition, params object[] parameters)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams(condition, parameters);
        }


        public List<CustomerViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public List<CustomerVO> FindAllByPageIndex2(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public List<ZxbConfigVO> FindZxbConfigByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IZxbConfigDAO uDAO = CustomerManagementDAOFactory.CreateZxbConfigDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            ICustomerViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public bool IsCustomerExist(CustomerVO vo)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            //允许LoginName和Phone都能够登录，所以全部要判断是否重复
            if (vo.CustomerId > 0)
            {
                List<CustomerVO> voList = uDAO.FindByParams("(CustomerAccount = @LoginName or Phone = @LoginName) and CustomerId <> @CustomerId", new object[] { DbHelper.CreateParameter("@LoginName", vo.CustomerAccount), DbHelper.CreateParameter("@CustomerId", vo.CustomerId) });
                return voList.Count > 0;
            }
            else
            {
                List<CustomerVO> voList = uDAO.FindByParams("CustomerAccount = @LoginName or Phone = @LoginName", new object[] { DbHelper.CreateParameter("@LoginName", vo.CustomerAccount) });
                return voList.Count > 0;
            }

        }

        public bool ChangePassword(int customerId, string password, string newPassword)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);
            if (uDAO.FindByParams("CustomerId = @CustomerId and Password = @Password", new object[] { DbHelper.CreateParameter("@CustomerId", customerId), DbHelper.CreateParameter("@Password", password) }).Count > 0)
            {
                try
                {
                    string sql = "update T_CSC_Customer set Password = @NewPassword where CustomerId = @CustomerId and Password = @Password";
                    DbHelper.ExecuteNonQuery(sql, new object[] { DbHelper.CreateParameter("@NewPassword", newPassword), DbHelper.CreateParameter("@CustomerId", customerId), DbHelper.CreateParameter("@Password", password) });
                    return true;
                }
                catch (Exception ex)
                {
                    LogBO _log = new LogBO(typeof(CustomerBO));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                    return false;
                }
            }
            return false;
        }
        public bool ChangeCustomerPassword(int customerId, string newPassword)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);

            try
            {
                string sql = "update T_CSC_Customer set Password = @NewPassword where CustomerId = @CustomerId";
                DbHelper.ExecuteNonQuery(sql, new object[] { DbHelper.CreateParameter("@NewPassword", newPassword), DbHelper.CreateParameter("@CustomerId", customerId) });
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool ChangeCustomerInvitationCustomerID(int customerId, int InvitationCustomerID)
        {
            ICustomerDAO uDAO = CustomerManagementDAOFactory.CreateCustomerDAO(this.CurrentCustomerProfile);

            try
            {
                string sql = "update T_CSC_Customer set InvitationCustomerID = @InvitationCustomerID where CustomerId = @CustomerId";
                DbHelper.ExecuteNonQuery(sql, new object[] { DbHelper.CreateParameter("@InvitationCustomerID", InvitationCustomerID), DbHelper.CreateParameter("@CustomerId", customerId) });
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool AddCustomerLoginHistory(CustomerLoginHistoryVO customerLoginVO)
        {
            ICustomerLoginHistoryDAO clDAO = CustomerManagementDAOFactory.CreateCustomerLoginHistoryDAO(new CustomerProfile());
            return clDAO.Insert(customerLoginVO) > 0;
        }

        public List<CustomerLoginHistoryVO> FindAllLoginHistoryByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICustomerLoginHistoryDAO clDAO = CustomerManagementDAOFactory.CreateCustomerLoginHistoryDAO(new CustomerProfile());
            return clDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindLoginHistoryTotalCount(string condition, params object[] parameters)
        {
            ICustomerLoginHistoryDAO clDAO = CustomerManagementDAOFactory.CreateCustomerLoginHistoryDAO(new CustomerProfile());
            return clDAO.FindTotalCount(condition, parameters);
        }

        public List<CustomerLoginHistoryVO> FindLoginHistoryByParams(string condition, params object[] parameters)
        {
            ICustomerLoginHistoryDAO clDAO = CustomerManagementDAOFactory.CreateCustomerLoginHistoryDAO(new CustomerProfile());
            return clDAO.FindByParams(condition, parameters);
        }

        public List<CustomerRecentLoginViewVO> GetCustomerRecentLoginView(string condtion)
        {
            ICustomerRecentLoginViewDAO bDAO = CustomerManagementDAOFactory.CustomerRecentLoginViewDAO(new UserProfile());
            return bDAO.FindByParams(condtion);
        }

        public bool AddViolation(ViolationVO ViolationVO)
        {
            IViolationDAO clDAO = CustomerManagementDAOFactory.ViolationDAO(new CustomerProfile());
            return clDAO.Insert(ViolationVO) > 0;
        }

        public List<ViolationVO> FindAllViolationByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IViolationDAO clDAO = CustomerManagementDAOFactory.ViolationDAO(new CustomerProfile());
            return clDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindViolationTotalCount(string condition, params object[] parameters)
        {
            IViolationDAO clDAO = CustomerManagementDAOFactory.ViolationDAO(new CustomerProfile());
            return clDAO.FindTotalCount(condition, parameters);
        }


        public string GetCustomerCode()
        {
            var customerCode = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                customerCode.Append(r.Next(0, 10));
            }
            return "C" + customerCode.ToString();
        }


        public int AddMark(MarkVO markVO)
        {
            try
            {
                IMarkDAO uDAO = CustomerManagementDAOFactory.CreateMarkDAO(this.CurrentCustomerProfile);
                return uDAO.Insert(markVO);

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }

        }

        public bool DeleteMark(MarkVO markVO)
        {
            try
            {
                IMarkDAO uDAO = CustomerManagementDAOFactory.CreateMarkDAO(this.CurrentCustomerProfile);
                uDAO.DeleteById(markVO.MarkId);
                return true;

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public int InsertCommissionIncome(CommissionIncomeVO ciVO)
        {
            try
            {
                ICommissionIncomeDAO uDAO = CustomerManagementDAOFactory.CreateCommissionIncomeDAO(this.CurrentCustomerProfile);
                return uDAO.Insert(ciVO);

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public MarkVO FindMark(int markObjectId, int markType, int customerId)
        {
            IMarkDAO uDAO = CustomerManagementDAOFactory.CreateMarkDAO(this.CurrentCustomerProfile);
            List<MarkVO> list = uDAO.FindByParams("CustomerId = " + customerId + " and MarkType = " + markType + " and MarkObjectId = " + markObjectId);
            if (list.Count > 0)
                return list[0];
            else
                return null;
        }

        public List<BankAccountVO> GetBankListByCustomerId(int customerId)
        {
            IBankAccountDAO _bDAO = CustomerManagementDAOFactory.CreateBankAccountDAO(this.CurrentCustomerProfile);
            List<BankAccountVO> bcList = _bDAO.FindByParams("CustomerId=" + customerId);
            return bcList;
        }

        public List<MarkAgencyViewVO> FindMarkAgencyAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IMarkAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMarkAgencyTotalCount(string condition, params object[] parameters)
        {
            IMarkAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public List<MarkBusinessViewVO> FindMarkBusinessAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IMarkBusinessViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkBusinessViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMarkBusinessTotalCount(string condition, params object[] parameters)
        {
            IMarkBusinessViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkBusinessViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public List<MarkProjectViewVO> FindMarkProjectAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IMarkProjectViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkProjectViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMarkProjectTotalCount(string condition, params object[] parameters)
        {
            IMarkProjectViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkProjectViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public List<MarkRequireViewVO> FindMarkRequireAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IMarkRequireViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkRequireViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMarkRequireTotalCount(string condition, params object[] parameters)
        {
            IMarkRequireViewDAO uDAO = CustomerManagementDAOFactory.CreateMarkRequireViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }


        public List<PayoutHistoryVO> FindPayoutHistoryAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IPayoutHistoryDAO uDAO = CustomerManagementDAOFactory.CreatePayoutHistoryDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindPayoutHistoryTotalCount(string condition, params object[] parameters)
        {
            IPayoutHistoryDAO uDAO = CustomerManagementDAOFactory.CreatePayoutHistoryDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }
        public decimal FindPayoutHistoryTotalCostSum(string condition, params object[] parameters)
        {
            IPayoutHistoryDAO uDAO = CustomerManagementDAOFactory.CreatePayoutHistoryDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCostSum(condition, parameters);
        }
        public List<PayinHistoryVO> FindPayinHistoryAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IPayinHistoryDAO uDAO = CustomerManagementDAOFactory.CreatePayinHistoryDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindPayinHistoryTotalCount(string condition, params object[] parameters)
        {
            IPayinHistoryDAO uDAO = CustomerManagementDAOFactory.CreatePayinHistoryDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public decimal FindPayinHistoryTotalCostSum(string condition, params object[] parameters)
        {
            IPayinHistoryDAO uDAO = CustomerManagementDAOFactory.CreatePayinHistoryDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCostSum(condition, parameters);
        }

        public List<CustomerPayOutHistoryViewVO> FindCustomerPayoutHistoryViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICustomerPayOutHistoryViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerPayOutHistoryViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindCustomerPayoutHistoryViewTotalCount(string condition, params object[] parameters)
        {
            ICustomerPayOutHistoryViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerPayOutHistoryViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }
        public List<CustomerPayInHistoryViewVO> FindCustomerPayInHistoryViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICustomerPayInHistoryViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerPayInHistoryViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindCustomerPayInHistoryViewTotalCount(string condition, params object[] parameters)
        {
            ICustomerPayInHistoryViewDAO uDAO = CustomerManagementDAOFactory.CreateCustomerPayInHistoryViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }



        public CustomerViewVO FindCustomerByOpenId(string openId, string loginType)
        {
            ICustomerMatchDAO cmDAO = CustomerManagementDAOFactory.CreateCustomerMatchDAO(new UserProfile());

            List<CustomerMatchVO> matchVOList = cmDAO.FindByParams("OpenId = @OpenId and MatchType = @MatchType", new object[] { DbHelper.CreateParameter("@OpenId", openId), DbHelper.CreateParameter("@MatchType", loginType) });
            if (matchVOList.Count > 0)
            {
                int customerId = matchVOList[0].CustomerId;

                return FindById(customerId);
            }
            else
            {
                return null;
            }
        }
        public CustomerViewVO FindCustomerByOpenId(string openId, string UnionID, string loginType,int AppType=1)
        {
            try
            {
                ICustomerMatchDAO cmDAO = CustomerManagementDAOFactory.CreateCustomerMatchDAO(new UserProfile());
                int customerId = 0;
                if (UnionID != "" && UnionID != null)
                {
                    List<CustomerMatchVO> matchVOList = cmDAO.FindByParams("UnionID = @UnionID and MatchType = @MatchType and AppType = @AppType", new object[] { DbHelper.CreateParameter("@UnionID", UnionID), DbHelper.CreateParameter("@MatchType", loginType), DbHelper.CreateParameter("@AppType", AppType) });
                    if (matchVOList.Count > 0)
                    {
                        customerId = matchVOList[0].CustomerId;
                    }
                }

                if (customerId == 0 && openId != "" && openId != null)
                {
                    List<CustomerMatchVO> matchVOList2 = cmDAO.FindByParams("OpenId = @OpenId and MatchType = @MatchType and AppType = @AppType", new object[] { DbHelper.CreateParameter("@OpenId", openId), DbHelper.CreateParameter("@MatchType", loginType), DbHelper.CreateParameter("@AppType", AppType) });
                    if (matchVOList2.Count > 0)
                    {
                        customerId = matchVOList2[0].CustomerId;
                    }
                }

                if (customerId > 0)
                {
                    return FindById(customerId);
                }else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        public List<CustomerMatchVO> FindCustomerMatch(int CustomerId)
        {
            ICustomerMatchDAO cmDAO = CustomerManagementDAOFactory.CreateCustomerMatchDAO(new UserProfile());
            List<CustomerMatchVO> bcList = cmDAO.FindByParams("CustomerId=" + CustomerId);
            return bcList;
        }
        public int AddCustomerMatch(CustomerMatchVO customerMatchVO)
        {
            try
            {
                if (customerMatchVO.OpenId == null && customerMatchVO.UnionID == null) {
                    return -1;
                }
                ICustomerMatchDAO cmDAO = CustomerManagementDAOFactory.CreateCustomerMatchDAO(new UserProfile());
                //判断是否存在，存在则更新
                List<CustomerMatchVO> voList = cmDAO.FindByParams("CustomerId = " + customerMatchVO.CustomerId+ " and MatchType= "+ customerMatchVO.MatchType);
                if (voList.Count > 0)
                {
                    cmDAO.UpdateByParams(customerMatchVO, "CustomerId = " + customerMatchVO.CustomerId);
                    return 1;
                }
                else
                    return cmDAO.Insert(customerMatchVO);

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool IsHasMoreBalance(int customerId, decimal totalCommission)
        {
            IBalanceDAO bDAO = CustomerManagementDAOFactory.CreateBalanceDAO(new UserProfile());
            List<BalanceVO> voList = bDAO.FindByParams("CustomerId = " + customerId);
            if (voList.Count > 0)
                return voList[0].Balance >= totalCommission;
            else
                return false;
        }

        public bool ReduceBalance(int customerId, decimal balance)
        {
            IBalanceDAO bDAO = CustomerManagementDAOFactory.CreateBalanceDAO(new UserProfile());
            return bDAO.ReduceBalance(customerId, balance);
        }

        public bool PlusBalance(int customerId, decimal balance)
        {
            IBalanceDAO bDAO = CustomerManagementDAOFactory.CreateBalanceDAO(new UserProfile());
            return bDAO.PlusBalance(customerId, balance);
        }
        public bool ZXBReduceBalance(int customerId, decimal balance, string str)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.ReduceBalance(customerId, balance, str);
        }
        public bool ZXBPlusBalance(int customerId, decimal balance, string str)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.PlusBalance(customerId, balance, str);
        }
        public bool ZXBAddrequire(int customerId, decimal balance, string str,int type)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.AddrequireZXB(customerId, balance, str, type);
        }
        public bool ZXBReceiveRequire(int ZXBrequireId)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.ReceiveRequireZXB(ZXBrequireId);
        }
        public bool ZXBdelRequire(int ZXBrequireId)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.delRequireZXB(ZXBrequireId);
        }
        public int ZXBFindRequireCount(string condition, params object[] parameters)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.FindTotalCount_Require(condition, parameters);
        }
        public decimal ZXBFindBalanceByCondition(string condition, params object[] parameters)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.GetBalanceByCondition(condition, parameters);
        }
        public List<zxbRequireVO> ZXBFindRequireAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.FindAllByPageIndex_Require(conditionStr, start, end, sortcolname, asc, parameters);
        }
        public List<zxbRequireVO> ZXBFindRequireAllByPageIndex(string conditionStr, params object[] parameters)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            return bDAO.FindAllByPageIndex_Require(conditionStr, parameters);
        }
        public List<ZxbConfigVO> ZXBAddrequirebyCode(string code)
        {
            IZxbConfigDAO uDAO = CustomerManagementDAOFactory.CreateZxbConfigDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams(" code = '"+ code+"'");
        }
        public ZxbConfigVO FindZxbConfigById(int ZxbConfigID)
        {
            IZxbConfigDAO uDAO = CustomerManagementDAOFactory.CreateZxbConfigDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(ZxbConfigID);
        }
        public bool UpdateZxbConfig(ZxbConfigVO vo)
        {
            try
            {
                IZxbConfigDAO rDAO = CustomerManagementDAOFactory.CreateZxbConfigDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public BalanceVO FindZXBBalanceByCustomerId(int customerId)
        {
            BalanceZXBDAO bDAO = CustomerManagementDAOFactory.CreateBalanceZXBDAO(new UserProfile());
            List<BalanceVO> voList = bDAO.FindByParams("CustomerId=" + customerId);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public int InsertPayinHistory(PayinHistoryVO vo)
        {
            IPayinHistoryDAO bDAO = CustomerManagementDAOFactory.CreatePayinHistoryDAO(new UserProfile());
            return bDAO.Insert(vo);
        }
        public void UpdatePayinHistory(PayinHistoryVO vo, string condtion)
        {
            IPayinHistoryDAO bDAO = CustomerManagementDAOFactory.CreatePayinHistoryDAO(new UserProfile());
            bDAO.UpdateByParams(vo, condtion);
        }


        public List<PayinHistoryVO> GetPayinHistoryVO(string condtion)
        {
            IPayinHistoryDAO bDAO = CustomerManagementDAOFactory.CreatePayinHistoryDAO(new UserProfile());
            return bDAO.FindByParams(condtion);
        }


        public List<CommissionIncomeViewVO> FindCommissionIncomeAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICommissionIncomeViewDAO uDAO = CustomerManagementDAOFactory.CreateCommissionIncomeViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }
        public int FindCommissionIncomeTotalCount(string condition, params object[] parameters)
        {
            ICommissionIncomeViewDAO uDAO = CustomerManagementDAOFactory.CreateCommissionIncomeViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }
        public decimal FindCommissionIncomeTotalCostSum(string condition, params object[] parameters)
        {
            ICommissionIncomeViewDAO uDAO = CustomerManagementDAOFactory.CreateCommissionIncomeViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCostSum(condition, parameters);
        }

        #region Project Report File
        public List<ToolFileVO> FindToolFileByCustomerId(int customerId, int typeId)
        {
            IToolFileDAO bcDAO = ProjectManagementDAOFactory.CreateToolFileDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("CreatedBy = " + customerId + " and TypeId=" + typeId);
        }
        public ToolFileVO FindToolFileById(int ToolFileId)
        {
            IToolFileDAO bcDAO = ProjectManagementDAOFactory.CreateToolFileDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(ToolFileId);
        }
        public int AddToolFile(ToolFileVO vo)
        {
            try
            {
                IToolFileDAO rDAO = ProjectManagementDAOFactory.CreateToolFileDAO(this.CurrentCustomerProfile);

                return rDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateToolFile(ToolFileVO vo)
        {
            try
            {
                IProjectFileDAO rDAO = ProjectManagementDAOFactory.CreateProjectFileDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteToolFile(int ToolFileId)
        {
            try
            {
                IToolFileDAO rDAO = ProjectManagementDAOFactory.CreateToolFileDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(ToolFileId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public BankAccountVO FindBankAccountById(int bankAccountId)
        {
            IBankAccountDAO bbo = CustomerManagementDAOFactory.CreateBankAccountDAO(this.CurrentCustomerProfile);
            return bbo.FindById(bankAccountId);
        }

        public int AddBankAccount(BankAccountVO bankAccountVO)
        {
            try
            {
                IBankAccountDAO bbo = CustomerManagementDAOFactory.CreateBankAccountDAO(this.CurrentCustomerProfile);
                return bbo.Insert(bankAccountVO);

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public int AddPayoutHistoryVO(PayoutHistoryVO payoutHistoryVO)
        {
            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    IPayoutHistoryDAO bbo = CustomerManagementDAOFactory.CreatePayoutHistoryDAO(this.CurrentCustomerProfile);
                    int PayoutHistoryId = bbo.Insert(payoutHistoryVO);

                    if (PayoutHistoryId > 0 && payoutHistoryVO.PayOutStatus == 0)//提交申请，必须余额；
                    {
                        ReduceBalance(payoutHistoryVO.CustomerId, payoutHistoryVO.Cost);
                    }
                    return PayoutHistoryId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public int HandleCustomerPayOut(PayoutHistoryVO payoutHistoryVO, int userId)
        {
            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    IPayoutHistoryDAO bbo = CustomerManagementDAOFactory.CreatePayoutHistoryDAO(this.CurrentCustomerProfile);
                    IHandlePayoutHistoryDAO hbo = CustomerManagementDAOFactory.CreateHandlePayoutHistoryDAO(this.CurrentCustomerProfile);
                    PayoutHistoryVO oldVO= bbo.FindById(payoutHistoryVO.PayOutHistoryId);
                    oldVO.HandleComment = payoutHistoryVO.HandleComment;
                    oldVO.ThirdOrder = payoutHistoryVO.ThirdOrder;
                    oldVO.PayOutStatus = payoutHistoryVO.PayOutStatus;
                    bbo.UpdateById(oldVO);

                    if ( payoutHistoryVO.PayOutStatus == -2)//提交申请，必须余额；
                    {
                       bool res= PlusBalance(oldVO.CustomerId, oldVO.Cost);
                        if (res == false)
                            return -1;
                    }


                    HandlePayoutHistoryVO hvo = new HandlePayoutHistoryVO();
                    hvo.PayOutHistoryId = payoutHistoryVO.PayOutHistoryId;
                    hvo.HandleStatus = payoutHistoryVO.PayOutStatus;
                    hvo.HandleDate = DateTime.Now;
                    hvo.HandleComment = payoutHistoryVO.HandleComment;
                    hvo.ThirdOrder = payoutHistoryVO.ThirdOrder;

                    int handlepayouthistoryId = hbo.Insert(hvo);


                    //发消息
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                 
                    if (payoutHistoryVO.PayOutStatus == 1)
                    {
                        CustomerBO _bo = new CustomerBO(new CustomerProfile());
                        //发放乐币
                        List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("提现");
                        if (_bo.ZXBFindRequireCount("CustomerId = " + oldVO.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                        {
                            //发放乐币奖励
                            _bo.ZXBAddrequire(oldVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                        }
                        mBO.SendMessage("会员提现申请处理通知", " 平台已经对您的提现申请处理！款项已经转账申请的银行卡，款项将在3到7个工作日内到账，请注意查收！", oldVO.CustomerId, MessageType.Project);
                    }
                    else if (payoutHistoryVO.PayOutStatus == -2)
                    {
                        mBO.SendMessage("会员提现申请处理通知", " 平台已经对您的提现申请处理！提现失败，失败原因:"+payoutHistoryVO.HandleComment+";申请金额已经退回平台钱包，如有疑问，请联系客服。", oldVO.CustomerId, MessageType.Project);
                    }

                    return handlepayouthistoryId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public CustomerPayOutHistoryViewVO GetCustomerPayOutHistoryView(int payoutHistoryId)
        {
            ICustomerPayOutHistoryViewDAO hbo = CustomerManagementDAOFactory.CreateCustomerPayOutHistoryViewDAO(this.CurrentCustomerProfile);
            List<CustomerPayOutHistoryViewVO> list= hbo.FindByParams("PayOutHistoryId="+payoutHistoryId);
            if (list == null || list.Count < 1)
                return null;
            else
                return list[0];

          
        }
        #endregion

        public string ViewAgencyPhone(int customerId, int agencyCustomerId)
        {
            ICustomerDAO cDAO = CustomerManagementDAOFactory.CreateCustomerDAO(new UserProfile());
            return cDAO.ViewAgencyPhone(customerId, agencyCustomerId);
        }
        public string ViewBusinessPhone(int customerId, int businessCustomerId)
        {
            ICustomerDAO cDAO = CustomerManagementDAOFactory.CreateCustomerDAO(new UserProfile());
            return cDAO.ViewBusinessPhone(customerId, businessCustomerId);
        }

        /// <summary>
        /// 添加签到
        /// </summary>
        public int AddSignIn(SignInVO signinVO)
        {
            try
            {
                ISignInDAO cDAO = CustomerManagementDAOFactory.CreateSignInDAO(new UserProfile());
                return cDAO.Insert(signinVO);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 添加根据条件查询签到数量
        /// </summary>
        public int FindSignInCount(string condition, params object[] parameters)
        {
            ISignInDAO uDAO = CustomerManagementDAOFactory.CreateSignInDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 更新签到
        /// </summary>
        public bool UpdateSignIn(SignInVO signinVO)
        {
            try
            {
                ISignInDAO rDAO = CustomerManagementDAOFactory.CreateSignInDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(signinVO);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除签到
        /// </summary>
        public bool DeleteSignIn(int SignInID)
        {
            try
            {
                ISignInDAO rDAO = CustomerManagementDAOFactory.CreateSignInDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(SignInID);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 按条件删除签到
        /// </summary>
        public bool DeleteSignInBycondition(string condition, params object[] parameters)
        {
            try
            {
                ISignInDAO rDAO = CustomerManagementDAOFactory.CreateSignInDAO(this.CurrentCustomerProfile);
                rDAO.DeleteSignInBycondition(condition,parameters);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 按条件更新签到
        /// </summary>
        public bool UpdateSignInBycondition(string Setdata, string condition, params object[] parameters)
        {
            try
            {
                ISignInDAO rDAO = CustomerManagementDAOFactory.CreateSignInDAO(this.CurrentCustomerProfile);
                rDAO.UpdateSignInBycondition(Setdata,condition,parameters);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 过滤掉导致小程序出错的字符
        /// </summary>
        public string GetFilterText(string s) {
            //s = s.Replace("\u0008", "\\u0008");
            //s = s.Replace("\u0009", "\\u0009");
            //s = s.Replace("\u000A", "\\u000A");
            //s = s.Replace("\u000B", "\\u000B");
            //s = s.Replace("\u000C", "\\u000C");
            //s = s.Replace("\u000D", "\\u000D");
            //s = s.Replace("\u0022", "\\u0022");
            //s = s.Replace("\u0027", "\\u0027");
            //s = s.Replace("\u005C", "\\u005C");
            //s = s.Replace("\u00A0", "\\u00A0");
            s = s.Replace("\u2028", "\\u2028");
            s = s.Replace("\u2029", "\\u2029");
            s = s.Replace("\uFEFF", "\\uFEFF");
            return s;
        }
        /// <summary>
        /// 首次托管金额限制金额
        /// </summary>
        public decimal GetFirstMandates(decimal commission)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            ConfigVO vo = sBO.FindConfig();
            return commission * (vo.FirstMandates / 100);
        }
        /// <summary>
        /// 首次托管金额限制百分比
        /// </summary>
        public decimal GetFirstMandates()
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            ConfigVO vo = sBO.FindConfig();
            return vo.FirstMandates;
        }
        /// <summary>
        /// 获取会员图片
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public string GetCustomerIMG(int CustomerId)
        {
            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            BusinessBO bBO = new BusinessBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO cVO = cBO.FindById(CustomerId);
            string Img = ConfigInfo.Instance.NoImg;

            string HeaderLogo = cVO.HeaderLogo;
            string CompanyLogo = "";
            string PersonalCard = "";

            if (cVO.BusinessId > 0)
            {
                BusinessViewVO bVO = bBO.FindBusinessById(cVO.BusinessId);
                CompanyLogo = bVO.CompanyLogo;
            }

            if (cVO.AgencyId > 0)
            {
                AgencyViewVO aVO = aBO.FindAgencyById(cVO.AgencyId);
                PersonalCard = aVO.PersonalCard;
            }

            if (HeaderLogo != "")
            {
                return HeaderLogo;
            }
            if (CompanyLogo != "")
            {
                return CompanyLogo;
            }
            if (PersonalCard != "")
            {
                return PersonalCard;
            }
            return Img;
        }
    }
}

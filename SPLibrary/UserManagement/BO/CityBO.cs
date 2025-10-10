using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.UserManagement.BO
{
    public class CityBO
    {
        private UserProfile CurrentUserProfile = new UserProfile();
        public CityBO(UserProfile userProfile)
        {
            this.CurrentUserProfile = userProfile;
        }
        public int ProvinceAdd(ProvinceVO vo,List<CityVO> cityVOList)
        {
            try
            {
                IProvinceDAO pDAO = UserManagementDAOFactory.CreateProvinceDAO(this.CurrentUserProfile);               
                
                ICityDAO cDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int provinceId = pDAO.Insert(vo); 
                    if (cityVOList != null)
                    {
                        foreach (CityVO cVO in cityVOList)
                        {
                            cVO.ProvinceId = provinceId;
                        }

                        cDAO.InsertList(cityVOList, 100);
                    }
                    return provinceId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CityBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool ProvinceUpdate(ProvinceVO vo, List<CityVO> cityVOList)
        {            
            try
            {
                IProvinceDAO pDAO = UserManagementDAOFactory.CreateProvinceDAO(this.CurrentUserProfile);
                ICityDAO cDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    pDAO.UpdateById(vo);

                    //更新存在的，添加新增的                    

                    if (cityVOList != null)
                    {
                        foreach (CityVO cVO in cityVOList)
                        {
                            if(cVO.CityId > 0)
                            {
                                cDAO.UpdateById(cVO);
                            }
                            else
                            {
                                cDAO.Insert(cVO);
                            }
                        }
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CityBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }        

        public ProvinceVO FindProvinceById(int provinceId)
        {
            IProvinceDAO pDAO = UserManagementDAOFactory.CreateProvinceDAO(this.CurrentUserProfile);
            return pDAO.FindById(provinceId);
        }

        public List<ProvinceVO> FindProvinceList(bool enable)
        {
            IProvinceDAO pDAO = UserManagementDAOFactory.CreateProvinceDAO(this.CurrentUserProfile);
            List<ProvinceVO> voList = pDAO.FindByParams((enable? " Status = true":""));
            return voList;
        }

        public List<ProvinceVO> FindProvinceAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IProvinceDAO pDAO = UserManagementDAOFactory.CreateProvinceDAO(this.CurrentUserProfile);
            return pDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindProvinceTotalCount(string condition, params object[] parameters)
        {
            IProvinceDAO pDAO = UserManagementDAOFactory.CreateProvinceDAO(this.CurrentUserProfile);
            return pDAO.FindTotalCount(condition, parameters);
        }

        public bool IsProvinceExist(ProvinceVO vo)
        {
            IProvinceDAO pDAO = UserManagementDAOFactory.CreateProvinceDAO(this.CurrentUserProfile);
            if (vo.ProvinceId > 0)
            {
                List<ProvinceVO> voList = pDAO.FindByParams("ProvinceName = @ProvinceName and ProvinceCode = @ProvinceCode and ProvinceId <> @ProvinceId", new object[] { DbHelper.CreateParameter("@ProvinceCode", vo.ProvinceCode), DbHelper.CreateParameter("@ProvinceName", vo.ProvinceName), DbHelper.CreateParameter("@ProvinceId", vo.ProvinceId) });
                return voList.Count > 0;
            }
            else
            {
                List<ProvinceVO> voList = pDAO.FindByParams("ProvinceName = @ProvinceName and ProvinceCode = @ProvinceCode", new object[] { DbHelper.CreateParameter("@ProvinceCode", vo.ProvinceCode), DbHelper.CreateParameter("@ProvinceName", vo.ProvinceName) });
                return voList.Count > 0;
            }
        }



        public int CityAdd(CityVO vo)
        {
            try
            {
                ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
                return pDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CityBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool CityUpdate(CityVO vo)
        {
            ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
            try
            {
                pDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CityBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<CityVO> FindCityByName(string cityname)
        {
            ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
            List<CityVO> voList = pDAO.FindByParams("CityName LIKE '%"+ cityname + "%'");
            return voList;
        }

        public CityVO FindCityById(int cityId)
        {
            ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
            return pDAO.FindById(cityId);
        }

        public List<CityVO> FindCityList(bool enable)
        {
            ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
            List<CityVO> voList = pDAO.FindByParams((enable ? "Status = true" : "1=1"));
            return voList;
        }

        public List<CityVO> FindCityByProvince(int provinceId, bool enable)
        {
            ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
            List<CityVO> voList = pDAO.FindByParams("ProvinceId = " + provinceId + (enable ? " AND Status = true" : ""));
            return voList;
        }

        public List<CityVO> FindCityAll(bool enable)
        {
            ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
            List<CityVO> voList = pDAO.FindByParams(" 1 = 1 " + (enable ? " AND Status = true" : ""));
            return voList;
        }

        public List<CityViewVO> FindCityAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICityViewDAO pDAO = UserManagementDAOFactory.CreateCityViewDAO(this.CurrentUserProfile);
            return pDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindCityTotalCount(string condition, params object[] parameters)
        {
            ICityViewDAO pDAO = UserManagementDAOFactory.CreateCityViewDAO(this.CurrentUserProfile);
            return pDAO.FindTotalCount(condition, parameters);
        }

        public bool IsCityExist(CityVO vo)
        {
            ICityDAO pDAO = UserManagementDAOFactory.CreateCityDAO(this.CurrentUserProfile);
            if (vo.ProvinceId > 0)
            {
                if (vo.CityId > 0)
                {
                    List<CityVO> voList = pDAO.FindByParams("CityName = @CityName and CityCode = @CityCode and ProvinceId = @ProvinceId and CityId <> @CityId", 
                        new object[] { DbHelper.CreateParameter("@CityCode", vo.CityCode),
                            DbHelper.CreateParameter("@CityName", vo.CityName),
                            DbHelper.CreateParameter("@ProvinceId", vo.ProvinceId),
                            DbHelper.CreateParameter("@CityId", vo.CityId) });
                    return voList.Count > 0;
                }
                else
                {
                    List<CityVO> voList = pDAO.FindByParams("CityName = @CityName and CityCode = @CityCode and ProvinceId = @ProvinceId", 
                        new object[] { DbHelper.CreateParameter("@CityCode", vo.CityCode),
                            DbHelper.CreateParameter("@ProvinceId", vo.ProvinceId),
                            DbHelper.CreateParameter("@CityName", vo.CityName) });
                    return voList.Count > 0;
                }
            }
            return false;    
        }
    }
}

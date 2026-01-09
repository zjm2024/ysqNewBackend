using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebUI.Common
{
    public static class SiteCommon
    {
        public static string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#TOKEN"].ToString();
                }
            }
        }

        public static CityVO GetCity(int cityId)
        {
            CityBO uBO = new CityBO(new UserProfile());
            return uBO.FindCityById(cityId);
        }

        public static CategoryVO GetCategory(int categoryId)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            return uBO.FindById(categoryId);
        }

        public static List<ProvinceVO> GetProvinceList()
        {
            CityBO uBO = new CityBO(new UserProfile());
            return uBO.FindProvinceList(true);
        }

        public static List<CategoryVO> GetParentCategoryList()
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            return uBO.FindParentCategoryList(true);
        }

        public static List<CityVO> GetCityList(int provinceId)
        {
            CityBO uBO = new CityBO(new UserProfile());
            //if(provinceId==-1|| provinceId == -2)
            //    return uBO.FindCityAll(true);
            //else
            return uBO.FindCityByProvince(provinceId, true);
        }

        public static List<CityVO> GetCityListAll()
        {
            CityBO uBO = new CityBO(new UserProfile());
            return uBO.FindCityAll(true);
        }

        public static List<CategoryVO> GetCategoryList(int parentCategoryId)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            //if(parentCategoryId==-1|| parentCategoryId == -2)
            //    return uBO.FindAllCategory(true);
            //else
            return uBO.FindCategoryByParent(parentCategoryId, true);
        }

        public static string GetHelpDocByTypeName(string typeName)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            HelpDocViewVO vo = sBO.FindHelpDocByTypeName(typeName);
            if (vo != null)
                return vo.Description;
            else
                return "";
        }

        public static List<HelpDocTypeVO> GetHelpDocTypeList(int parentHelpDocTypeId)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            return sBO.FindHelpDocTypeList(parentHelpDocTypeId, true);
        }

        public static HelpDocViewVO GetHelpDocByTypeId(int typeId)
        {
            SystemBO sBO = new SystemBO(new UserProfile());
            return sBO.FindHelpDocByType(typeId, true);
        }

        public static ResultObject ValidCustomerAccount(string userName, string pwd)
        {
            CustomerController customerCon = new CustomerController();
            ResultObject result = customerCon.ValidCustomerAccount(userName, pwd);
            return result;
        }

        public static CustomerViewVO GetCustomerById(int customerId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetCustomer(customerId, Token);
            CustomerViewVO vo = result.Result as CustomerViewVO;
            return vo;
        }

        public static BusinessViewVO GetBusiness(int businessId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetBusiness(businessId, Token);
            BusinessViewVO vo = result.Result as BusinessViewVO;
            return vo;
        }

        public static RequirementViewVO GetRequireById(int requireId)
        {
            RequireController requireCon = new RequireController();

            ResultObject result = requireCon.GetRequire(requireId, Token);
            RequirementViewVO voList = result.Result as RequirementViewVO;
            return voList;
        }

        public static List<TenderInviteViewVO> GetRequireTenderInviteByRequire(int requireId)
        {
            RequireController requireCon = new RequireController();

            ResultObject result = requireCon.GetRequireTenderInviteByRequire(requireId, Token);
            List<TenderInviteViewVO> voList = result.Result as List<TenderInviteViewVO>;
            return voList;

        }

        public static List<RequirementTargetCategoryViewVO> GetRequireCategoryByRequire(int requireId)
        {
            RequireController requireCon = new RequireController();

            ResultObject result = requireCon.GetRequireCategoryByRequire(requireId, Token);
            List<RequirementTargetCategoryViewVO> voList = result.Result as List<RequirementTargetCategoryViewVO>;
            return voList;

        }

        public static List<RequirementTargetCityViewVO> GetRequireCityByRequire(int requireId)
        {
            RequireController requireCon = new RequireController();

            ResultObject result = requireCon.GetRequireCityByRequire(requireId, Token);
            List<RequirementTargetCityViewVO> voList = result.Result as List<RequirementTargetCityViewVO>;
            return voList;
        }

        public static List<RequirementFileVO> GetRequireFileByRequire(int requireId)
        {
            RequireController requireCon = new RequireController();

            ResultObject result = requireCon.GetRequireFileByRequire(requireId, Token);
            List<RequirementFileVO> voList = result.Result as List<RequirementFileVO>;
            return voList;
        }

        public static BusinessApproveHistoryVO GetBusinessApproveInfo(int businessId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetBusinessApproveInfo(businessId, Token);
            BusinessApproveHistoryVO voList = result.Result as BusinessApproveHistoryVO;
            return voList;
        }

        public static List<BusinessCategoryViewVO> GetBusinessCategoryByBusiness(int businessId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetBusinessCategoryByBusiness(businessId, Token);
            List<BusinessCategoryViewVO> voList = result.Result as List<BusinessCategoryViewVO>;
            return voList;
        }


        public static List<BusinessIdcardVO> GetBusinessIdCardByBusiness(int businessId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetBusinessIdCardByBusiness(businessId, Token);
            List<BusinessIdcardVO> voList = result.Result as List<BusinessIdcardVO>;
            return voList;
        }


        public static List<TargetCategoryViewVO> GetTargetCategoryByBusiness(int businessId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetTargetCategoryByBusiness(businessId, Token);
            List<TargetCategoryViewVO> voList = result.Result as List<TargetCategoryViewVO>;
            return voList;
        }

        public static List<TargetCityViewVO> GetTargetCityByBusiness(int businessId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetTargetCityByBusiness(businessId, Token);
            List<TargetCityViewVO> voList = result.Result as List<TargetCityViewVO>;
            return voList;
        }

        public static List<BusinessClientVO> GetBusinessClientByBusiness(int businessId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetBusinessClientByBusiness(businessId, Token);
            List<BusinessClientVO> voList = result.Result as List<BusinessClientVO>;
            return voList;
        }

        public static AgencyViewVO GetAgency(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgency(agencyId, Token);
            AgencyViewVO voList = result.Result as AgencyViewVO;
            return voList;
        }
        public static AgencyApproveHistoryVO GetAgencyApproveInfo(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgencyApproveInfo(agencyId, Token);
            AgencyApproveHistoryVO voList = result.Result as AgencyApproveHistoryVO;
            return voList;
        }

        public static List<AgencyCategoryViewVO> GetAgencyCategoryByAgency(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgencyCategoryByAgency(agencyId, Token);
            List<AgencyCategoryViewVO> voList = result.Result as List<AgencyCategoryViewVO>;
            return voList;
        }

        public static List<AgencyCityViewVO> GetAgencyCityByAgency(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgencyCityByAgency(agencyId, Token);
            List<AgencyCityViewVO> voList = result.Result as List<AgencyCityViewVO>;
            return voList;
        }

        public static List<AgencyIdCardVO> GetAgencyIdCardByAgency(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgencyIdCardByAgency(agencyId, Token);
            List<AgencyIdCardVO> voList = result.Result as List<AgencyIdCardVO>;
            return voList;
        }

        public static List<AgencyTechnicalVO> GetAgencyTechnicalByAgency(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgencyTechnicalByAgency(agencyId, Token);
            List<AgencyTechnicalVO> voList = result.Result as List<AgencyTechnicalVO>;
            return voList;
        }

        public static List<AgencySuperClientVO> GetAgencySuperClientByAgency(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgencySuperClientByAgency(agencyId, Token);
            List<AgencySuperClientVO> voList = result.Result as List<AgencySuperClientVO>;
            return voList;
        }

        public static List<AgencySolutionVO> GetAgencySolutionByAgency(int agencyId)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetAgencySolutionByAgency(agencyId, Token);
            List<AgencySolutionVO> voList = result.Result as List<AgencySolutionVO>;
            return voList;
        }

        public static ResultObject GetCodeURL(string out_trade_no, string productId, string total_fee)
        {
            ProjectController customerCon = new ProjectController();

            ResultObject result = customerCon.GetCodeURL(out_trade_no, productId, total_fee, Token);

            return result;
        }

        public static List<BankAccountVO> GetBankListByCustomerId(int CustomerId, string Token)
        {
            CustomerController customerCon = new CustomerController();

            ResultObject result = customerCon.GetBankListByCustomerId(CustomerId, Token);
            List<BankAccountVO> voList = result.Result as List<BankAccountVO>;
            return voList;
        }

        public static List<RequirementTargetClientVO> GetRequireClientByBusiness(int requireId)
        {
            RequireController customerCon = new RequireController();

            ResultObject result = customerCon.GetRequireClientByRequire(requireId, Token);
            List<RequirementTargetClientVO> voList = result.Result as List<RequirementTargetClientVO>;
            return voList;
        }
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
    }
}
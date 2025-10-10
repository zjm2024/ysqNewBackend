using System;
using System.Collections.Generic;
using System.Text;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
	public partial class RequireManagementDAOFactory
	{	
		
		public static IRequirementDAO CreateRequirementDAO(UserProfile userProfile)
		{
			return new RequirementDAO(userProfile);
		}

        public static IDemandDAO CreateDemandDAO(UserProfile userProfile)
        {
            return new DemandDAO(userProfile);
        }

        public static IDemandViewDAO CreateDemandViewDAO(UserProfile userProfile)
        {
            return new DemandViewDAO(userProfile);
        }

        public static IDemandCategoryDAO CreateDemandCategoryDAO(UserProfile userProfile)
        {
            return new DemandCategoryDAO(userProfile);
        }

        public static IDemandOfferDAO CreateDemandOfferDAO(UserProfile userProfile)
        {
            return new DemandOfferDAO(userProfile);
        }

        public static IRequirementFileDAO CreateRequirementFileDAO(UserProfile userProfile)
		{
			return new RequirementFileDAO(userProfile);
		}

        public static IRequirementCopiesDAO CreateRequirementCopiesDAO(UserProfile userProfile)
        {
            return new RequirementCopiesDAO(userProfile);
        }

        public static IRequirementTargetCategoryDAO CreateRequirementTargetCategoryDAO(UserProfile userProfile)
		{
			return new RequirementTargetCategoryDAO(userProfile);
		}
		
		public static IRequirementTargetCityDAO CreateRequirementTargetCityDAO(UserProfile userProfile)
		{
			return new RequirementTargetCityDAO(userProfile);
		}
		
		public static ITenderInfoDAO CreateTenderInfoDAO(UserProfile userProfile)
		{
			return new TenderInfoDAO(userProfile);
		}
		
		public static ITenderInviteDAO CreateTenderInviteDAO(UserProfile userProfile)
		{
			return new TenderInviteDAO(userProfile);
		}
		
		public static IRequirementTargetCategoryViewDAO CreateRequirementTargetCategoryViewDAO(UserProfile userProfile)
		{
			return new RequirementTargetCategoryViewDAO(userProfile);
		}
		
		public static IRequirementTargetCityViewDAO CreateRequirementTargetCityViewDAO(UserProfile userProfile)
		{
			return new RequirementTargetCityViewDAO(userProfile);
		}
		
		public static IRequirementViewDAO CreateRequirementViewDAO(UserProfile userProfile)
		{
			return new RequirementViewDAO(userProfile);
		}
		
		public static ITenderInfoViewDAO CreateTenderInfoViewDAO(UserProfile userProfile)
		{
			return new TenderInfoViewDAO(userProfile);
		}
		
		public static ITenderInviteViewDAO CreateTenderInviteViewDAO(UserProfile userProfile)
		{
			return new TenderInviteViewDAO(userProfile);
		}

        public static IServicesCategoryViewDAO CreateServicesCategoryViewDAO(UserProfile userProfile)
        {
            return new ServicesCategoryViewDAO(userProfile);
        }

        public static IServicesViewDAO CreateServicesViewDAO(UserProfile userProfile)
        {
            return new ServicesViewDAO(userProfile);
        }

        public static IServicesDAO CreateServicesDAO(UserProfile userProfile)
        {
            return new ServicesDAO(userProfile);
        }

        public static IServicesCategoryDAO CreateServicesCategoryDAO(UserProfile userProfile)
        {
            return new ServicesCategoryDAO(userProfile);
        }
        public static ITenderInfoRequirementViewDAO CreateTenderInfoRequirementViewDAO(UserProfile userProfile)
        {
            return new TenderInfoRequirementViewDAO(userProfile);
        }

        public static ITenderInviteRequirementViewDAO CreateTenderInviteRequirementViewDAO(UserProfile userProfile)
        {
            return new TenderInviteRequirementViewDAO(userProfile);
        }

        public static IRequireCommissionDelegationDAO CreateRequireCommissionDelegationDAO(UserProfile userProfile)
        {
            return new RequireCommissionDelegationDAO(userProfile);
        }

        public static IRequireCommissionDelegationviewDAO CreateRequireCommissionDelegationviewDAO(UserProfile userProfile)
        {
            return new RequireCommissionDelegationviewDAO(userProfile);
        }

        public static IRequirementTargetClientDAO CreateRequirementTargetClientDAO(UserProfile userProfile)
        {
            return new RequirementTargetClientDAO(userProfile);
        }
    }
}

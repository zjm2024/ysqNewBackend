using System;
using System.Collections.Generic;
using System.Text;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
	public partial class UserManagementDAOFactory
	{	
		
		public static IDepartmentDAO CreateDepartmentDAO(UserProfile userProfile)
		{
			return new DepartmentDAO(userProfile);
		}
		
		public static IRoleDAO CreateRoleDAO(UserProfile userProfile)
		{
			return new RoleDAO(userProfile);
		}
		
		public static IRoleSecurityDAO CreateRoleSecurityDAO(UserProfile userProfile)
		{
			return new RoleSecurityDAO(userProfile);
		}
				
		public static ISecurityDAO CreateSecurityDAO(UserProfile userProfile)
		{
			return new SecurityDAO(userProfile);
		}
		
		public static ISecurityTypeDAO CreateSecurityTypeDAO(UserProfile userProfile)
		{
			return new SecurityTypeDAO(userProfile);
		}
		
		public static IUserDAO CreateUserDAO(UserProfile userProfile)
		{
			return new UserDAO(userProfile);
		}
		
		public static IUserSecurityDAO CreateUserSecurityDAO(UserProfile userProfile)
		{
			return new UserSecurityDAO(userProfile);
		}		

        public static IUserSecurityViewDAO CreateUserSecurityViewDAO(UserProfile userProfile)
        {
            return new UserSecurityViewDAO(userProfile);
        }

        public static IDepartmentViewDAO CreateDepartmentViewDAO(UserProfile userProfile)
        {
            return new DepartmentViewDAO(userProfile);
        }

        public static IUserViewDAO CreateUserViewDAO(UserProfile userProfile)
        {
            return new UserViewDAO(userProfile);
        }

        public static IRoleViewDAO CreateRoleViewDAO(UserProfile userProfile)
        {
            return new RoleViewDAO(userProfile);
        }

        public static IRoleSecurityViewDAO CreateRoleSecurityViewDAO(UserProfile userProfile)
        {
            return new RoleSecurityViewDAO(userProfile);
        }
        
        public static ICompanyDAO CreateCompanyDAO(UserProfile userProfile)
        {
            return new CompanyDAO(userProfile);
        }

        public static IGroupTypeDAO CreateGroupTypeDAO(UserProfile userProfile)
        {
            return new GroupTypeDAO(userProfile);
        }

        public static ISecurityActionDAO CreateSecurityActionDAO(UserProfile userProfile)
        {
            return new SecurityActionDAO(userProfile);
        }

        public static IUserRoleDAO CreateUserRoleDAO(UserProfile userProfile)
        {
            return new UserRoleDAO(userProfile);
        }

        public static ISecurityTypeViewDAO CreateSecurityTypeViewDAO(UserProfile userProfile)
        {
            return new SecurityTypeViewDAO(userProfile);
        }

        public static ISecurityViewDAO CreateSecurityViewDAO(UserProfile userProfile)
        {
            return new SecurityViewDAO(userProfile);
        }

        public static IConfigDAO CreateConfigDAO(UserProfile userProfile)
        {
            return new ConfigDAO(userProfile);
        }

        public static ITokenDAO CreateTokenDAO(UserProfile userProfile)
        {
            return new TokenDAO(userProfile);
        }

        public static IUserRoleViewDAO CreateUserRoleViewDAO(UserProfile userProfile)
        {
            return new UserRoleViewDAO(userProfile);
        }

        public static IHelpDocDAO CreateHelpDocDAO(UserProfile userProfile)
        {
            return new HelpDocDAO(userProfile);
        }

        public static IHelpDocTypeDAO CreateHelpDocTypeDAO(UserProfile userProfile)
        {
            return new HelpDocTypeDAO(userProfile);
        }

        public static IHelpDocViewDAO CreateHelpDocViewDAO(UserProfile userProfile)
        {
            return new HelpDocViewDAO(userProfile);
        }

        public static ICityDAO CreateCityDAO(UserProfile userProfile)
        {
            return new CityDAO(userProfile);
        }

        public static IProvinceDAO CreateProvinceDAO(UserProfile userProfile)
        {
            return new ProvinceDAO(userProfile);
        }

        public static ICityViewDAO CreateCityViewDAO(UserProfile userProfile)
        {
            return new CityViewDAO(userProfile);
        }

        public static ICategoryDAO CreateCategoryDAO(UserProfile userProfile)
        {
            return new CategoryDAO(userProfile);
        }

        public static ICarouselDAO CreateCarouselDAO(UserProfile userProfile)
        {
            return new CarouselDAO(userProfile);
        }

        public static IUserLoginHistoryDAO CreateUserLoginHistoryDAO(UserProfile userProfile)
        {
            return new UserLoginHistoryDAO(userProfile);
        }

        public static ISuggestionDAO CreateSuggestionDAO(UserProfile userProfile)
        {
            return new SuggestionDAO(userProfile);
        }

        public static ICategoryViewDAO CreateCategoryViewDAO(UserProfile userProfile)
        {
            return new CategoryViewDAO(userProfile);
        }

        public static ISystemMessageDAO CreateSystemMessageDAO(UserProfile userProfile)
        {
            return new SystemMessageDAO(userProfile);
        }

        public static ISystemMessageViewDAO CreateSystemMessageViewDAO(UserProfile userProfile)
        {
            return new SystemMessageViewDAO(userProfile);
        }

        public static ICommissionDAO CreateCommissionDAO(UserProfile userProfile)
        {
            return new CommissionDAO(userProfile);
        }

        public static IRecommendAgencyDAO CreateRecommendAgencyDAO(UserProfile userProfile)
        {
            return new RecommendAgencyDAO(userProfile);
        }

        public static IRecommendRequireDAO CreateRecommendRequireDAO(UserProfile userProfile)
        {
            return new RecommendRequireDAO(userProfile);
        }

        public static IRecommendAgencyViewDAO CreateRecommendAgencyViewDAO(UserProfile userProfile)
        {
            return new RecommendAgencyViewDAO(userProfile);
        }

        public static IRecommendRequireViewDAO CreateRecommendRequireViewDAO(UserProfile userProfile)
        {
            return new RecommendRequireViewDAO(userProfile);
        }
        public static IProjectCommissionViewDAO CreateProjectCommissionViewDAO(UserProfile userProfile)
        {
            return new ProjectCommissionViewDAO(userProfile);
        }
    }
}

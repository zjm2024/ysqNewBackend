using System;
using System.Collections.Generic;
using System.Text;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
	public partial class ProjectManagementDAOFactory
	{	
		
		public static IAgencyReviewDAO CreateAgencyReviewDAO(UserProfile userProfile)
		{
			return new AgencyReviewDAO(userProfile);
		}
		
		public static IAgencyReviewDetailDAO CreateAgencyReviewDetailDAO(UserProfile userProfile)
		{
			return new AgencyReviewDetailDAO(userProfile);
		}
		
		public static IBusinessReviewDAO CreateBusinessReviewDAO(UserProfile userProfile)
		{
			return new BusinessReviewDAO(userProfile);
		}
		
		public static IBusinessReviewDetailDAO CreateBusinessReviewDetailDAO(UserProfile userProfile)
		{
			return new BusinessReviewDetailDAO(userProfile);
		}
		
		public static ICommissionDelegationDAO CreateCommissionDelegationDAO(UserProfile userProfile)
		{
			return new CommissionDelegationDAO(userProfile);
		}
        public static ICommissionDelegationViewDAO CreateCommissionDelegationViewDAO(UserProfile userProfile)
        {
            return new CommissionDelegationViewDAO(userProfile);
        }

        public static IComplaintsDAO CreateComplaintsDAO(UserProfile userProfile)
		{
			return new ComplaintsDAO(userProfile);
		}
		
		public static IComplaintsImgDAO CreateComplaintsImgDAO(UserProfile userProfile)
		{
			return new ComplaintsImgDAO(userProfile);
		}
		
		public static IProjectDAO CreateProjectDAO(UserProfile userProfile)
		{
			return new ProjectDAO(userProfile);
		}
		
		public static IProjectActionDAO CreateProjectActionDAO(UserProfile userProfile)
		{
			return new ProjectActionDAO(userProfile);
		}
		
		public static IProjectCommissionDAO CreateProjectCommissionDAO(UserProfile userProfile)
		{
			return new ProjectCommissionDAO(userProfile);
		}

        public static IProjectChangeDAO CreateProjectChangeDAO(UserProfile userProfile)
        {
            return new ProjectChangeDAO(userProfile);
        }

        public static IProjectRefundDAO CreateProjectRefundDAO(UserProfile userProfile)
        {
            return new ProjectRefundDAO(userProfile);
        }

        public static IProjectFileDAO CreateProjectFileDAO(UserProfile userProfile)
		{
			return new ProjectFileDAO(userProfile);
		}
		
		public static IProjectRefundsDAO CreateProjectRefundsDAO(UserProfile userProfile)
		{
			return new ProjectRefundsDAO(userProfile);
		}
		
		public static IComplaintsViewDAO CreateComplaintsViewDAO(UserProfile userProfile)
		{
			return new ComplaintsViewDAO(userProfile);
		}
		
		public static IProjectActionViewDAO CreateProjectActionViewDAO(UserProfile userProfile)
		{
			return new ProjectActionViewDAO(userProfile);
		}
		
		public static IProjectCommissionViewDAO CreateProjectCommissionViewDAO(UserProfile userProfile)
		{
			return new ProjectCommissionViewDAO(userProfile);
		}
		
		public static IProjectFileViewDAO CreateProjectFileViewDAO(UserProfile userProfile)
		{
			return new ProjectFileViewDAO(userProfile);
		}
		
		public static IProjectRefundsViewDAO CreateProjectRefundsViewDAO(UserProfile userProfile)
		{
			return new ProjectRefundsViewDAO(userProfile);
		}
		
		public static IProjectViewDAO CreateProjectViewDAO(UserProfile userProfile)
		{
			return new ProjectViewDAO(userProfile);
		}
        public static IProjectReportFileDAO CreateProjectReportFileDAO(UserProfile userProfile)
        {
            return new ProjectReportFileDAO(userProfile);
        }

        public static IToolFileDAO CreateToolFileDAO(UserProfile userProfile)
        {
            return new ToolFileDAO(userProfile);
        }

        public static IProjectActionFileDAO CreateProjectActionFileDAO(UserProfile userProfile)
        {
            return new ProjectActionFileDAO(userProfile);
        }

        public static IContractDAO CreateContractDAO(UserProfile userProfile)
        {
            return new ContractDAO(userProfile);
        }

        public static IContractFileDAO CreateContractFileDAO(UserProfile userProfile)
        {
            return new ContractFileDAO(userProfile);
        }

        public static IContractStepsDAO CreateContractStepsDAO(UserProfile userProfile)
        {
            return new ContractStepsDAO(userProfile);
        }

        public static IContractViewDAO CreateContractViewDAO(UserProfile userProfile)
        {
            return new ContractViewDAO(userProfile);
        }

        public static IAgencySumReviewDAO CreateAgencySumReviewDAO(UserProfile userProfile)
        {
            return new AgencySumReviewDAO(userProfile);
        }


        public static IBusinessSumReviewDAO CreateBusinessSumReviewDAO(UserProfile userProfile)
        {
            return new BusinessSumReviewDAO(userProfile);
        }
        public static IAllAgencyAverageScoreViewDAO CreateAllAgencyAverageScoreViewDAO(UserProfile userProfile)
        {
            return new AllAgencyAverageScoreViewDAO(userProfile);
        }

        public static IAllBusinessAverageScoreViewDAO CreateAllBusinessAverageScoreViewDAO(UserProfile userProfile)
        {
            return new AllBusinessAverageScoreViewDAO(userProfile);
        }

        public static IAgencyReviewViewDAO CreateAgencyReviewViewDAO(UserProfile userProfile)
        {
            return new AgencyReviewViewDAO(userProfile);
        }

        public static IBusinessReviewViewDAO CreateBusinessReviewViewDAO(UserProfile userProfile)
        {
            return new BusinessReviewViewDAO(userProfile);
        }

    }
}

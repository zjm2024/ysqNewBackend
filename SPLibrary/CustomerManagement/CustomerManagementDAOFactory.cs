using System;
using System.Collections.Generic;
using System.Text;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
	public partial class CustomerManagementDAOFactory
	{	
		
		public static ICustomerDAO CreateCustomerDAO(UserProfile userProfile)
		{
			return new CustomerDAO(userProfile);
		}

        public static ICustomerViewDAO CreateCustomerViewDAO(UserProfile userProfile)
        {
            return new CustomerViewDAO(userProfile);
        }

        public static ICustomerLoginHistoryDAO CreateCustomerLoginHistoryDAO(UserProfile userProfile)
        {
            return new CustomerLoginHistoryDAO(userProfile);
        }

        public static IBusinessDAO CreateBusinessDAO(UserProfile userProfile)
        {
            return new BusinessDAO(userProfile);
        }

        public static IBusinessApproveHistoryDAO CreateBusinessApproveHistoryDAO(UserProfile userProfile)
        {
            return new BusinessApproveHistoryDAO(userProfile);
        }

        public static IBusinessCategoryDAO CreateBusinessCategoryDAO(UserProfile userProfile)
        {
            return new BusinessCategoryDAO(userProfile);
        }

        public static ITargetCategoryDAO CreateTargetCategoryDAO(UserProfile userProfile)
        {
            return new TargetCategoryDAO(userProfile);
        }

        public static IBusinessViewDAO CreateBusinessViewDAO(UserProfile userProfile)
        {
            return new BusinessViewDAO(userProfile);
        }

        public static ITargetCityDAO CreateTargetCityDAO(UserProfile userProfile)
        {
            return new TargetCityDAO(userProfile);
        }

        public static IBusinessCategoryViewDAO CreateBusinessCategoryViewDAO(UserProfile userProfile)
        {
            return new BusinessCategoryViewDAO(userProfile);
        }

        public static ITargetCategoryViewDAO CreateTargetCategoryViewDAO(UserProfile userProfile)
        {
            return new TargetCategoryViewDAO(userProfile);
        }

        public static ITargetCityViewDAO CreateTargetCityViewDAO(UserProfile userProfile)
        {
            return new TargetCityViewDAO(userProfile);
        }

        public static IAgencyDAO CreateAgencyDAO(UserProfile userProfile)
        {
            return new AgencyDAO(userProfile);
        }

        public static IAgencyApproveHistoryDAO CreateAgencyApproveHistoryDAO(UserProfile userProfile)
        {
            return new AgencyApproveHistoryDAO(userProfile);
        }

        public static IAgencyCategoryDAO CreateAgencyCategoryDAO(UserProfile userProfile)
        {
            return new AgencyCategoryDAO(userProfile);
        }

        public static IAgencyCityDAO CreateAgencyCityDAO(UserProfile userProfile)
        {
            return new AgencyCityDAO(userProfile);
        }

        public static IAgencyExperienceDAO CreateAgencyExperienceDAO(UserProfile userProfile)
        {
            return new AgencyExperienceDAO(userProfile);
        }

        public static IAgencyExperienceViewDAO CreateAgencyExperienceViewDAO(UserProfile userProfile)
        {
            return new AgencyExperienceViewDAO(userProfile);
        }

        public static IAgencyExperienceImageDAO CreateAgencyExperienceImageDAO(UserProfile userProfile)
        {
            return new AgencyExperienceImageDAO(userProfile);
        }

        public static IAgencyIdCardDAO CreateAgencyIdCardDAO(UserProfile userProfile)
        {
            return new AgencyIdCardDAO(userProfile);
        }

        public static IAgencyTechnicalDAO CreateAgencyTechnicalDAO(UserProfile userProfile)
        {
            return new AgencyTechnicalDAO(userProfile);
        }

        public static IAgencyCategoryViewDAO CreateAgencyCategoryViewDAO(UserProfile userProfile)
        {
            return new AgencyCategoryViewDAO(userProfile);
        }

        public static IAgencyCityViewDAO CreateAgencyCityViewDAO(UserProfile userProfile)
        {
            return new AgencyCityViewDAO(userProfile);
        }

        public static IAgencyViewDAO CreateAgencyViewDAO(UserProfile userProfile)
        {
            return new AgencyViewDAO(userProfile);
        }

        public static IMarkDAO CreateMarkDAO(UserProfile userProfile)
        {
            return new MarkDAO(userProfile);
        }

        public static IMarkAgencyViewDAO CreateMarkAgencyViewDAO(UserProfile userProfile)
        {
            return new MarkAgencyViewDAO(userProfile);
        }

        public static IMarkBusinessViewDAO CreateMarkBusinessViewDAO(UserProfile userProfile)
        {
            return new MarkBusinessViewDAO(userProfile);
        }

        public static IMarkRequireViewDAO CreateMarkRequireViewDAO(UserProfile userProfile)
        {
            return new MarkRequireViewDAO(userProfile);
        }

        public static IMarkServicesViewDAO CreateMarkServicesViewDAO(UserProfile userProfile)
        {
            return new MarkServicesViewDAO(userProfile);
        }

        public static IAliasDAO CreateAliasDAO(UserProfile userProfile)
        {
            return new AliasDAO(userProfile);
        }

        public static IMessageDAO CreateMessageDAO(UserProfile userProfile)
        {
            return new MessageDAO(userProfile);
        }

        public static IMessageTypeDAO CreateMessageTypeDAO(UserProfile userProfile)
        {
            return new MessageTypeDAO(userProfile);
        }

        public static IMessageViewDAO CreateMessageViewDAO(UserProfile userProfile)
        {
            return new MessageViewDAO(userProfile);
        }

        public static ICustomerMatchDAO CreateCustomerMatchDAO(UserProfile userProfile)
        {
            return new CustomerMatchDAO(userProfile);
        }

        public static IBalanceDAO CreateBalanceDAO(UserProfile userProfile)
        {
            return new BalanceDAO(userProfile);
        }

        public static BalanceZXBDAO CreateBalanceZXBDAO(UserProfile userProfile)
        {
            return new BalanceZXBDAO(userProfile);
        }

        public static IPayinHistoryDAO CreatePayinHistoryDAO(UserProfile userProfile)
        {
            return new PayinHistoryDAO(userProfile);
        }

        public static IPayoutHistoryDAO CreatePayoutHistoryDAO(UserProfile userProfile)
        {
            return new PayoutHistoryDAO(userProfile);
        }

        public static IAgencySolutionDAO CreateAgencySolutionDAO(UserProfile userProfile)
        {
            return new AgencySolutionDAO(userProfile);
        }

        public static IAgencySolutionFileDAO CreateAgencySolutionFileDAO(UserProfile userProfile)
        {
            return new AgencySolutionFileDAO(userProfile);
        }

        public static IAgencySuperClientDAO CreateAgencySuperClientDAO(UserProfile userProfile)
        {
            return new AgencySuperClientDAO(userProfile);
        }

        public static IBusinessClientDAO CreateBusinessClientDAO(UserProfile userProfile)
        {
            return new BusinessClientDAO(userProfile);
        }

        public static IMarkProjectViewDAO CreateMarkProjectViewDAO(UserProfile userProfile)
        {
            return new MarkProjectViewDAO(userProfile);
        }
        public static ICommissionIncomeDAO CreateCommissionIncomeDAO(UserProfile userProfile)
        {
            return new CommissionIncomeDAO(userProfile);
        }
        public static ICommissionIncomeViewDAO CreateCommissionIncomeViewDAO(UserProfile userProfile)
        {
            return new CommissionIncomeViewDAO(userProfile);
        }
        public static ICustomerIMDAO CreateCustomerIMDAO(UserProfile userProfile)
        {
            return new CustomerIMDAO(userProfile);
        }

        public static ICustomerIMGroupDAO CreateCustomerIMGroupDAO(UserProfile userProfile)
        {
            return new CustomerIMGroupDAO(userProfile);
        }

        public static ICustomerIMGroupUserDAO CreateCustomerIMGroupUserDAO(UserProfile userProfile)
        {
            return new CustomerIMGroupUserDAO(userProfile);
        }

        public static IIMMessageDAO CreateIMMessageDAO(UserProfile userProfile)
        {
            return new IMMessageDAO(userProfile);
        }

        public static IIMTokenDAO CreateIMTokenDAO(UserProfile userProfile)
        {
            return new IMTokenDAO(userProfile);
        }

        public static IRosterViewDAO CreateRosterViewDAO(UserProfile userProfile)
        {
            return new RosterViewDAO(userProfile);
        }
        public static ICustomerPayInHistoryViewDAO CreateCustomerPayInHistoryViewDAO(UserProfile userProfile)
        {
            return new CustomerPayInHistoryViewDAO(userProfile);
        }

        public static ICustomerPayOutHistoryViewDAO CreateCustomerPayOutHistoryViewDAO(UserProfile userProfile)
        {
            return new CustomerPayOutHistoryViewDAO(userProfile);
        }

        public static IAgencyExperienceApproveHistoryDAO CreateAgencyExperienceApproveHistoryDAO(UserProfile userProfile)
        {
            return new AgencyExperienceApproveHistoryDAO(userProfile);
        }
        public static IBankAccountDAO CreateBankAccountDAO(UserProfile userProfile)
        {
            return new BankAccountDAO(userProfile);
        }
        public static IHandlePayoutHistoryDAO CreateHandlePayoutHistoryDAO(UserProfile userProfile)
        {
            return new HandlePayoutHistoryDAO(userProfile);
        }
        public static IBusinessIdcardDAO CreateBusinessIdcardDAO(UserProfile userProfile)
        {
            return new BusinessIdcardDAO(userProfile);
        }
        public static IZxbConfigDAO CreateZxbConfigDAO(UserProfile userProfile)
        {
            return new ZxbConfigDAO(userProfile);
        }
        public static IZXTMessageDAO CreateZXTMessageDAO(UserProfile userProfile)
        {
            return new ZXTMessageDAO(userProfile);
        }
        public static IZXTFriendDAO CreateZXTFriendDAO(UserProfile userProfile)
        {
            return new ZXTFriendDAO(userProfile);
        }
        public static IZXTMessageViewDAO CreateZXTMessageViewDAO(UserProfile userProfile)
        {
            return new ZXTMessageViewDAO(userProfile);
        }
        public static IZXTFriendViewDAO CreateZXTFriendViewDAO(UserProfile userProfile)
        {
            return new ZXTFriendViewDAO(userProfile);
        }
        public static ISignInDAO CreateSignInDAO(UserProfile userProfile)
        {
            return new SignInDAO(userProfile);
        }
        public static ICardDataDAO CreateCardDataDAO(UserProfile userProfile)
        {
            return new CardDataDAO(userProfile);
        }
        public static ICardCollectionDAO CreateCardCollectionDAO(UserProfile userProfile)
        {
            return new CardCollectionDAO(userProfile);
        }
        public static ICardCollectionViewDAO CreateCardCollectionViewDAO(UserProfile userProfile)
        {
            return new CardCollectionViewDAO(userProfile);
        }
        public static ICardSendDAO CreateCardSendDAO(UserProfile userProfile)
        {
            return new CardSendDAO(userProfile);
        }
        public static ICardSendViewDAO CreateCardSendViewDAO(UserProfile userProfile)
        {
            return new CardSendViewDAO(userProfile);
        }
        public static ICardGroupDAO CreateCardGroupDAO(UserProfile userProfile)
        {
            return new CardGroupDAO(userProfile);
        }
        public static ICardGroupCardDAO CardGroupCardDAO(UserProfile userProfile)
        {
            return new CardGroupCardDAO(userProfile);
        }
        public static ICardGroupViewDAO CardGroupViewDAO(UserProfile userProfile)
        {
            return new CardGroupViewDAO(userProfile);
        }
        public static ICardGroupCardViewViewDAO CardGroupCardViewViewDAO(UserProfile userProfile)
        {
            return new CardGroupCardViewViewDAO(userProfile);
        }
        public static ICardGroupCardViewDAO CardGroupCardViewDAO(UserProfile userProfile)
        {
            return new CardGroupCardViewDAO(userProfile);
        }
        public static ICardPartyDAO CardPartyDAO(UserProfile userProfile)
        {
            return new CardPartyDAO(userProfile);
        }
        public static ICardPartyCostDAO CardPartyCostDAO(UserProfile userProfile)
        {
            return new CardPartyCostDAO(userProfile);
        }
        public static ICardPartyContactsDAO CardPartyContactsDAO(UserProfile userProfile)
        {
            return new CardPartyContactsDAO(userProfile);
        }
        public static ICardPartyContactsViewDAO CardPartyContactsViewDAO(UserProfile userProfile)
        {
            return new CardPartyContactsViewDAO(userProfile);
        }
        public static ICardPartySignUpDAO CardPartySignUpDAO(UserProfile userProfile)
        {
            return new CardPartySignUpDAO(userProfile);
        }
        public static ICardPartySignUpViewDAO CardPartySignUpViewDAO(UserProfile userProfile)
        {
            return new CardPartySignUpViewDAO(userProfile);
        }
        public static ICardPartyViewDAO CardPartyViewDAO(UserProfile userProfile)
        {
            return new CardPartyViewDAO(userProfile);
        }
        public static ICardPartyViewViewDAO CardPartyViewViewDAO(UserProfile userProfile)
        {
            return new CardPartyViewViewDAO(userProfile);
        }
        public static ICardPartySignUpFormDAO CardPartySignUpFormDAO(UserProfile userProfile)
        {
            return new CardPartySignUpFormDAO(userProfile);
        }
        public static ICardFormListDAO CardFormListDAO(UserProfile userProfile)
        {
            return new CardFormListDAO(userProfile);
        }
        public static ICardFormListViewDAO CardFormListViewDAO(UserProfile userProfile)
        {
            return new CardFormListViewDAO(userProfile);
        }
        public static ICardFormListTempDAO CardFormListTempDAO(UserProfile userProfile)
        {
            return new CardFormListTempDAO(userProfile);
        }
        public static ICardNoticeDAO CardNoticeDAO(UserProfile userProfile)
        {
            return new CardNoticeDAO(userProfile);
        }
        public static ICardPartyOrderDAO CardPartyOrderDAO(UserProfile userProfile)
        {
            return new CardPartyOrderDAO(userProfile);
        }
        public static ICardPartyOrderViewDAO CardPartyOrderViewDAO(UserProfile userProfile)
        {
            return new CardPartyOrderViewDAO(userProfile);
        }
        public static ICardBalanceDAO CardBalanceDAO(UserProfile userProfile)
        {
            return new CardBalanceDAO(userProfile);
        }

        public static ICardPayOutDAO CardPayOutDAO(UserProfile userProfile)
        {
            return new CardPayOutDAO(userProfile);
        }

        public static ICardNewsDAO CardNewsDAO(UserProfile userProfile)
        {
            return new CardNewsDAO(userProfile);
        }

        public static ICardRedPacketDAO CardRedPacketDAO(UserProfile userProfile)
        {
            return new CardRedPacketDAO(userProfile);
        }
        public static ICardRedPacketDetailDAO CardRedPacketDetailDAO(UserProfile userProfile)
        {
            return new CardRedPacketDetailDAO(userProfile);
        }
        public static ICardRedPacketListDAO CardRedPacketListDAO(UserProfile userProfile)
        {
            return new CardRedPacketListDAO(userProfile);
        }
        public static ICardRedPacketViewDAO CardRedPacketViewDAO(UserProfile userProfile)
        {
            return new CardRedPacketViewDAO(userProfile);
        }
        public static ICardRedPacketListViewDAO CardRedPacketListViewDAO(UserProfile userProfile)
        {
            return new CardRedPacketListViewDAO(userProfile);
        }
        public static ICardAchievemenViewDAO CardAchievemenViewDAO(UserProfile userProfile)
        {
            return new CardAchievemenViewDAO(userProfile);
        }
        public static ICustomerCardViewDAO CustomerCardViewDAO(UserProfile userProfile)
        {
            return new CustomerCardViewDAO(userProfile);
        }
        public static ILogDAO LogDAO(UserProfile userProfile)
        {
            return new LogDAO(userProfile);
        }
        public static ICardOrderDAO CardOrderDAO(UserProfile userProfile)
        {
            return new CardOrderDAO(userProfile);
        }
        public static ICardOrderViewDAO CardOrderViewDAO(UserProfile userProfile)
        {
            return new CardOrderViewDAO(userProfile);
        }
        public static ICardAccessRecordsDAO CardAccessRecordsDAO(UserProfile userProfile)
        {
            return new CardAccessRecordsDAO(userProfile);
        }
        public static IViolationDAO ViolationDAO(UserProfile userProfile)
        {
            return new ViolationDAO(userProfile);
        }
        public static ICardMessageDAO CardMessageDAO(UserProfile userProfile)
        {
            return new CardMessageDAO(userProfile);
        }
        public static ICustomerRecentLoginViewDAO CustomerRecentLoginViewDAO(UserProfile userProfile)
        {
            return new CustomerRecentLoginViewDAO(userProfile);
        }
        public static ICardSubscriptionDAO CardSubscriptionDAO(UserProfile userProfile)
        {
            return new CardSubscriptionDAO(userProfile);
        }

        public static ICardQuestionnaireDAO CardQuestionnaireDAO(UserProfile userProfile)
        {
            return new CardQuestionnaireDAO(userProfile);
        }
        public static ICardQuestionnaireSignupDAO CardQuestionnaireSignupDAO(UserProfile userProfile)
        {
            return new CardQuestionnaireSignupDAO(userProfile);
        }
        public static ICardTransferDAO CardTransferDAO(UserProfile userProfile)
        {
            return new CardTransferDAO(userProfile);
        }
        public static ICardDemandViewDAO CardDemandViewDAO(UserProfile userProfile)
        {
            return new CardDemandViewDAO(userProfile);
        }
        public static ICardDemandOfferViewDAO CardDemandOfferViewDAO(UserProfile userProfile)
        {
            return new CardDemandOfferViewDAO(userProfile);
        }
        public static ICardSoftArticleDAO CardSoftArticleDAO(UserProfile userProfile)
        {
            return new CardSoftArticleDAO(userProfile);
        }
        public static ICardSoftArticleOrderDAO CardSoftArticleOrderDAO(UserProfile userProfile)
        {
            return new CardSoftArticleOrderDAO(userProfile);
        }
        public static ICardSoftArticleOrderViewDAO CardSoftArticleOrderViewDAO(UserProfile userProfile)
        {
            return new CardSoftArticleOrderViewDAO(userProfile);
        }
        public static ICardSoftArticleComplaintDAO CardSoftArticleComplaintDAO(UserProfile userProfile)
        {
            return new CardSoftArticleComplaintDAO(userProfile);
        }
        public static ICardHelpDAO CardHelpDAO(UserProfile userProfile)
        {
            return new CardHelpDAO(userProfile);
        }
        public static ICardDataViewDAO CardDataViewDAO(UserProfile userProfile)
        {
            return new CardDataViewDAO(userProfile);
        }
        public static ICardQuestionnaireViewDAO CardQuestionnaireViewDAO(UserProfile userProfile)
        {
            return new CardQuestionnaireViewDAO(userProfile);
        }
        public static ICompanyDAO CompanyDAO(UserProfile userProfile)
        {
            return new CompanyDAO(userProfile);
        }
        public static ICompanyDAO CompanyUPDAO(UserProfile userProfile)
        {
            return new CompanyUPDAO(userProfile);
        }
        public static ICompanyLocationViewDAO CompanyLocationViewDAO(UserProfile userProfile)
        {
            return new CompanyLocationViewDAO(userProfile);
        }
        public static ICardRankingViewDAO CardRankingViewDAO(UserProfile userProfile)
        {
            return new CardRankingViewDAO(userProfile);
        }
        public static ICardDataRecommendedViewDAO CardDataRecommendedViewDAO(UserProfile userProfile)
        {
            return new CardDataRecommendedViewDAO(userProfile);
        }
        public static ICardKeywordDAO CardKeywordDAO(UserProfile userProfile)
        {
            return new CardKeywordDAO(userProfile);
        }
        public static ICardKeywordViewDAO CardKeywordViewDAO(UserProfile userProfile)
        {
            return new CardKeywordViewDAO(userProfile);
        }
        public static ICardDiscountCodeDAO CardDiscountCodeDAO(UserProfile userProfile)
        {
            return new CardDiscountCodeDAO(userProfile);
        }
        public static ICardAgentDAO CardAgentDAO(UserProfile userProfile)
        {
            return new CardAgentDAO(userProfile);
        }
        public static ICardAgentDepositDAO CardAgentDepositDAO(UserProfile userProfile)
        {
            return new CardAgentDepositDAO(userProfile);
        }
        public static ICardAgentViewDAO CardAgentViewDAO(UserProfile userProfile)
        {
            return new CardAgentViewDAO(userProfile);
        }
        public static ICardAgentApplyDAO CardAgentApplyDAO(UserProfile userProfile)
        {
            return new CardAgentApplyDAO(userProfile);
        }
        public static ICardAgentFinanceDAO CardAgentFinanceDAO(UserProfile userProfile)
        {
            return new CardAgentFinanceDAO(userProfile);
        }
        public static ICardOrderMonthDAO CardOrderMonthDAO(UserProfile userProfile)
        {
            return new CardOrderMonthDAO(userProfile);
        }
        public static ICardVipApplyDAO CardVipApplyDAO(UserProfile userProfile)
        {
            return new CardVipApplyDAO(userProfile);
        }
        public static ICardByPhoneDAO CardByPhoneDAO(UserProfile userProfile)
        {
            return new CardByPhoneDAO(userProfile);
        }
        public static ICardExchangeCodeDAO CardExchangeCodeDAO(UserProfile userProfile)
        {
            return new CardExchangeCodeDAO(userProfile);
        }
        public static ICardQuestionnaireAdminDAO CardQuestionnaireAdminDAO(UserProfile userProfile)
        {
            return new CardQuestionnaireAdminDAO(userProfile);
        }
        public static ICardPoterDAO CardPoterDAO(UserProfile userProfile)
        {
            return new CardPoterDAO(userProfile);
        }

        public static ICardRebateViewDAO CardRebateViewDAO(UserProfile userProfile)
        {
            return new CardRebateViewDAO(userProfile);
        }
        public static IMediaDAO MediaDAO(UserProfile userProfile)
        {
            return new MediaDAO(userProfile);
        }
        public static IIndexDataDAO IndexDataDAO(UserProfile userProfile)
        {
            return new IndexDataDAO(userProfile);
        }
        public static ITicketDAO TicketDAO(UserProfile userProfile)
        {
            return new TicketDAO(userProfile);
        }
        public static IAuthorizationDAO AuthorizationDAO(UserProfile userProfile)
        {
            return new AuthorizationDAO(userProfile);
        }
        public static ICardLaunchDAO CardLaunchDAO(UserProfile userProfile)
        {
            return new CardLaunchDAO(userProfile);
        }
        public static ICertificatesDAO CertificatesDAO(UserProfile userProfile)
        {
            return new CertificatesDAO(userProfile);
        }
        public static ICardHotPartyDAO CardHotPartyDAO(UserProfile userProfile)
        {
            return new CardHotPartyDAO(userProfile);
        }
        public static ICardIndexPartyDAO CardIndexPartyDAO(UserProfile userProfile)
        {
            return new CardIndexPartyDAO(userProfile);
        }
        public static IwxMerchantDAO wxMerchantDAO(UserProfile userProfile)
        {
            return new wxMerchantDAO(userProfile);
        }
        public static ICardAccessRecordsViewDAO CardAccessRecordsViewDAO(UserProfile userProfile)
        {
            return new CardAccessRecordsViewDAO(userProfile);
        }
        public static IFarmGameDAO FarmGameDAO(UserProfile userProfile)
        {
            return new FarmGameDAO(userProfile);
        }
        public static IFarmGameTaskDAO FarmGameTaskDAO(UserProfile userProfile)
        {
            return new FarmGameTaskDAO(userProfile);
        }
        public static IFarmGameViewDAO FarmGameViewDAO(UserProfile userProfile)
        {
            return new FarmGameViewDAO(userProfile);
        }
        public static IFarmgamePrizeDAO FarmgamePrizeDAO(UserProfile userProfile)
        {
            return new FarmgamePrizeDAO(userProfile);
        }
        public static IFarmgamePrizeOrderDAO FarmgamePrizeOrderDAO(UserProfile userProfile)
        {
            return new FarmgamePrizeOrderDAO(userProfile);
        }
        public static IFarmgamePrizeOrderViewDAO FarmgamePrizeOrderViewDAO(UserProfile userProfile)
        {
            return new FarmgamePrizeOrderViewDAO(userProfile);
        }
        public static IwxMiniprogramsDAO wxMiniprogramsDAO(UserProfile userProfile)
        {
            return new wxMiniprogramsDAO(userProfile);
        }
        public static IOriginCustomerIdViewDAO OriginCustomerIdViewDAO(UserProfile userProfile)
        {
            return new OriginCustomerIdViewDAO(userProfile);
        }
        public static IBaiduAIConfigDAO BaiduAIConfigDAO(UserProfile userProfile)
        {
            return new BaiduAIConfigDAO(userProfile);
        }
    }
}

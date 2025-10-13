using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFramework.VO;
using SPLibrary.CustomerManagement.DAO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class BusinessCardManagementDAOFactory
    {
        public static IPersonalDAO PersonalDAO(UserProfile userProfile)
        {
            return new PersonalDAO(userProfile);
        }
        public static IBusinessCardDAO BusinessCardDAO(UserProfile userProfile)
        {
            return new BusinessCardDAO(userProfile);
        }
        public static IInfoDAO InfoDAO(UserProfile userProfile)
        {
            return new InfoDAO(userProfile);
        }
        public static IInfoSortDAO InfoSortDAO(UserProfile userProfile)
        {
            return new InfoSortDAO(userProfile);
        }
        public static IInfoViewDAO InfoViewDAO(UserProfile userProfile)
        {
            return new InfoViewDAO(userProfile);
        }
        public static IDepartmentDAO DepartmentDAO(UserProfile userProfile)
        {
            return new DepartmentDAO(userProfile);
        }
        public static IPersonalViewDAO PersonalViewDAO(UserProfile userProfile)
        {
            return new PersonalViewDAO(userProfile);
        }
        public static IJurisdictionDAO JurisdictionDAO(UserProfile userProfile)
        {
            return new JurisdictionDAO(userProfile);
        }
        public static ICrmViewDAO CrmViewDAO(UserProfile userProfile)
        {
            return new CrmViewDAO(userProfile);
        }
        public static ICrmDAO CrmDAO(UserProfile userProfile)
        {
            return new CrmDAO(userProfile);
        }
        public static IAccessrecordsDAO AccessrecordsDAO(UserProfile userProfile)
        {
            return new AccessrecordsDAO(userProfile);
        }
        public static IAccessrecordsViewDAO AccessrecordsViewDAO(UserProfile userProfile)
        {
            return new AccessrecordsViewDAO(userProfile);
        }
        public static IAccessrecordsViewByRespondentsDAO AccessrecordsViewByRespondentsDAO(UserProfile userProfile)
        {
            return new AccessrecordsViewByRespondentsDAO(userProfile);
        }
        public static IAccessrecordsViewDAO AccessrecordsViewGroupDAO(UserProfile userProfile)
        {
            return new AccessrecordsViewGroupDAO(userProfile);
        }
        public static IBusinessCardViewDAO BusinessCardViewDAO(UserProfile userProfile)
        {
            return new BusinessCardViewDAO(userProfile);
        }

        public static IAgentDAO AgentDAO(UserProfile userProfile)
        {
            return new AgentDAO(userProfile);
        }
        public static IAgentLevelDAO AgentLevelDAO(UserProfile userProfile)
        {
            return new AgentLevelDAO(userProfile);
        }
        public static IAgentViewDAO AgentViewDAO(UserProfile userProfile)
        {
            return new AgentViewDAO(userProfile);
        }
        public static IAgentlevelCostDAO AgentlevelCostDAO(UserProfile userProfile)
        {
            return new AgentlevelCostDAO(userProfile);
        }
        public static IOrderDAO OrderDAO(UserProfile userProfile)
        {
            return new OrderDAO(userProfile);
        }
        public static IOrderViewDAO OrderViewDAO(UserProfile userProfile)
        {
            return new OrderViewDAO(userProfile);
        }
        public static ISecondBusinessDAO SecondBusinessDAO(UserProfile userProfile)
        {
            return new SecondBusinessDAO(userProfile);
        }
        public static ISecondBusinessViewDAO SecondBusinessViewDAO(UserProfile userProfile)
        {
            return new SecondBusinessViewDAO(userProfile);
        }
        public static ICommentViewDAO CommentViewDAO(UserProfile userProfile)
        {
            return new CommentViewDAO(userProfile);
        }
        public static IGreetingCardDAO GreetingCardDAO(UserProfile userProfile)
        {
            return new GreetingCardDAO(userProfile);
        }
        public static ISubscriptionDAO SubscriptionDAO(UserProfile userProfile)
        {
            return new SubscriptionDAO(userProfile);
        }
        public static IShareDAO ShareDAO(UserProfile userProfile)
        {
            return new ShareDAO(userProfile);
        }
        public static ITargetDAO TargetDAO(UserProfile userProfile)
        {
            return new TargetDAO(userProfile);
        }
        public static IPunchDAO PunchDAO(UserProfile userProfile)
        {
            return new PunchDAO(userProfile);
        }
        public static IGroupBuyDAO GroupBuyDAO(UserProfile userProfile)
        {
            return new GroupBuyDAO(userProfile);
        }
        public static IGroupBuyMemberDAO GroupBuyMemberDAO(UserProfile userProfile)
        {
            return new GroupBuyMemberDAO(userProfile);
        }
        public static IOrderGroupBuyViewDAO OrderGroupBuyViewDAO(UserProfile userProfile)
        {
            return new OrderGroupBuyViewDAO(userProfile);
        }
        public static IGroupBuyMemberViewDAO GroupBuyMemberViewDAO(UserProfile userProfile)
        {
            return new GroupBuyMemberViewDAO(userProfile);
        }
        public static IBcPayOutHistoryDAO BcPayOutHistoryDAO(UserProfile userProfile)
        {
            return new BcPayOutHistoryDAO(userProfile);
        }
        public static IInfoCostDAO InfoCostDAO(UserProfile userProfile)
        {
            return new InfoCostDAO(userProfile);
        }
        public static IBankAccountDAO BankAccountDAO(UserProfile userProfile)
        {
            return new BankAccountDAO(userProfile);
        }
        public static IBusinessBalanceDAO BalanceDAO(UserProfile userProfile)
        {
            return new BusinessBalanceDAO(userProfile);
        }
        public static IBalanceHistoryDAO BalanceHistoryDAO(UserProfile userProfile)
        {
            return new BalanceHistoryDAO(userProfile);
        }
        public static IThemeDAO ThemeDAO(UserProfile userProfile)
        {
            return new ThemeDAO(userProfile);
        }
        public static IIntegralDAO IntegralDAO(UserProfile userProfile)
        {
            return new IntegralDAO(userProfile);
        }
        public static IIntegralViewDAO IntegralViewDAO(UserProfile userProfile)
        {
            return new IntegralViewDAO(userProfile);
        }
        public static IHelpDAO HelpDAO(UserProfile userProfile)
        {
            return new HelpDAO(userProfile);
        }
        public static ICallCenterDAO CallCenterDAO(UserProfile userProfile)
        {
            return new CallCenterDAO(userProfile);
        }
        public static ICallNumberDAO CallNumberDAO(UserProfile userProfile)
        {
            return new CallNumberDAO(userProfile);
        }
        public static ICallRecordDAO CallRecordDAO(UserProfile userProfile)
        {
            return new CallRecordDAO(userProfile);
        }
        public static IAgentIntegralDAO AgentIntegralDAO(UserProfile userProfile)
        {
            return new AgentIntegralDAO(userProfile);
        }
        public static IOrderByGroupbuyIdViewDAO OrderByGroupbuyIdViewDAO(UserProfile userProfile)
        {
            return new OrderByGroupbuyIdViewDAO(userProfile);
        }
        public static IOrderGroupBuyNotGroupViewDAO OrderGroupBuyNotGroupViewDAO(UserProfile userProfile)
        {
            return new OrderGroupBuyNotGroupViewDAO(userProfile);
        }
        public static IShortUrlDAO ShortUrlDAO(UserProfile userProfile)
        {
            return new ShortUrlDAO(userProfile);
        }
        public static IShopVipDAO ShopVipDAO(UserProfile userProfile)
        {
            return new ShopVipDAO(userProfile);
        }
        public static IShopVipPersonalDAO ShopVipPersonalDAO(UserProfile userProfile)
        {
            return new ShopVipPersonalDAO(userProfile);
        }
        public static IActivityDAO ActivityDAO(UserProfile userProfile)
        {
            return new ActivityDAO(userProfile);
        }
        public static IActivityCountDAO ActivityCountDAO(UserProfile userProfile)
        {
            return new ActivityCountDAO(userProfile);
        }
        public static IActivityTicketDAO ActivityTicketDAO(UserProfile userProfile)
        {
            return new ActivityTicketDAO(userProfile);
        }
        public static IActivitySignTicketDAO ActivitySignTicketDAO(UserProfile userProfile)
        {
            return new ActivitySignTicketDAO(userProfile);
        }
        public static IProfitsharingDAO ProfitsharingDAO(UserProfile userProfile)
        {
            return new ProfitsharingDAO(userProfile);
        }
        public static IAdDAO AdDAO(UserProfile userProfile)
        {
            return new AdDAO(userProfile);
        }
        public static IBCPartyDAO BCPartyDAO(UserProfile userProfile)
        {
            return new BCPartyDAO(userProfile);
        }
        public static IBCPartyCostDAO BCPartyCostDAO(UserProfile userProfile)
        {
            return new BCPartyCostDAO(userProfile);
        }
        public static IBCPartyContactsDAO BCPartyContactsDAO(UserProfile userProfile)
        {
            return new BCPartyContactsDAO(userProfile);
        }
     
        public static IBCPartySignUpDAO BCPartySignUpDAO(UserProfile userProfile)
        {
            return new BCPartySignUpDAO(userProfile);
        }
        public static IBCPartySignUpFormDAO BCPartySignUpFormDAO(UserProfile userProfile)
        {
            return new BCPartySignUpFormDAO(userProfile);
        }
        public static IBCPartySignUpViewDAO BCPartySignUpViewDAO(UserProfile userProfile)
        {
            return new BCPartySignUpViewDAO(userProfile);
        }
        public static IBCPartyContactsViewDAO BCPartyContactsViewDAO(UserProfile userProfile)
        {
            return new BCPartyContactsViewDAO(userProfile);
        }
        public static IBCPartyOrderViewDAO BCPartyOrderViewDAO(UserProfile userProfile)
        {
            return new BCPartyOrderViewDAO(userProfile);
        }
        public static IBCPartyOrderDAO BCPartyOrderDAO(UserProfile userProfile)
        {
            return new BCPartyOrderDAO(userProfile);
        }

        public static IVisitDAO VisitDAO(UserProfile userProfile)
        {
            return new VisitDAO(userProfile);
        }

        public static IVisitViewDAO VisitViewDAO(UserProfile userProfile)
        {
            return new VisitViewDAO(userProfile);
        }
        public static IQuestionnaireDataDAO QuestionnaireDataDAO(UserProfile userProfile)
        {
            return new QuestionnaireDataDAO(userProfile);
        }

        public static IAnswerSheetDAO AnswerSheetDAO(UserProfile userProfile)
        {
            return new AnswerSheetDAO(userProfile);
        }
        public static IAnswerSheetViewDAO AnswerSheetViewDAO(UserProfile userProfile)
        {
            return new AnswerSheetViewDAO(userProfile);
        }
        public static ICJLotteriesDAO CJLotteriesDAO(UserProfile userProfile)
        {
            return new CJLotteriesDAO(userProfile);
        }
        public static ICJWinningRecordsDAO CJWinningRecordsDAO(UserProfile userProfile)
        {
            return new CJWinningRecordsDAO(userProfile);
        }

        public static ICardRegistertableDAO CardRegistertableDAO(UserProfile userProfile)
        {
            return new CardRegistertableDAO(userProfile);
        }

        public static ICardRegistertableSignupDAO CardRegistertableSignupDAO(UserProfile userProfile)
        {
            return new CardRegistertableSignupDAO(userProfile);
        }

        public static ICardRegistertableViewDAO CardRegistertableViewDAO(UserProfile userProfile)
        {
            return new CardRegistertableViewDAO(userProfile);
        }

        public static ICardRegistertableAdminDAO CardRegistertableAdminDAO(UserProfile userProfile)
        {
            return new CardRegistertableAdminDAO(userProfile);
        }

        internal static ICardNoticedDAO CardNoticedDAO(UserProfile userProfile)
        {
            return new CardNoticedDAO(userProfile);
        }
    }
}

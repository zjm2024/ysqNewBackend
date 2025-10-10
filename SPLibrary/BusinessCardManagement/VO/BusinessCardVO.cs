using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BusinessCardVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BusinessCardVO));
       
		[DataMember]
		 public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID") ; } set {  SetValue("BusinessID", value); } }
        [DataMember]
        public String BusinessName { get { return (String)GetValue(typeof(String), "BusinessName"); } set { SetValue("BusinessName", value); } }
        [DataMember]
        public String Industry { get { return (String)GetValue(typeof(String), "Industry"); } set { SetValue("Industry", value); } }
        [DataMember]
        public String LogoImg { get { return (String)GetValue(typeof(String), "LogoImg"); } set { SetValue("LogoImg", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Number { get { return (Int32)GetValue(typeof(Int32), "Number"); } set { SetValue("Number", value); } }
        [DataMember]
        public DateTime ExpirationAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpirationAt"); } set { SetValue("ExpirationAt", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "latitude"); } set { SetValue("latitude", value); } }
        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "longitude"); } set { SetValue("longitude", value); } }
        [DataMember]
        public String Tel { get { return (String)GetValue(typeof(String), "Tel"); } set { SetValue("Tel", value); } }
        [DataMember]
        public Int32 isAddress { get { return (Int32)GetValue(typeof(Int32), "isAddress"); } set { SetValue("isAddress", value); } }
        [DataMember]
        public Int32 isTel { get { return (Int32)GetValue(typeof(Int32), "isTel"); } set { SetValue("isTel", value); } }
        [DataMember]
        public String JoinQR { get { return (String)GetValue(typeof(String), "JoinQR"); } set { SetValue("JoinQR", value); } }
        [DataMember]
        public String BusinessLicenseImg { get { return (String)GetValue(typeof(String), "BusinessLicenseImg"); } set { SetValue("BusinessLicenseImg", value); } }
        [DataMember]
        public String OfficialProducts { get { return (String)GetValue(typeof(String), "OfficialProducts"); } set { SetValue("OfficialProducts", value); } }
        [DataMember]
        public Int32 HeadquartersID { get { return (Int32)GetValue(typeof(Int32), "HeadquartersID"); } set { SetValue("HeadquartersID", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 ThemeID { get { return (Int32)GetValue(typeof(Int32), "ThemeID"); } set { SetValue("ThemeID", value); } }
        [DataMember]
        public String CustomColumn { get { return (String)GetValue(typeof(String), "CustomColumn"); } set { SetValue("CustomColumn", value); } }
        [DataMember]
        public String ProductColumn { get { return (String)GetValue(typeof(String), "ProductColumn"); } set { SetValue("ProductColumn", value); } }

        [DataMember]
        public Int32 AccessSetUpSignIn { get { return (Int32)GetValue(typeof(Int32), "AccessSetUpSignIn"); } set { SetValue("AccessSetUpSignIn", value); } }
        [DataMember]
        public Int32 AccessSetUpPhone { get { return (Int32)GetValue(typeof(Int32), "AccessSetUpPhone"); } set { SetValue("AccessSetUpPhone", value); } }
        [DataMember]
        public Int32 AccessSetUpExchange { get { return (Int32)GetValue(typeof(Int32), "AccessSetUpExchange"); } set { SetValue("AccessSetUpExchange", value); } }
        [DataMember]
        public Int32 SubsidiarySum { get { return (Int32)GetValue(typeof(Int32), "SubsidiarySum"); } set { SetValue("SubsidiarySum", value); } }

        [DataMember]
        public Int32 isPay { get { return (Int32)GetValue(typeof(Int32), "isPay"); } set { SetValue("isPay", value); } }
        [DataMember]
        public Int32 isAgent { get { return (Int32)GetValue(typeof(Int32), "isAgent"); } set { SetValue("isAgent", value); } }
        [DataMember]
        public Int32 isGroup { get { return (Int32)GetValue(typeof(Int32), "isGroup"); } set { SetValue("isGroup", value); } }
        [DataMember]
        public Int32 DisplayCard { get { return (Int32)GetValue(typeof(Int32), "DisplayCard"); } set { SetValue("DisplayCard", value); } }
        [DataMember]
        public Int32 DisplayVideo { get { return (Int32)GetValue(typeof(Int32), "DisplayVideo"); } set { SetValue("DisplayVideo", value); } }



        #region ICloneable Member
        public override object Clone()
        {
            BusinessCardVO tmp = new BusinessCardVO();
            tmp.changeData = new Dictionary<string, object>(this.changeData);
            tmp.originData = new Dictionary<string, object>(this.originData);
            return tmp;
        }
        #endregion
         
    	#region ICommonVO Member
        List<string> ICommonVO.PropertyList
        {
            get { return _propertyList; }
        }
        #endregion
    }

    /// <summary>
    /// 企业版权限区分
    /// </summary>
    public partial class BusinessCard_JurisdictionVO
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        public Int32 BusinessID { get; set; }
        /// <summary>
        /// 企业会员是否到期
        /// </summary>
        public Boolean isEffective { get; set; }
        /// <summary>
        /// CRM权限
        /// </summary>
        public Boolean isCrm { get; set; }
        /// <summary>
        /// 企业会员
        /// </summary>
        public Boolean isVip { get; set; }
        /// <summary>
        /// 是否是子公司
        /// </summary>
        public Boolean isSubsidiary { get; set; }
        /// <summary>
        /// 是否有支付权限
        /// </summary>
        public Boolean isPay { get; set; }
        /// <summary>
        /// 代理商功能
        /// </summary>
        public Boolean isAgent { get; set; }
        /// <summary>
        /// 集团功能 
        /// </summary>
        public Boolean isGroup { get; set; }

        /// <summary>
        /// 贺卡功能
        /// </summary>
        public Boolean isGreetingCard { get; set; }
        /// <summary>
        /// 移动官网
        /// </summary>
        public Boolean isWeb { get; set; }
        /// <summary>
        /// 在线店铺
        /// </summary>
        public Boolean isProduct { get; set; }
        /// <summary>
        /// 文章中心
        /// </summary>
        public Boolean isNews { get; set; }
        /// <summary>
        /// 案例中心
        /// </summary>
        public Boolean isCase { get; set; }
        /// <summary>
        /// 电子彩页
        /// </summary>
        public Boolean isColorPage { get; set; }
        /// <summary>
        /// 短视频
        /// </summary>
        public Boolean isVideo { get; set; }
        /// <summary>
        /// 话术库
        /// </summary>
        public Boolean isInfo { get; set; }
        /// <summary>
        /// 资源汇
        /// </summary>
        public Boolean isCrmStorehouse { get; set; }
        /// <summary>
        /// 商机发布
        /// </summary>
        public Boolean isShangji { get; set; }
        /// <summary>
        /// 考勤系统
        /// </summary>
        public Boolean isPunch { get; set; }
        /// <summary>
        /// 内外部团队管理
        /// </summary>
        public Boolean isTeam { get; set; }
        /// <summary>
        /// 搜客
        /// </summary>
        public Boolean isSearch { get; set; }
        /// <summary>
        /// 情报厅
        /// </summary>
        public Boolean isStatistics { get; set; }
        /// <summary>
        /// 会务活动
        /// </summary>
        public Boolean isParty { get; set; }

        /// <summary>
        /// 营销数据
        /// </summary>
        public Boolean isMarketingData { get; set; }

        /// <summary>
        /// 拼团功能
        /// </summary>
        public Boolean isGroupBuy { get; set; }
        /// <summary>
        /// 秒杀功能
        /// </summary>
        public Boolean isSeckill { get; set; }
    }
}
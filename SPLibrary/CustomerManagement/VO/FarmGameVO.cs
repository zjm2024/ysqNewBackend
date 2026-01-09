using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class FarmGameVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(FarmGameVO));
       
		[DataMember]
		 public Int32 FarmGameID { get { return (Int32)GetValue(typeof(Int32), "FarmGameID") ; } set {  SetValue("FarmGameID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 Gold { get { return (Int32)GetValue(typeof(Int32), "Gold"); } set { SetValue("Gold", value); } }

        [DataMember]
		 public String FieldsType1 { get { return (String)GetValue(typeof(String), "FieldsType1") ; } set {  SetValue("FieldsType1", value); } }
        [DataMember]
        public Int32 FieldsSum1 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum1"); } set { SetValue("FieldsSum1", value); } }

        [DataMember]
        public String FieldsType2 { get { return (String)GetValue(typeof(String), "FieldsType2"); } set { SetValue("FieldsType2", value); } }
        [DataMember]
        public Int32 FieldsSum2 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum2"); } set { SetValue("FieldsSum2", value); } }

        [DataMember]
        public String FieldsType3 { get { return (String)GetValue(typeof(String), "FieldsType3"); } set { SetValue("FieldsType3", value); } }
        [DataMember]
        public Int32 FieldsSum3 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum3"); } set { SetValue("FieldsSum3", value); } }

        [DataMember]
        public String FieldsType4 { get { return (String)GetValue(typeof(String), "FieldsType4"); } set { SetValue("FieldsType4", value); } }
        [DataMember]
        public Int32 FieldsSum4 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum4"); } set { SetValue("FieldsSum4", value); } }

        [DataMember]
        public String FieldsType5 { get { return (String)GetValue(typeof(String), "FieldsType5"); } set { SetValue("FieldsType5", value); } }
        [DataMember]
        public Int32 FieldsSum5 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum5"); } set { SetValue("FieldsSum5", value); } }

        [DataMember]
        public String FieldsType6 { get { return (String)GetValue(typeof(String), "FieldsType6"); } set { SetValue("FieldsType6", value); } }
        [DataMember]
        public Int32 FieldsSum6 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum6"); } set { SetValue("FieldsSum6", value); } }

        [DataMember]
        public String FieldsType7 { get { return (String)GetValue(typeof(String), "FieldsType7"); } set { SetValue("FieldsType7", value); } }
        [DataMember]
        public Int32 FieldsSum7 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum7"); } set { SetValue("FieldsSum7", value); } }

        [DataMember]
        public String FieldsType8 { get { return (String)GetValue(typeof(String), "FieldsType8"); } set { SetValue("FieldsType8", value); } }
        [DataMember]
        public Int32 FieldsSum8 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum8"); } set { SetValue("FieldsSum8", value); } }

        [DataMember]
        public Int32 Apple_seed { get { return (Int32)GetValue(typeof(Int32), "Apple_seed"); } set { SetValue("Apple_seed", value); } }
        [DataMember]
        public Int32 Carrot_seed { get { return (Int32)GetValue(typeof(Int32), "Carrot_seed"); } set { SetValue("Carrot_seed", value); } }
        [DataMember]
        public Int32 Eggplant_seed { get { return (Int32)GetValue(typeof(Int32), "Eggplant_seed"); } set { SetValue("Eggplant_seed", value); } }
        [DataMember]
        public Int32 Tomato_seed { get { return (Int32)GetValue(typeof(Int32), "Tomato_seed"); } set { SetValue("Tomato_seed", value); } }

        [DataMember]
        public Int32 Water { get { return (Int32)GetValue(typeof(Int32), "Water"); } set { SetValue("Water", value); } }
        [DataMember]
        public Int32 Fertilizer { get { return (Int32)GetValue(typeof(Int32), "Fertilizer"); } set { SetValue("Fertilizer", value); } }
        [DataMember]
        public Int32 Diamond { get { return (Int32)GetValue(typeof(Int32), "Diamond"); } set { SetValue("Diamond", value); } }
        

        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            FarmGameVO tmp = new FarmGameVO();
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
    /// 游戏参数配置
    /// </summary>
    public static class FGConfig
    {
        /// <summary>
        /// 初始金币
        /// </summary>
        static public int Gold = 1000;

        /// <summary>
        /// 初始水滴
        /// </summary>
        static public int Water = 10;

        /// <summary>
        /// 水滴成长值
        /// </summary>
        static public int Water_Growth = 10;

        /// <summary>
        /// 初始肥料
        /// </summary>
        static public int Fertilizer = 5;
        /// <summary>
        /// 肥料成长值
        /// </summary>
        static public int Fertilizer_Growth = 30;

        /// <summary>
        /// 萝卜种子金额
        /// </summary>
        static public int Carrot_seed = 500;
        /// <summary>
        /// 萝卜收获金币
        /// </summary>
        static public int Carrot_ripening = 1600;
        /// <summary>
        /// 萝卜成熟所需成长值
        /// </summary>
        static public int Carrot_Growth = 120;

        /// <summary>
        /// 番茄种子金额
        /// </summary>
        static public int Tomato_seed = 1200;
        /// <summary>
        /// 番茄收获金币
        /// </summary>
        static public int Tomato_ripening = 3200;
        /// <summary>
        /// 番茄成熟所需成长值
        /// </summary>
        static public int Tomato_Growth = 240;

        /// <summary>
        /// 茄子种子金额
        /// </summary>
        static public int Eggplant_seed = 2500;
        /// <summary>
        /// 茄子收获金币
        /// </summary>
        static public int Eggplant_ripening = 6000;
        /// <summary>
        /// 茄子成熟所需成长值
        /// </summary>
        static public int Eggplant_Growth = 480;

        /// <summary>
        /// 苹果种子金额
        /// </summary>
        static public int Apple_seed = 5500;
        /// <summary>
        /// 苹果收获金币
        /// </summary>
        static public int Apple_ripening = 10000;
        /// <summary>
        /// 苹果成熟所需成长值
        /// </summary>
        static public int Apple_Growth = 960;

        /// <summary>
        /// 第一块土地价格
        /// </summary>
        static public int Fields1_Money = 6000;
        /// <summary>
        /// 第二块土地价格
        /// </summary>
        static public int Fields2_Money = 12000;
        /// <summary>
        /// 第三块土地价格
        /// </summary>
        static public int Fields3_Money = 24000;
        /// <summary>
        /// 第四块土地价格
        /// </summary>
        static public int Fields4_Money = 50000;
        /// <summary>
        /// 第五块土地价格
        /// </summary>
        static public int Fields5_Money = 100000;

        /// <summary>
        /// 暴击概率0-100
        /// </summary>
        static public int CriticalHit = 10;
        /// <summary>
        /// 暴击倍数
        /// </summary>
        static public int CriticalHit_Times = 3;

        /// <summary>
        /// 每小时自动增加成长值
        /// </summary>
        static public int PerHour_Growth = 10;

        /// <summary>
        /// 每日登录奖励(水滴)
        /// </summary>
        static public int SignIn_Water = 10;

        /// <summary>
        /// 每日登录奖励（金币）
        /// </summary>
        static public int SignIn_Gold = 500;
        
        /// <summary>
        /// 参与一次抽奖
        /// </summary>
        static public int SignLuckDraw_Water = 10;

        /// <summary>
        /// 参与三次抽奖
        /// </summary>
        static public int SignThreeLuckDraw_Water = 20;

        /// <summary>
        /// 分享一次抽奖
        /// </summary>
        static public int ShareLuckDraw_Water = 10;

        /// <summary>
        /// 邀请一次助力
        /// </summary>
        static public int HelpLuckDraw_Fertilizer = 5;

        /// <summary>
        /// 观看广告
        /// </summary>
        static public int WatchAd_Fertilizer = 10;

        /// <summary>
        /// 钻石价格
        /// </summary>
        static public int Diamond_Money = 10000;

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static FGConfigVO getVO()
        {
            FGConfigVO fVO = new FGConfigVO();

            fVO.Gold = Gold;
            fVO.Water = Water;
            fVO.Water_Growth = Water_Growth;
            fVO.Fertilizer = Fertilizer;
            fVO.Fertilizer_Growth = Fertilizer_Growth;
            fVO.Carrot_seed = Carrot_seed;
            fVO.Carrot_ripening = Carrot_ripening;
            fVO.Carrot_Growth = Carrot_Growth;
            fVO.Tomato_seed = Tomato_seed;
            fVO.Tomato_ripening = Tomato_ripening;
            fVO.Tomato_Growth = Tomato_Growth;
            fVO.Eggplant_seed = Eggplant_seed;
            fVO.Eggplant_ripening = Eggplant_ripening;
            fVO.Eggplant_Growth = Eggplant_Growth;
            fVO.Apple_seed = Apple_seed;
            fVO.Apple_ripening = Apple_ripening;
            fVO.Apple_Growth = Apple_Growth;
            fVO.Fields1_Money = Fields1_Money;
            fVO.Fields2_Money = Fields2_Money;
            fVO.Fields3_Money = Fields3_Money;
            fVO.Fields4_Money = Fields4_Money;
            fVO.Fields5_Money = Fields5_Money;
            fVO.PerHour_Growth = PerHour_Growth;
            fVO.Diamond_Money = Diamond_Money;
            return fVO;
        }
    }

    /// <summary>
    /// 游戏参数配置
    /// </summary>
    public class FGConfigVO
    {
        public int Gold { get;set; }
        public int Water  { get; set; }
        public int Water_Growth { get; set; }
        public int Fertilizer { get; set; }
        public int Fertilizer_Growth { get; set; }
        public int Carrot_seed { get; set; }
        public int Carrot_ripening { get; set; }
        public int Carrot_Growth { get; set; }
        public int Tomato_seed { get; set; }
        public int Tomato_ripening { get; set; }
        public int Tomato_Growth { get; set; }
        public int Eggplant_seed { get; set; }
        public int Eggplant_ripening { get; set; }
        public int Eggplant_Growth { get; set; }
        public int Apple_seed { get; set; }
        public int Apple_ripening { get; set; }
        public int Apple_Growth { get; set; }
        public int Fields1_Money { get; set; }
        public int Fields2_Money { get; set; }
        public int Fields3_Money { get; set; }
        public int Fields4_Money { get; set; }
        public int Fields5_Money { get; set; }
        public int PerHour_Growth { get; set; }
        public int Diamond_Money { get; set; }
    }
}
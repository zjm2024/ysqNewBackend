using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class RankItemVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RankItemVO));

        [DataMember]
        public Int32 rank_items_id { get { return (Int32)GetValue(typeof(Int32), "rank_items_id"); } set { SetValue("rank_items_id", value); } }
        [DataMember]
        public Int32 rank_list_id { get { return (Int32)GetValue(typeof(Int32), "rank_list_id"); } set { SetValue("rank_list_id", value); } }
        [DataMember]
        public Int32 b_company_id { get { return (Int32)GetValue(typeof(Int32), "b_company_id"); } set { SetValue("b_company_id", value); } }
        [DataMember]
        public String company_name { get { return (String)GetValue(typeof(String), "company_name"); } set { SetValue("company_name", value); } }
        [DataMember]
        public String brand_name { get { return (String)GetValue(typeof(String), "brand_name"); } set { SetValue("brand_name", value); } }
        [DataMember]
        public decimal annual_output { get { return (decimal)GetValue(typeof(decimal), "annual_output"); } set { SetValue("annual_output", value); } }
        [DataMember]
        public String custom_field1 { get { return (String)GetValue(typeof(String), "custom_field1"); } set { SetValue("custom_field1", value); } }
       [DataMember]
        public String custom_field2 { get { return (String)GetValue(typeof(String), "custom_field2"); } set { SetValue("custom_field2", value); } }
        [DataMember]
        public Int32 sort_order { get { return (Int32)GetValue(typeof(Int32), "sort_order"); } set { SetValue("sort_order", value); } }
        [DataMember]
        public Int32 is_bound { get { return (Int32)GetValue(typeof(Int32), "is_bound"); } set { SetValue("is_bound", value); } }
        [DataMember]
        public DateTime created_at { get { return (DateTime)GetValue(typeof(DateTime), "created_at"); } set { SetValue("created_at", value); } }
        
        #region ICloneable Member
        public override object Clone()
        {
            RankItemVO tmp = new RankItemVO();
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
}

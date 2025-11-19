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
    public partial class RankItemListVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RankItemListVO));

        [DataMember]
        public Int32 rank_list_id { get { return (Int32)GetValue(typeof(Int32), "rank_list_id"); } set { SetValue("rank_list_id", value); } }
        [DataMember]
        public String list_name { get { return (String)GetValue(typeof(String), "list_name"); } set { SetValue("list_name", value); } }
        [DataMember]
        public Int32 category_id { get { return (Int32)GetValue(typeof(Int32), "category_id"); } set { SetValue("category_id", value); } }
        [DataMember]
        public String category_name { get { return (String)GetValue(typeof(String), "category_name"); } set { SetValue("category_name", value); } }
        [DataMember]
        public String data_source { get { return (String)GetValue(typeof(String), "data_source"); } set { SetValue("data_source", value); } }
        [DataMember]
        public String publish_unit { get { return (String)GetValue(typeof(String), "publish_unit"); } set { SetValue("publish_unit", value); } }
        [DataMember]
        public Int32 column_count { get { return (Int32)GetValue(typeof(Int32), "column_count"); } set { SetValue("column_count", value); } }
        [DataMember]
        public String column1_name { get { return (String)GetValue(typeof(String), "column1_name"); } set { SetValue("column1_name", value); } }
        [DataMember]
        public String column2_name { get { return (String)GetValue(typeof(String), "column2_name"); } set { SetValue("column2_name", value); } }
        [DataMember]
        public String column3_name { get { return (String)GetValue(typeof(String), "column3_name"); } set { SetValue("column3_name", value); } }
        [DataMember]
        public String column4_name { get { return (String)GetValue(typeof(String), "column4_name"); } set { SetValue("column4_name", value); } }
        [DataMember]
        public String publisher { get { return (String)GetValue(typeof(String), "publisher"); } set { SetValue("publisher", value); } }
        [DataMember]
        public Int32 personal_id { get { return (Int32)GetValue(typeof(Int32), "personal_id"); } set { SetValue("personal_id", value); } }
        [DataMember]
        public DateTime publish_time { get { return (DateTime)GetValue(typeof(DateTime), "publish_time"); } set { SetValue("publish_time", value); } }
        [DataMember]
        public Int32 status { get { return (Int32)GetValue(typeof(Int32), "status"); } set { SetValue("status", value); } }
        [DataMember]
        public Int32 view_count { get { return (Int32)GetValue(typeof(Int32), "view_count"); } set { SetValue("view_count", value); } }
        [DataMember]
        public Int32 sync_to_b { get { return (Int32)GetValue(typeof(Int32), "sync_to_b"); } set { SetValue("sync_to_b", value); } }
        [DataMember]
        public DateTime created_at { get { return (DateTime)GetValue(typeof(DateTime), "created_at"); } set { SetValue("created_at", value); } }
        [DataMember]
        public DateTime updated_at { get { return (DateTime)GetValue(typeof(DateTime), "updated_at"); } set { SetValue("updated_at", value); } }
        [DataMember]
        public String rank_img { get { return (String)GetValue(typeof(String), "rank_img"); } set { SetValue("rank_img", value); } }
        [DataMember]
        public Int32 rank_items_id { get { return (Int32)GetValue(typeof(Int32), "rank_items_id"); } set { SetValue("rank_items_id", value); } }
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

        #region ICloneable Member
        public override object Clone()
        {
            RankItemListVO tmp = new RankItemListVO();
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

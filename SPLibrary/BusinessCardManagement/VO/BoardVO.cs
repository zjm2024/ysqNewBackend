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
    public partial class BoardVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BoardVO));

        [DataMember]
        public Int32 BoardID { get { return (Int32)GetValue(typeof(Int32), "BoardID"); } set { SetValue("BoardID", value); } }
        [DataMember]
        public String BoardName { get { return (String)GetValue(typeof(String), "BoardName"); } set { SetValue("BoardName", value); } }
        [DataMember]
        public String ImageUrl { get { return (String)GetValue(typeof(String), "ImageUrl"); } set { SetValue("ImageUrl", value); } }
        [DataMember]
        public Int32 SortOrder { get { return (Int32)GetValue(typeof(Int32), "SortOrder"); } set { SetValue("SortOrder", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime UpdateAt { get { return (DateTime)GetValue(typeof(DateTime), "UpdateAt"); } set { SetValue("UpdateAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            BoardVO tmp = new BoardVO();
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

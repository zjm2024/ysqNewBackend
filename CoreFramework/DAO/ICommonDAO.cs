using CoreFramework.VO;
namespace CoreFramework.DAO
{
    public interface ICommonDAO
    {
        /// <summary>
        /// To Create a filter for current query criteria for this DAO.
        /// </summary>
        ISelectFilter CreateFiler();

        /// <summary>
        /// To Insert a record to DB with VO.
        /// </summary>
        int Insert(ICommonVO vo);

        /// <summary>
        /// To Update the specified record to DB with VO by specified key(PK).
        /// </summary>
        void UpdateById(ICommonVO vo);

        /// <summary>
        /// To Update record(s) to DB with VO, The udpate condtion can be key(PK) or some parameters.
        /// </summary>
        void UpdateByParams(ICommonVO vo, string condtion, params object[] dbParameters);

        /// <summary>
        /// To Delete a record by specified key(PK).
        /// </summary>
        void DeleteById(object id);

        /// <summary>
        /// To Share a record by specified key(PK).
        /// </summary>
        void ShareById(object id);

        /// <summary>
        /// To Delete record(s) to the table with VO, The delete condtion can be key(PK) or some parameters.
        /// </summary>
        void DeleteByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// To control whether filter inactive object or not.
        /// </summary>
        bool IsCheckActive { get;set;}

        /// <summary>
        /// To control access right for your query statement for integration with security module.
        /// </summary>
        /// <param name="accessRightKey">
        /// The value of enum ActionEnum, e.g ActionEnum.Read, ActionEnum.FullControl, ActionEnum.Null,
        /// refer to CoreFramework.Security.VO.ActionEnum
        /// </param>
        /// <param name="orderByFieldName">The field name of order by</param>
        void SetAccessRight(object accessRightKey, string orderByFieldName);

        int CountByParams(string condtion, params object[] dbParameters);

        int CountByFilter(ISelectFilter filter); 
    }
}

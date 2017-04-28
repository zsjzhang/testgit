using System;
using System.Collections.Generic;

namespace Vcyber.BLMS.Application.CarService
{

    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    public interface ISonataService
    {

        /// <summary>
        /// 新增服务记录
        /// </summary>
        /// <param name="entity">服务数据</param>
        /// <returns>服务记录Id</returns>
        int Add(SonataServiceEntity entity);

        [Obsolete("请使用没有updatId和updateName参数的方法")]
        int Add(SonataServiceEntity entity, string updateId, string updateName);


        [Obsolete("请使用没有updatId和updateName参数的方法")]
        int Add(CSSonataService entity, string updateId, string updateName);

        /// <summary>
        /// 根据UserId查询服务记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        Page<CSSonataService> QueryOrdersByUserId(string userId, EBMServiceType type, long page, long itemsPerPage, string dealerId = null);

        /// <summary>
        /// 根据查询参数查询服务记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        Page<CSSonataService> QueryOrders(QueryParamEntity entity, long page, long itemsPerPage);

        IEnumerable<CSSonataService> QueryOrdersWithUpdate(QueryParamEntity entity);

        int UpdateState(int id, EOrderState state, string updateId, string updateName);

        /// <summary>
        /// 查询单项服务记录
        /// </summary>
        /// <param name="id">服务记录id(区别于OrderNo)</param>
        /// <returns></returns>
        CSSonataService GetEntityById(int id);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <returns></returns>
        bool IsSonataUser(string identity);

        bool IsTlcUser(string identityNumber);

        /// <summary>
        /// 获取服务剩余次数
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <param name="type">DMS服务类型</param>
        /// <returns>剩余免费次数</returns>
        int GetServiceCount(string identity, EDMSServiceType type);

        /// <summary>
        /// 获取用户所有服务的免费次数
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        Dictionary<int, int> GetServiceCount(string identity);

        /// <summary>
        /// 更新导出状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool UpdateIsExported(int id, string updateId, string updateName);

        /// <summary>
        /// 上门关怀 购车时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DateTime GetSonataBuyTime(string identitynumber);



      //  CSMaintenance GetMaintenanceyById(int UserId);
    }
}

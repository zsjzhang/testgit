using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;

    public interface ICarServiceBase<in T1, T2>
    {
        /// <summary>
        /// 新增服务记录
        /// </summary>
        /// <param name="entity">服务数据</param>
        /// <returns>服务记录Id</returns>
        int Add(T1 entity);
        
        [Obsolete("请使用没有updatId和updateName参数的方法")]
        int Add(T1 entity, string updateId, string updateName);


        [Obsolete("请使用没有updatId和updateName参数的方法")]
        int Add(T2 entity, string updateId, string updateName);

        /// <summary>
        /// 根据UserId查询服务记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        Page<T2> QueryOrdersByUserId(string userId, long page, long itemsPerPage, string dealerId = null);

        /// <summary>
        /// 根据查询参数查询服务记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        Page<T2> QueryOrders(QueryParamEntity entity, long page, long itemsPerPage);

        /// <summary>
        /// 导出数据的同时更新数据状态，暂时不用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Obsolete("Note implemeted", true)]
        IEnumerable<T2> QueryOrdersWithUpdate(QueryParamEntity entity);

        int UpdateState(int id, EOrderState state, string updateId, string updateName);

        /// <summary>
        /// 查询单项服务记录
        /// </summary>
        /// <param name="id">服务记录id(区别于OrderNo)</param>
        /// <returns></returns>
        T2 GetEntityById(int id);


        /// <summary>
        /// 查询单项服务记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
       // T2 GetEntityByUserid(int userid);
        /// <summary>
        /// 更新导出状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool UpdateIsExported(int id, string updateId, string updateName);
    }
}

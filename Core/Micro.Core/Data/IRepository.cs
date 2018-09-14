using Micro.Core.Domain;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Micro.Core.Data
{
    /// <summary>
    /// 数据仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
        where T : class, IEntity
    {
        /// <summary>
        /// 设置是否通过IsDeleted进行过滤
        /// </summary>
        /// <param name="isDeleteFilter">是否通过IsDeleted进行过滤</param>
        void SetDeleteFilter(bool isDeleteFilter = true);

        /// <summary>
        /// 加载字表数据
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="t"></param>
        /// <param name="keySelector"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task LoadChildrenAsync<TProperty>(T t, Expression<Func<T, IEnumerable<TProperty>>> keySelector, Expression<Func<TProperty, bool>> where = null)
            where TProperty :class, IEntity;

        #region Properties
        /// <summary>
        /// 获取具有跟踪状态的集合
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// 获取没有跟踪状态的集合
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        /// <summary>
        /// 工作单元, 用于触发SaveChangedAsync
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
        #endregion

        #region 新增
        /// <summary>
        /// 添加实例
        /// 1. 只是加入内存, 只有调用工作单位的SaveChangeAsync才会保存到数据库
        /// 2. 会自动更新PlatformId、Creator和DateCreated三个字段的值
        /// 3. entity 不能为空
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);
        /// <summary>
        /// 批量添加实例集合
        /// 1. 直接写入到数据库
        /// 2. 会自动更新PlatformId、Creator和DateCreated三个字段的值 
        /// 3. entities不能为空
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BulkAddAsync(IList<T> entities);
        #endregion

        #region 更新
        /// <summary>
        /// 更新实例
        /// 1. 只是修改跟踪状态， 未保存到数据库中，只有调用工作单位的SaveChangeAsync才会保存到数据库
        /// 2. 会对PlatformId 进行验证，如果操作人的PlatformId和实例的PlatformId不一致，将抛出非法操作的异常
        /// 3. 自动更新Modifier和DateModified两个字段的值
        /// 4. 参数entity不能为空
        /// 5. 要求用户必须登陆才能调用，如果未登陆， 将会抛出登陆过期的异常
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update(T entity);

        /// <summary>
        /// 批量更新实例
        /// 1. 直接保存到数据库中
        /// 2. 会对PlatformId 进行验证，如果集合中实例的PlatformId有一个或多个与操作人的PlatformId不一致，将抛出非法操作的异常
        /// 3. 自动更新Modifier和DateModified两个字段的值
        /// 4. entities
        /// 5. 要求用户必须登陆才能调用，如果未登陆， 将会抛出登陆过期的异常
        /// </summary>
        /// <param name="entities">领域实体集合</param>
        /// <returns></returns>
        Task<int> BulkUpdateAsync(IList<T> entities);
        /// <summary>
        /// 批量更新实例
        /// 1. 直接保存到数据库中
        /// 2. 会对PlatformId 进行验证，如果集合中实例的PlatformId有一个或多个与操作人的PlatformId不一致，将抛出非法操作的异常
        /// 3. 自动更新Modifier和DateModified两个字段的值
        /// 4. entities
        /// 5. 要求用户必须登陆才能调用，如果未登陆， 将会抛出登陆过期的异常
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <param name="expre">数据处理</param>
        /// <returns></returns>
        Task<int> BulkUpdateAsync(Expression<Func<T, bool>> where, Func<T, T> expre);
        #endregion

        #region 删除 
        /// <summary>
        /// 删除实例
        /// 1. 实例entity 不能为空
        /// 2. 实例的PlatformId必须与操作人的PlatformId一致,否则抛出非法操作的异常
        /// 3. 逻辑删除时, 值更新IsDeleted字段的值为True,并且自动更新自Modifier和DateModified两个字段的值
        /// 4. 只是操作内容, 没有保存到数据库，只有调用工作单位的SaveChangeAsync才会保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isLogicDelete">是否未逻辑删除</param>
        void Delete(T entity, bool isLogicDelete = true);
        /// <summary>
        /// 删除实例
        /// 1. entities 不能为空
        /// 2. 实例集合中的所有实例的PlatformId必须与操作人的PlatformId一致,否则抛出非法操作的异常
        /// 3. 逻辑删除时, 值更新IsDeleted字段的值为True,并且自动更新自Modifier和DateModified两个字段的值
        /// 4. 直接保存到数据库
        /// </summary>
        /// <param name="entities">领域实体集合</param>
        /// <param name="isLogicDelete">是否未逻辑删除</param>
        Task<int> BulkDeleteAsync(IList<T> entities, bool isLogicDelete = true);

        /// <summary>
        /// 删除实例
        /// 1. entities 不能为空
        /// 2. 实例集合中的所有实例的PlatformId必须与操作人的PlatformId一致,否则抛出非法操作的异常
        /// 3. 逻辑删除时, 值更新IsDeleted字段的值为True,并且自动更新自Modifier和DateModified两个字段的值
        /// 4. 直接保存到数据库
        /// </summary>
        /// <param name="idList">Id的集合</param>
        /// <param name="isLogicDelete">是否未逻辑删除</param>
        Task<int> BulkDeleteAsync(int[] idList, bool isLogicDelete = true);

        /// <summary>
        /// 删除实例
        /// 1. entities 不能为空
        /// 2. 实例集合中的所有实例的PlatformId必须与操作人的PlatformId一致,否则抛出非法操作的异常
        /// 3. 逻辑删除时, 值更新IsDeleted字段的值为True,并且自动更新自Modifier和DateModified两个字段的值
        /// 4. 直接保存到数据库
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <param name="isLogicDelete">是否未逻辑删除</param>
        Task BulkDeleteAsync(Expression<Func<T, bool>> where, bool isLogicDelete = true);
        #endregion

        #region 查询
        /// <summary>
        /// 通过标识获取实例
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <returns></returns>
        Task<T> GetAsync(int id);
        /// <summary>
        /// 通过条件表达式获取单个实例
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// 获取集合
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// 获取集合(没有跟踪状态)
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary> 
        /// <param name="where">查询条件</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        IPagedList<T> GetPagedList(Expression<Func<T, bool>> where, int pageNumber, int pageSize);
        /// <summary>
        /// 获取集合(没有跟踪状态)
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        IPagedList<T> GetPagedList<TKey>(Expression<Func<T, bool>> where, Func<T, TKey> keySelector, OrderType orderType, int pageNumber, int pageSize);
        #endregion
    }
    /// <summary>
    /// 排序类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 倒叙
        /// </summary>
        Desc,
        /// <summary>
        /// 顺序
        /// </summary>
        Asc
    }
}

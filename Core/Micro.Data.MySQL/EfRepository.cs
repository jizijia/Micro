using Core;
using Core.Data;
using Micro.Core;
using Micro.Core.Data;
using Micro.Core.Domain;
using Micro.Core.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.MySQL
{
    public class EfRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly IDbContext _context;
        private IUnitOfWork _unitOfWork;
        private DbSet<T> _entities;
        private bool _isDeleteFilter = true;
        private readonly ILogger<EfRepository<T>> _logger;

        /// <summary>
        /// 设置是否通过IsDeleted进行过滤
        /// </summary>
        /// <param name="isPlatformFilter"></param>
        public void SetDeleteFilter(bool isDeleteFilter = true)
        {
            _isDeleteFilter = isDeleteFilter;
        }

        /// <summary>
        /// 加载子表表数据
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="t"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public Task LoadChildrenAsync<TProperty>(T t, Expression<Func<T, IEnumerable<TProperty>>> keySelector, Expression<Func<TProperty, bool>> where = null)
            where TProperty : class, IEntity
        {
            var query = _context.Entry(t).Collection(keySelector).Query();
            //if (_isDeleteFilter)
            //{
            //    if (typeof(TProperty).IsAssignableFrom(typeof(ISoftDelete)))
            //    {
            //        query = query.Where(IsDeleteCondition(typeof(TProperty)));
            //    }
            //}
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.LoadAsync();
        }
        /// <summary>
        /// 分页加载子表表数据
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="t"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public Task LoadChildrenPageAsync<TProperty>(T t, Expression<Func<T, IEnumerable<TProperty>>> keySelector, Expression<Func<TProperty, bool>> where = null)
            where TProperty : BaseEntity
        {
            var query = _context.Entry(t).Collection(keySelector).Query();
            //if (_isDeleteFilter)
            //{
            //    query = query.Where(x => x.IsDeleted == false);
            //}
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.LoadAsync();
        }
        #region Ctor
        public EfRepository(IDbContext context, ILogger<EfRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 获取具有跟踪状态的集合
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                IQueryable<T> entitys = Entities;
                //if (_isDeleteFilter)
                //{
                //    entitys = entitys.Where(x => x.IsDeleted == false);
                //}
                return entitys;
            }
        }

        /// <summary>
        /// 获取没有跟踪状态的集合
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                IQueryable<T> entitys = Entities.AsNoTracking();
                //if (_isDeleteFilter)
                //{
                //    entitys = entitys.Where(x => x.IsDeleted == false);
                //}
                return entitys;
            }
        }

        /// <summary>
        /// 数据库数据集合(最基础,没有任何过滤)
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        /// <summary>
        /// 工作单元, 用于触发SaveChangedAsync
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    _unitOfWork = new UnitOfWork<T>(_context, _logger);
                }
                return _unitOfWork;
            }
        }
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
        public T Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return Entities.Add(entity).Entity;
        }
        /// <summary>
        /// 批量添加实例集合
        /// 1. 直接写入到数据库
        /// 2. 会自动更新PlatformId、Creator和DateCreated三个字段的值 
        /// 3. entities不能为空
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task BulkAddAsync(IList<T> entities)
        {
            foreach (var item in entities)
            {
                Add(item);
            }
            return UnitOfWork.SaveChangesAsync();
        }
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
        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// 批量更新实例(必须为实体)
        /// 1. 直接保存到数据库中
        /// 2. 会对PlatformId 进行验证，如果集合中实例的PlatformId有一个或多个与操作人的PlatformId不一致，将抛出非法操作的异常
        /// 3. 自动更新Modifier和DateModified两个字段的值
        /// 4. entities
        /// 5. 要求用户必须登陆才能调用，如果未登陆， 将会抛出登陆过期的异常
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> BulkUpdateAsync(IList<T> entities)
        {
            foreach (var item in entities)
            {
                Update(item);
            }
            return UnitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 批量更新实例
        /// 1. 直接保存到数据库中
        /// 2. 会对PlatformId 进行验证，如果集合中实例的PlatformId有一个或多个与操作人的PlatformId不一致，将抛出非法操作的异常
        /// 3. 自动更新Modifier和DateModified两个字段的值
        /// 4. entities
        /// 5. 要求用户必须登陆才能调用，如果未登陆， 将会抛出登陆过期的异常
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> BulkUpdateAsync(Expression<Func<T, bool>> where, Func<T, T> expre)
        {
            var list = await Table.Where(where).ToListAsync();
            list.ForEach(x =>
            {
                expre(x);
            });
            return await BulkUpdateAsync(list);
        }
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
        public void Delete(T entity, bool isLogicDelete = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (isLogicDelete)
            {
                Update(entity);
            }
            else
                Entities.Remove(entity);
        }
        /// <summary>
        /// 删除实例
        /// 1. entities 不能为空
        /// 2. 实例集合中的所有实例的PlatformId必须与操作人的PlatformId一致,否则抛出非法操作的异常
        /// 3. 逻辑删除时, 值更新IsDeleted字段的值为True,并且自动更新自Modifier和DateModified两个字段的值
        /// 4. 直接保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isLogicDelete">是否未逻辑删除</param>
        public Task<int> BulkDeleteAsync(IList<T> entities, bool isLogicDelete = true)
        {
            foreach (var item in entities)
            {
                Delete(item, isLogicDelete);
            }
            return UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除实例
        /// 1. entities 不能为空
        /// 2. 实例集合中的所有实例的PlatformId必须与操作人的PlatformId一致,否则抛出非法操作的异常
        /// 3. 逻辑删除时, 值更新IsDeleted字段的值为True,并且自动更新自Modifier和DateModified两个字段的值
        /// 4. 直接保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isLogicDelete">是否未逻辑删除</param>
        public Task<int> BulkDeleteAsync(int[] idList, bool isLogicDelete = true)
        {
            if (idList == null)
                throw new ArgumentNullException(nameof(idList));
            var list = Table.Where(c => idList.Contains(c.Id)).ToList();
            if (list == null)
            {
                return Task.FromResult(0);
            }
            return BulkDeleteAsync(list, isLogicDelete);
        }

        /// <summary>
        /// 删除实例
        /// 1. entities 不能为空
        /// 2. 实例集合中的所有实例的PlatformId必须与操作人的PlatformId一致,否则抛出非法操作的异常
        /// 3. 逻辑删除时, 值更新IsDeleted字段的值为True,并且自动更新自Modifier和DateModified两个字段的值
        /// 4. 直接保存到数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isLogicDelete">是否未逻辑删除</param>
        public virtual Task BulkDeleteAsync(Expression<Func<T, bool>> where, bool isLogicDelete = true)
        {
            var deleteEntities = Table.Where(where).ToList();
            if (deleteEntities == null)
            {
                return Task.FromResult(0);
            }
            return BulkDeleteAsync(deleteEntities, isLogicDelete);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 通过标识获取实例
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> GetAsync(int id)
        {
            var entity = Table.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(T), id);
            return entity;
        }
        /// <summary>
        /// 通过条件表达式获取单个实例
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            var entity = Table.FirstOrDefaultAsync(where);
            if (entity == null)
                throw new EntityNotFoundException(typeof(T), where.ToString());
            return entity;
        }

        /// <summary>
        /// 获取集合
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> where)
        {
            return Table.Where(where).ToListAsync();
        }

        /// <summary>
        /// 获取集合(没有跟踪状态)
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary> 
        /// <param name="where">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public IPagedList<T> GetPagedList(Expression<Func<T, bool>> where, int pageNumber, int pageSize)
        {
            return GetPagedList(where, (x => x.Id), OrderType.Asc, pageNumber, pageSize);
        }

        /// <summary>
        /// 获取集合(没有跟踪状态)
        /// 1. 如果设置平台过滤, 获取的数据会对平台字段进行过滤, 且用户没有登陆, 将抛出会员登陆过期的异常
        /// 2. 如果设置删除过滤, 获取的数据会对删除状态字段进行过滤
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public IPagedList<T> GetPagedList<TKey>(Expression<Func<T, bool>> where, Func<T, TKey> keySelector, OrderType orderType, int pageNumber, int pageSize)
        {
            var query = TableNoTracking;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (keySelector != null)
            {
                if (orderType == OrderType.Desc)
                {
                    query = query.OrderBy(keySelector).AsQueryable();
                }
                else
                {
                    query = query.OrderBy(keySelector).AsQueryable();
                }
            }
            return query.ToPagedList(pageNumber, pageSize);
        }


        #endregion
        #region Private method
        //private Expression<Func<TSoftDelete, bool>> IsDeleteCondition<TSoftDelete>(ISoftDelete softDelete)
        //    where TSoftDelete : ISoftDelete
        //{
        //    return PredicateBuilder.CreateEqual<TSoftDelete>(nameof(ISoftDelete.IsDeleted), "1");
        //}
        #endregion

    }
}

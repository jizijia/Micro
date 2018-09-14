using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Core.Domain
{
    /// <summary>
    /// 领域实体基类
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        int _Id;
        /// <summary>
        /// 标识
        /// </summary>
        public virtual int Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }
        private List<INotification> _domainEvents;
        /// <summary>
        /// 领域实体事件集合, 只读
        /// </summary>
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        /// <summary>
        /// 移除领域事件
        /// </summary>
        /// <param name="eventItem"></param>
        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }
        /// <summary>
        /// 清空领域事件
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}

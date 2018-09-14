using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Core.Domain
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Check if this Entity is null of marked as deleted.
        /// </summary>
        public static bool IsNullOrDeleted(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }

        /// <summary>
        /// Undeletes this entity by setting <see cref="ISoftDelete.IsDeleted"/> to false and
        /// <see cref="IDeletionAudited"/> properties to null.
        /// </summary>
        public static void UnDelete(this ISoftDelete entity)
        {
            entity.IsDeleted = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Core.Data.Query
{
    /// <summary>
    /// 查询条件帮助
    /// </summary>
    public static class QueryableWhereExtentions
    {
        /// <summary>
        /// !string.IsNullOrWhiteSpace(value)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, string value, Expression<Func<T, bool>> expression)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, int value, Expression<Func<T, bool>> expression)
        {
            if (value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value.HasValue && value.Value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, int? value, Expression<Func<T, bool>> expression)
        {
            if (value.HasValue && value.Value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, bool value, Expression<Func<T, bool>> expression)
        {
            query = query.Where(expression);
            return query;
        }
        /// <summary>
        /// value.HasValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, bool? value, Expression<Func<T, bool>> expression)
        {
            if (value.HasValue)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value > DateTime.MinValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, DateTime value, Expression<Func<T, bool>> expression)
        {
            if (value > DateTime.MinValue)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value.HasValue && value.Value > DateTime.MinValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, DateTime? value, Expression<Func<T, bool>> expression)
        {
            if (value.HasValue && value.Value > DateTime.MinValue)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, decimal value, Expression<Func<T, bool>> expression)
        {
            if (value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value.HasValue && value.Value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, decimal? value, Expression<Func<T, bool>> expression)
        {
            if (value.HasValue && value.Value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, float value, Expression<Func<T, bool>> expression)
        {
            if (value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value.HasValue && value.Value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, float? value, Expression<Func<T, bool>> expression)
        {
            if (value.HasValue && value.Value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, double value, Expression<Func<T, bool>> expression)
        {
            if (value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
        /// <summary>
        /// value.HasValue && value.Value > 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IQueryable<T> TryAppend<T>(this IQueryable<T> query, double? value, Expression<Func<T, bool>> expression)
        {
            if (value.HasValue && value.Value > 0)
            {
                query = query.Where(expression);
            }
            return query;
        }
    }
}

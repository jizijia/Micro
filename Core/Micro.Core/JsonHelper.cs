using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micro.Core
{
    /// <summary>
    /// Json帮助类
    /// 依赖Newtonsoft.Json
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 转成动态类型对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic ToDic(string value)
        {
            var obj = JsonConvert.DeserializeObject(value) as JToken;

            return ToDic(obj);
        }
        /// <summary>
        /// 转成动态类型对象
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static dynamic ToDic(JToken token)
        {
            var list = new List<dynamic>();
            var item = new Dictionary<string, object>();

            switch (token.Type)
            {
                case JTokenType.Array:
                    for (var i = 0; i < token.Count(); i++)
                    {
                        list.Add(ToDic(token[i]));
                    }
                    return list;
                case JTokenType.Object:
                    var to = token as JObject;
                    var node = to.First;
                    do
                    {
                        var tp = node as JProperty;
                        item.Add(tp.Name, ToDic(tp.Value));
                        node = node.Next;
                    }
                    while (node != null);
                    return item;
                default:
                    return (token as dynamic).Value;
            }
        }
        /// <summary>
        /// 将Json字符串转成实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToClass<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 将Json字符串转成实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T ToClass<T>(string value, T type)
        {
            try
            {
                if (value == null)
                {
                    return default(T);
                }
                return JsonConvert.DeserializeAnonymousType(value, type);
            }
            catch (Exception e)
            {
                Console.WriteLine(value);
                throw e;
            }
        }
        /// <summary>
        /// 将对象转成Json字符串
        /// 配置:
        ///     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        ///     DateFormatString = "yyyy'/'MM'/'dd' 'HH':'mm':'ss"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy'/'MM'/'dd' 'HH':'mm':'ss"
            });
        }
        /// <summary>
        /// 将Json字符串转成实例集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> ToClassList<T>(string value, T type)
        {
            List<T> temp = null;
            return JsonConvert.DeserializeAnonymousType(value, temp);
        }

    }
}

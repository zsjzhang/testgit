using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    using System.Web;
    using System.Web.Caching;

    public class VcyberCache
    {
        /// <summary>
        /// Add object to cache. Object with the same key would be overwritten.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="priority"></param>
        /// <param name="onRemoveCallback"></param>
        public static void Add(
            string key,
            object value,
            CacheDependency dependencies,
            DateTime absoluteExpiration,
            TimeSpan slidingExpiration,
            CacheItemPriority priority,
            CacheItemRemovedCallback onRemoveCallback)
        {
            HttpRuntime.Cache.Insert(
                key,
                value,
                dependencies,
                absoluteExpiration,
                slidingExpiration,
                priority,
                onRemoveCallback);
        }

        public static object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public static object Remove(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }
    }
}

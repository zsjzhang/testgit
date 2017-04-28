using System;
namespace Vcyber.BLMS.Common
{
    public interface ICacheHelp
    {
        object GetCache(string CacheKey);
        void  RemoveCache(string CacheKey);
        void SetCache(string CacheKey, object objObject);
        void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration);
    }
}

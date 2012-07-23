using System;
using DowJones.Mapping;
using GWCacheScope = Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope;
namespace DowJones.Caching
{
    public enum CacheScope
    {
        Session = 0,
        User,
        Account,
        All
    }

    public class CacheScopeToSessionCacheScopeMapper :
        ITypeMapper<CacheScope, GWCacheScope>,
        ITypeMapper<GWCacheScope, CacheScope>
    {
        public GWCacheScope Map(CacheScope source)
        {
            switch (source)
            {
                case CacheScope.Session:
                    return GWCacheScope.Session;
                case CacheScope.User:
                    return GWCacheScope.User;
                case CacheScope.Account:
                    return GWCacheScope.Account;
                case CacheScope.All:
                    return GWCacheScope.All;
            }
            throw new NotSupportedException();
        }

        public CacheScope Map(GWCacheScope source)
        {
            switch (source)
            {
                case GWCacheScope.Session:
                    return CacheScope.Session;
                case GWCacheScope.User:
                    return CacheScope.User;
                case GWCacheScope.Account:
                    return CacheScope.Account;
                case GWCacheScope.All:
                    return CacheScope.All;
            }
            throw new NotSupportedException();
        }

        public object Map(object source)
        {
            if (source is CacheScope)
                return Map((CacheScope)source);
            if (source is GWCacheScope)
                return Map((GWCacheScope)source);

            throw new NotSupportedException();
        }
    }
}
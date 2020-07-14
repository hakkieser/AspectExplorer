using AspectExplorer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer
{
    public class AspectExplorerCacheAttribute : AspectBase
    {
        public double DurationMinute { get; set; } = 10;

        public virtual void OnAfterAddToCache(RealTypeResponseArgument param, MethodContext _methodContext)
        {
            string cacheKey = string.Format("{0}_{1}", _methodContext.MethodName, string.Join("_", _methodContext.Arguments));

            if (MemoryCache.Default.Get(cacheKey) == null && param.Value != null && param.IsRealTypeValue)
            {
                MemoryCache.Default.Add(cacheKey, param.Value, DateTimeOffset.Now.AddMinutes(DurationMinute));
            }
        }
        public virtual Object OnBeforeWithReturnValueGetFromCache(MethodContext _methodContext)
        {
            string cacheKey = string.Format("{0}_{1}", _methodContext.MethodName, string.Join("_", _methodContext.Arguments)); 
            object cachedItem = MemoryCache.Default.Get(cacheKey);


            Console.WriteLine("{0} isimli cache key ile cache üzerinden geliyorum!", cacheKey);
            return cachedItem;
        }


        public override void OnAfter(RealTypeResponseArgument param, MethodContext _methodContext)
        {
            this.OnAfterAddToCache(param, _methodContext);
        }  
        public override object OnBeforeWithReturnValue(MethodContext _methodContext)
        {
            return this.OnBeforeWithReturnValueGetFromCache(_methodContext);
        }
    }
}

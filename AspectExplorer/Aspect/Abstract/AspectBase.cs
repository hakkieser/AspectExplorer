using AspectExplorer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer
{
    public abstract class AspectBase : Attribute
    {
        public AspectBase()
        {  }

        public int Priority { get; set; } 

        //JOIN POINTS ON AFTER 
        public virtual void OnAfter(RealTypeResponseArgument _param, MethodContext _methodContext) { }

        //JOIN POINTS ON BEFORE 
        public virtual void OnBefore(MethodContext _methodContext) {   } 
        public virtual object OnBeforeWithReturnValue(MethodContext _methodContext) { return null; }

    }
}

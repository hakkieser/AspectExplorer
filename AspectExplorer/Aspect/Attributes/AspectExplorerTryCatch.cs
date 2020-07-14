using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace AspectExplorer
{
    public class AspectExplorerTryCatch : AspectBase
    {
        public virtual object MethodInvoke(MethodContext _methodContext)
        {
            object result = null;
            result = _methodContext.Invoke();
            result = result ?? new Object();

            return result;
        }

        public override object OnBeforeWithReturnValue(MethodContext _methodContext)
        {
            object result = null;
            if (this.GetType().GetMethods().FirstOrDefault(x => x.Name == "MethodInvoke").DeclaringType
                == typeof(AspectExplorerTryCatch).GetMethods().FirstOrDefault(x => x.Name == "MethodInvoke").DeclaringType)
            {
                try
                {
                    result = this.MethodInvoke(_methodContext);
                }
                catch (Exception ex)
                {
                    result = ex;
                }
            }
            else
            {
                result = this.MethodInvoke(_methodContext);
            }

            return result;
        }

    }
}

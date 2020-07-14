using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer
{
    public class MethodContext
    {
        public MethodContext()
        { }

        public IMethodCallMessage MethodBase { get; set; }
        public Type RealReturnType { get; set; }
        public string MethodName { get; set; }
        public object[] Arguments { get; set; }
        public bool IsInvoke { get; set; } = false;
        public Type RealServiceType { get; set; }
        private object _realServiceInstance;
        public object RealServiceInstance
        {
            get
            {
                return _realServiceInstance;
            }
            set
            {
                _realServiceInstance = value;
            }
        }

        private object _proxyServiceInstance;
        public object ProxyServiceInstance
        {
            get
            {
                if (_proxyServiceInstance == null)
                {
                    _proxyServiceInstance = Activator.CreateInstance(this.RealServiceType);
                }
                return _proxyServiceInstance;
            }
            set
            {
                if (_proxyServiceInstance == null)
                {
                    _proxyServiceInstance = value;
                }
            }
        }

        public object Invoke()
        {
            object _result = null;
            if (this.IsInvoke == false)
            {
                this.IsInvoke = true;
                IMethodCallMessage _realMessage = this.MethodBase;
                _result = _realMessage.MethodBase.Invoke(this.RealServiceInstance, _realMessage.InArgs);
            }

            return _result;
        }
    }
}

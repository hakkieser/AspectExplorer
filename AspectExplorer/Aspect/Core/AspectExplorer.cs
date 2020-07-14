using AspectExplorer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer
{

    public static class AspectExplorerExtension
    {
        public static TI CreateProxyInstance<TI>(this TI @this) where TI : MarshalByRefObject, new()
        {
            return AspectExplorer<TI>.CreateProxyInstance(@this);
        }
    }

    public class AspectExplorer<T> : RealProxy where T : MarshalByRefObject, new()
    {
        private AspectExplorer() : base(typeof(T))
        {
            this.MethodContext = new MethodContext();
        }

        public MethodContext MethodContext { get; set; }
        public static T CreateProxyInstance(T _realServiceInstance)
        {
            AspectExplorer<T> instance = new AspectExplorer<T>();
            instance.MethodContext.ProxyServiceInstance = (T)instance.GetTransparentProxy();
            instance.MethodContext.RealServiceInstance = _realServiceInstance;
            return (T)instance.MethodContext.ProxyServiceInstance;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage _methodCallMessage = msg as IMethodCallMessage;
            ReturnMessage _returnMessage = null;

            try
            {
                Type _realServiceType = typeof(T);
                MethodInfo _mInfo = _realServiceType.GetMethod(_methodCallMessage.MethodName);
                object[] _aspects = this.GetAspects(_mInfo, _realServiceType); // _mInfo.GetCustomAttributes(typeof(AspectBase), true);


                _aspects = _aspects.OrderBy(o => (o as AspectBase).Priority).ToArray();

                //
                FillMethodContext(_methodCallMessage, _mInfo.ReturnType, _realServiceType);

                //önce araya  giriyoruz. 
                //JOIN POINT 1 : BEFORE ASPECTS
                object _response = CheckBeforeAspect(_aspects);

                //
                //real type method before aspect' de calısmadıysa çalışsın (if real type method for before don't work. let the method work )
                if (!this.MethodContext.IsInvoke)
                {
                    _response = this.MethodContext.Invoke();
                }

                //return message dolduruldu.
                _returnMessage = new ReturnMessage(_response, null, 0, _methodCallMessage.LogicalCallContext, _methodCallMessage);

                //
                //bu bölüme kadar olanda real type method çalışmasını kesinlikle bitirdi.
                //

                //sonraki işlemler için real type doğrulama işlemi yapılıyor.
                RealTypeResponseArgument _realTypeResponseArgumentItem = new RealTypeResponseArgument()
                {
                    Value = _response,
                    IsRealTypeValue = _response != null && _response.GetType().GUID == this.MethodContext.RealReturnType.GUID
                };

                //sonra araya giriyoruz
                CheckAfterAspect(_realTypeResponseArgumentItem, _aspects);

                //son olarak geri dönüş değeri vaadedilen geri dönüş değeri değilse null göndermek mantıklı olacaktır :)
                if (!_realTypeResponseArgumentItem.IsRealTypeValue)
                {
                    _returnMessage = new ReturnMessage(null, null, 0, _methodCallMessage.LogicalCallContext, _methodCallMessage);
                }

                return _returnMessage;
            }
            catch (Exception ex)
            {
                return new ReturnMessage(null, null, 0, _methodCallMessage.LogicalCallContext, _methodCallMessage);
            }
        }

        public object[] GetAspects(MethodInfo _mInfo, Type RealServiceType)
        {
            List<object> _aspects = new List<object>();

            _aspects.AddRange(_mInfo.GetCustomAttributes(typeof(AspectBase), true));

            _aspects.AddRange(RealServiceType.GetInterfaces().First().GetMethod(_mInfo.Name).GetCustomAttributes(typeof(AspectBase), true));
          
            _aspects.AddRange(AttributeToRealTypeMapper.GetAttributesForMethodFromConfig(_mInfo));

            _aspects = _aspects.Distinct().ToList();
            return _aspects.ToArray();
        }

        private void FillMethodContext(IMethodCallMessage _methodCallMessage, Type _realReturnType, Type _realServiceType)
        {
            this.MethodContext.Arguments = _methodCallMessage.InArgs;
            this.MethodContext.IsInvoke = false;
            this.MethodContext.MethodBase = _methodCallMessage;
            this.MethodContext.MethodName = _methodCallMessage.MethodName;
            this.MethodContext.RealReturnType = _realReturnType;
            this.MethodContext.RealServiceType = _realServiceType;
        }

        private object CheckBeforeAspect(object[] _aspects)
        {
            object response = null;
            foreach (AspectBase itemAttribute in _aspects)
            {
                itemAttribute.OnBefore(this.MethodContext);

                //response değeri eğer öncelik sıralamasına göre bir şekilde atanmışsın
                //sonraki attribute ların response' a değer atması engellenir.
                if (response == null)
                {
                    object _methodResponse = itemAttribute.OnBeforeWithReturnValue(this.MethodContext);
                    response = _methodResponse;
                }
            }

            if (response != null)
            {
                this.MethodContext.IsInvoke = true;
            }

            return response;
        }

        private void CheckAfterAspect(RealTypeResponseArgument _realTypeResponse, object[] _aspects)
        {
            foreach (AspectBase itemAttribute in _aspects)
            {
                itemAttribute.OnAfter(_realTypeResponse, this.MethodContext);
            }
        }

    }
}

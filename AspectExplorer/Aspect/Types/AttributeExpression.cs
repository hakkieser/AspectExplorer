using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer.Types
{

    public class AttributeExpression
    {
        public Attribute AttributeObject { get; set; }
        public AttributeExpression(Attribute _attribute)
        {
            this.AttributeObject = _attribute;
        }

        public MethodExpression<TBaseClass> ToClass<TBaseClass>()
        {
            new AttributeToRealTypeMapper(typeof(TBaseClass), this.AttributeObject);

            return new MethodExpression<TBaseClass>(this.AttributeObject);
        }

        public MethodExpression<TBaseInterface> ToInterface<TBaseInterface>()
        {
            if (typeof(TBaseInterface).IsInterface)
            {
                AttributeToRealTypeMapper.AttributeClassMap.Add(
                    new AttributeToRealTypeMapper(typeof(TBaseInterface), this.AttributeObject));
            }

            return new MethodExpression<TBaseInterface>(this.AttributeObject);
        }
    }
}

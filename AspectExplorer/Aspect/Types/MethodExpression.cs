using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer.Types
{ 

    public class MethodExpression<TClass> 
    {
        public Attribute AttributeObject { get; set; }
        public MethodExpression(Attribute _attribute)
        {
            AttributeObject = _attribute;
        }

        public void ExcludeMethods(params string[] _methodNames) {
            AttributeToRealTypeMapper.ExcludeMethodsFromRealType(AttributeObject, typeof(TClass), _methodNames);
        }

        public void IncludeMethods(params string[] _methodNames)
        {
            AttributeToRealTypeMapper.IncludeMethodsFromRealType(AttributeObject, typeof(TClass), _methodNames);
        }

    }
}

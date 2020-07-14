using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer.Types
{
    public class AttributeToRealTypeMapper
    {

        public Attribute AttributeObject { get; set; }
        public Type RealType { get; set; }
        public MethodInfo[] Methods { get; set; }

        private AttributeToRealTypeMapper()
        {

        }
        public AttributeToRealTypeMapper(Type _realType, Attribute _attribute)
        {
            AttributeToClass(_realType, _attribute);
        }

        public static List<AttributeToRealTypeMapper> AttributeClassMap = new List<AttributeToRealTypeMapper>();
        private static void AttributeToClass(Type _realType, Attribute _attribute)
        {
            AttributeClassMap.Add(new AttributeToRealTypeMapper()
            {
                AttributeObject = _attribute,
                RealType = _realType,
                Methods = _realType.GetMethods()
            });
        }

        public static void ExcludeMethodsFromRealType(Attribute _attribute, Type _realType, params string[] _methodNames)
        {
            int _indexOf = AttributeClassMap
                             .IndexOf(AttributeClassMap
                             .SingleOrDefault(i => i.RealType.GUID == _realType.GUID && i.AttributeObject.GetType().GUID == _attribute.GetType().GUID));

            AttributeToRealTypeMapper _resultRealTypeFromAttribute = null;
            if (_indexOf > -1)
            {
                _resultRealTypeFromAttribute = AttributeClassMap[_indexOf];
            }

            if (_resultRealTypeFromAttribute != null)
            {
                _resultRealTypeFromAttribute.Methods = AttributeClassMap[_indexOf].Methods.Where(x => !_methodNames.Contains(x.Name)).ToArray();
            }
        }

        public static void IncludeMethodsFromRealType(Attribute _attribute, Type _realType, params string[] _methodNames)
        {
            int _indexOf = AttributeClassMap
                             .IndexOf(AttributeClassMap
                             .SingleOrDefault(i => i.RealType.GUID == _realType.GUID && i.AttributeObject.GetType().GUID == _attribute.GetType().GUID));

            AttributeToRealTypeMapper _resultRealTypeFromAttribute = null;
            if (_indexOf > -1)
            {
                _resultRealTypeFromAttribute = AttributeClassMap[_indexOf];
            }

            if (_resultRealTypeFromAttribute != null)
            {
                _resultRealTypeFromAttribute.Methods = AttributeClassMap[_indexOf].Methods.Where(x => _methodNames.Contains(x.Name)).ToArray();
            }
        }

        public static Attribute[] GetAttributesForMethodFromConfig(MethodInfo _mInfo)
        {
            Attribute[] resultAttributes = null;

            resultAttributes = AttributeClassMap
                .Where(x => x.RealType.GUID == _mInfo.DeclaringType.GUID && x.Methods.Count(c => c.Name == _mInfo.Name) > 0)
                .Select(s => s.AttributeObject)
                .ToArray();

            return resultAttributes;
        }
    }
}

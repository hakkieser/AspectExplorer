using AspectExplorer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer
{
    public class AspectExplorerConfiguration
    {
        public AspectExplorerConfiguration(Action<AspectExplorerConfiguration> configure)
        {
            configure.Invoke(this);
        }

        public AttributeExpression SetAttribute(Attribute _attribute)
        {
            return new AttributeExpression(_attribute);
        } 

    }
}

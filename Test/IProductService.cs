using AspectExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    public interface IProductService
    {
        [AspectExplorerCache(Priority = 1)]
        [AspectExplorerTryCatch(Priority = 2)]
        [AspectExplorerLog(Priority = 3)]
        Product GetProduct(int productId);

        [AspectExplorerTryCatch(Priority = 1)]
        [AspectExplorerLog(Priority = 2)]
        void AddProduct(int _key, Product _productItem);
    }
}

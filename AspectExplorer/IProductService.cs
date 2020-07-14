using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspectExplorer
{
    public interface IProductService
    {
        Product GetProduct(int productId); 
        void AddProduct(int _key, Product _productItem);
    }
}

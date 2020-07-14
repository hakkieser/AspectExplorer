using AspectExplorer; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public abstract class Entity
    {

    }

    public class Product
    {
        public Product()
        { }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class ProductService : MarshalByRefObject, IProductService
    {
        private static Dictionary<int, Product> _InMemDb = new Dictionary<int, Product>();

        public ProductService()
        {
            _InMemDb.Add(1, new Product() { Id = 1, Name = "MacBook Air", Price = 3000 });
            _InMemDb.Add(2, new Product() { Id = 2, Name = "Sony Xperia", Price = 1400 });
        }
        
 
        public Product GetProduct(int productId)
        { 
            return _InMemDb[productId];
        }

  
        public void AddProduct(int _key, Product _productItem) {
            _InMemDb.Add(_key, _productItem);
        }
    }
}

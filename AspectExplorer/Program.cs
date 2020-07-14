using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspectExplorer
{
    class Program
    {
        static void Main(string[] args)
        {

            var config2 = new AspectExplorerConfiguration(cfg =>
            {
                cfg.SetAttribute(new AspectExplorerLogAttribute() { Priority = 2 }).ToClass<ProductService>().ExcludeMethods("AddProduct");
                cfg.SetAttribute(new AspectExplorerCacheAttribute() { Priority = 1, DurationMinute = 10 }).ToClass<ProductService>().IncludeMethods("GetProduct");
            });

            IProductService _p = new ProductService().CreateProxyInstance();

            _p.AddProduct(3, new Product() { Id = 1, Name = "Casper", Price = 1800d });

            _p.GetProduct(1);
            _p.GetProduct(1);

            Console.ReadKey();


            //var config = new MapperConfiguration(cfg => {

            //    cfg.CreateMap<string, int>(); 
            //}).CreateMap<Product, string>()
            //     .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));


            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<string, string>().ForMember(x=> x;
            //});

        }
    }
}

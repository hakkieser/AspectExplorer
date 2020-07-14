using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspectExplorer;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var config2 = new AspectExplorerConfiguration(cfg =>
            //{
            //    cfg.SetAttribute(new AspectExplorerCacheAttribute() { DurationMinute = 10, Priority = 1 }).ToClass<ProductService>().ExcludeMethods();
            //});

            IProductService _p = new ProductService().CreateProxyInstance();

            _p.AddProduct(3, new Product() { Id = 1, Name = "Casper", Price = 1800d });

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

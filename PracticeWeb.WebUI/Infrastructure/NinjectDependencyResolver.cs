using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;
using Ninject;
using Moq;
using Practice.Domain.Abstract;
using Practice.Domain.Concrete;
using Practice.Domain.Entities;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace PracticeWeb.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product {Name="SanDisk Ultra 3D 1TB SSD", Brand="SanDisk", Price=2988},
            //    new Product {Name="Micron MX500 1TB SSD", Brand="Micron", Price=3400},
            //    new Product {Name="Samsung 860 QVO 1TB SSD", Brand="Samsung", Price=2999}
            //});
            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
            kernel.Bind<IOrderLogger>().To<DbOrderLogger>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "true")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IUserManager>().To<RealUserManager>();
        }
    }
}
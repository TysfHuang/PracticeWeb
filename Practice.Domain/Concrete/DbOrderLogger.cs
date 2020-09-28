using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;

namespace Practice.Domain.Concrete
{
    public class DbOrderLogger : IOrderLogger
    {
        private IProductRepository repository;

        public DbOrderLogger(IProductRepository repo)
        {
            repository = repo;
        }

        public void RecordOrder(Cart cart, string userId, string receiverName, string shippingAddress)
        {
            if (cart.Lines.Count() == 0)
                return;
            ProductOrder order = new ProductOrder();
            order.AppUserID = userId;
            order.ReceiverName = receiverName;
            order.Date = DateTime.Now;
            order.ShippingAddress = shippingAddress;
            foreach (CartLine data in cart.Lines)
            {
                order.ConvertProductListToDbFormat(data.Product.ID, data.Product.Name, data.Quantity, data.Product.Price);
            }
            repository.SaveProductOrder(order);
        }
    }
}

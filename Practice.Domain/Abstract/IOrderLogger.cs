using Practice.Domain.Entities;

namespace Practice.Domain.Abstract
{
    public interface IOrderLogger
    {
        void RecordOrder(Cart cart, string userId, string receiverName, string shippingAddress);
    }
}

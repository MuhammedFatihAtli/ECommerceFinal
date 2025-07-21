using ECommerce.Application.DTOs.BasketDTOs;
using ECommerce.Application.DTOs.ProductDTOs;

namespace ECommerce.MVC.Services
{
    public interface ICartService
    {
        CartDTO GetCart(ISession session);
        void AddToCart(ISession session, ProductDTO product, int quantity);
        void RemoveFromCart(ISession session, int productId);
        void ClearCart(ISession session);
    }
}

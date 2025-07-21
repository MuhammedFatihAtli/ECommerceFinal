using ECommerce.Application.DTOs.BasketDTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.MVC.Extensions;

namespace ECommerce.MVC.Services
{
    public class CartService : ICartService
    {
        private const string CartSessionKey = "cart";

        public CartDTO GetCart(ISession session)
        {
            return session.GetObject<CartDTO>(CartSessionKey) ?? new CartDTO();
        }
        public void AddToCart(ISession session, ProductDTO product, int stock)
        {
            var cart = GetCart(session);

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Stock += stock;
            }
            else
            {
                cart.Items.Add(new CartItemDTO
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.Image,
                    UnitPrice = product.Price,
                    Stock= stock
                });
            }

            session.SetObject(CartSessionKey, cart);
        }

        public void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }

        

        public void RemoveFromCart(ISession session, int productId)
        {
            var cart = GetCart(session);
            cart.Items.RemoveAll(i => i.ProductId == productId);
            session.SetObject(CartSessionKey, cart);
        }
    }
}

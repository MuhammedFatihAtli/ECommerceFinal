using ECommerce.Application.DTOs.BasketDTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.MVC.Extensions;

namespace ECommerce.MVC.Services
{
    // Sepet işlemlerini gerçekleştiren servis sınıfı
    public class CartService : ICartService
    {// Session içinde sepetin tutulacağı anahtar ismi sabit olarak tanımlanır
        private const string CartSessionKey = "cart";
        // Session'dan sepet nesnesini getirir, eğer yoksa yeni boş bir CartDTO döner
        public CartDTO GetCart(ISession session)
        {
            return session.GetObject<CartDTO>(CartSessionKey) ?? new CartDTO();
        }
        // Sepete ürün ekler. Eğer ürün zaten varsa miktarını arttırır, yoksa yeni ürün ekler
        public void AddToCart(ISession session, ProductDTO product, int stock)
        {
            // Mevcut sepet alınır
            var cart = GetCart(session);
            // Sepette aynı ürün daha önce eklenmiş mi kontrol edilir
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                // Ürün zaten sepette varsa, stok adedi güncellenir
                existingItem.Stock += stock;
            }
            else
            {
                // Ürün sepette yoksa, yeni bir sepet öğesi (CartItemDTO) olarak eklenir
                cart.Items.Add(new CartItemDTO
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.Image,
                    UnitPrice = product.Price,
                    Stock= stock
                });
            }
            // Güncellenmiş sepet, session'a kaydedilir
            session.SetObject(CartSessionKey, cart);
        }
        // Sepeti tamamen temizler (session'dan kaldırır)
        public void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }

        // Sepetten belirli bir ürünü çıkarır

        public void RemoveFromCart(ISession session, int productId)
        { // Güncel sepet alınır
            var cart = GetCart(session);
            // Belirtilen ürün ID’sine sahip tüm öğeler sepetten silinir
            cart.Items.RemoveAll(i => i.ProductId == productId);
            // Güncellenmiş sepet tekrar session’a kaydedilir
            session.SetObject(CartSessionKey, cart);
        }
    }
}

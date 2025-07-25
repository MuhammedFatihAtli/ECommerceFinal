using ECommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.UnitOfWorks
{
    /// <summary>
    /// Ürün yönetimi, kullanıcı yönetimi, sipariş işleme ve daha fazlası gibi çeşitli alanlara özgü
    /// işlemlere erişim sağlayan bir hizmet birimini temsil eder.
    /// </summary>
    /// <remarks>
    /// Bu arayüz, bir uygulama içerisindeki ilgili işlemlerin uyumlu şekilde yönetilmesini sağlayan
    /// birden fazla hizmet arayüzüne merkezi bir erişim noktası olarak görev yapar.
    /// Her bir özellik, ürünler, kategoriler, kullanıcılar, siparişler ve diğer alanlara yönelik işlevselliği
    /// kapsayan belirli bir hizmet arayüzünü açığa çıkarır.
    /// </remarks>
    public interface IServiceUnit
    {
        IProductService ProductService { get; }
        ICategoryService CategoryService { get; }
        IUserService UserService { get; }
        IOrderService OrderService { get; }
        ISellerService SellerService { get; }
        ICustomerService CustomerService { get; }
        ICartService CartService { get; }
        ICommentService CommentService { get; }
    }
}

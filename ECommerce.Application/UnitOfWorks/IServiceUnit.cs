using ECommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.UnitOfWorks
{
    public interface IServiceUnit
    {
        IProductService ProductService { get; }
        ICategoryService CategoryService { get; }
        IUserService UserService { get; }
        IOrderService OrderService { get; }
        ISellerService SellerService { get; }
        ICustomerService CustomerService { get; }
        ICartService CartService { get; }
    }
}

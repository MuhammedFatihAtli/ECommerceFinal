using ECommerce.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.UnitOfWorks
{
    public class ServiceUnit : IServiceUnit
    {
        private readonly ICustomerService _customerService;

        public ServiceUnit(ICategoryService categoryService, IUserService userService, IProductService productService, IOrderService orderService, ISellerService sellerService, ICartService cartService, ICustomerService customerService, ICommentService commentService)
        {
            CategoryService = categoryService;
            UserService = userService;
            ProductService = productService;
            this.OrderService = orderService;
            SellerService = sellerService;
            _customerService = customerService;
            CartService = cartService;
            CommentService = commentService;
        }
        public ICategoryService CategoryService { get; }
        public IUserService UserService { get;  }
        public IProductService ProductService { get; }
        public IOrderService OrderService { get; }
        public ISellerService SellerService { get; }
        public ICustomerService CustomerService => _customerService;
        public ICartService CartService { get; set; } 
        public ICommentService CommentService { get; set; }
    }
}

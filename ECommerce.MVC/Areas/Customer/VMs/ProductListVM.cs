namespace ECommerce.MVC.Areas.Customer.VMs
{
    // Ürünleri listelemek için kullanılan ViewModel
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}

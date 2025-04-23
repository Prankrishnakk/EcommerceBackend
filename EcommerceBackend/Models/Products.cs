namespace EcommerceBackend.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal offerPrize { get; set; }
        public decimal Rating { get; set; }
        public string ImageUrl { get; set; }
        public int StockId { get; set; }
        public int CategoryId { get; set; }


        public virtual Category _Category { get; set; }
        public virtual ICollection<CartItems> _CartItems { get; set; }




    }
}

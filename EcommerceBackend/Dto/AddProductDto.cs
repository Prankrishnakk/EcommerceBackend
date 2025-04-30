using System.ComponentModel.DataAnnotations;
namespace EcommerceBackend.Dto
{
    public class AddProductDto
    {
        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public decimal Rating { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        public decimal OfferPrize { get; set; }
        //public string ? CategoryName { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

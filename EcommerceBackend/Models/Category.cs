using System.ComponentModel.DataAnnotations;

namespace EcommerceBackend.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? CategoryName { get; set; }
        public virtual ICollection<Products> _Produts { get; set; }

    }
}

namespace EcommerceBackend.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public virtual User _User { get; set; }
        public virtual Products _Products { get; set; } 

 
    }
}

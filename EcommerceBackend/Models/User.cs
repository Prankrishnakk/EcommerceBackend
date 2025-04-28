namespace EcommerceBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
        public virtual Cart _Cart { get; set; }
        public virtual ICollection<WishList> _WishLists { get; set; }
        public virtual ICollection<UserAddress> _UserAddress { get; set; }
      
    }
}

namespace EcommerceBackend.Dto
{
    public class CartwithTotalPrice
    {
        public int TotalCartPrice { get; set; }
        public List<CartViewDto> c_items { get; set; }
    }
}

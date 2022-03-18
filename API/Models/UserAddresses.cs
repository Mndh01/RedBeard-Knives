namespace API.Models
{
    public class UserAddresses
    {
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
    }
}
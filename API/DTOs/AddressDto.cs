namespace API.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressParts { get; set; }
        public bool IsCurrent { get; set; }
    }
}
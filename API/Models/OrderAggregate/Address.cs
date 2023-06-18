namespace API.Models.OrderAggregate
{
      public class Address {
            public Address()
            {
            }

            public Address(string displayName, string street, string city,
            string state, string country, string zipCode)
            {
                  DisplayName = displayName;
                  Street = street;
                  City = city;
                  State = state;
                  Country = country;
                  ZipCode = zipCode;
                  FullAddress = country + state + city + street + zipCode;
            }
            public string DisplayName { get; set; }
            public string FullAddress { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string ZipCode { get; set; }
      }
}

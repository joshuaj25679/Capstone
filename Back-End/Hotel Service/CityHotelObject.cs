// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class CityHotelRoot
    {
        public List<CityHotelDatum> data { get; set; }
        public Meta meta { get; set; }
    }

    public class CityHotelDatum
    {
        public string chainCode { get; set; }
        public string iataCode { get; set; }
        public int dupeId { get; set; }
        public string name { get; set; }
        public string hotelId { get; set; }
        public GeoCode geoCode { get; set; }
        public Address address { get; set; }
    }

    public class GeoCode
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

        public class Address
    {
        public string countryCode { get; set; }
    }

    public class Meta
    {
        public int count { get; set; }
        public Links links { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }






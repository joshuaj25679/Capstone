// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //JSON object as a class
    public class Root
    {
        public List<Datum> data { get; set; }
        public Dictionaries dictionaries { get; set; }
    }

    public class Datum
    {
        public string type { get; set; }
        public Hotel hotel { get; set; }
        public bool available { get; set; }
        public List<Offer> offers { get; set; }
        public string self { get; set; }
    }

    public class Hotel
    {
        public string type { get; set; }
        public string hotelId { get; set; }
        public string chainCode { get; set; }
        public string dupeId { get; set; }
        public string name { get; set; }
        public string cityCode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Offer
    {
        public string id { get; set; }
        public string checkInDate { get; set; }
        public string checkOutDate { get; set; }
        public string rateCode { get; set; }
        public RateFamilyEstimated rateFamilyEstimated { get; set; }
        public Room room { get; set; }
        public Guests guests { get; set; }
        public Price price { get; set; }
        public Policies policies { get; set; }
        public string self { get; set; }
    }

    public class RateFamilyEstimated
    {
        public string code { get; set; }
        public string type { get; set; }
    }

    public class Room
    {
        public string type { get; set; }
        public TypeEstimated typeEstimated { get; set; }
        public Description description { get; set; }
    }

    public class TypeEstimated
    {
        public string category { get; set; }
    }

    public class Description
    {
        public string text { get; set; }
        public string lang { get; set; }
    }

    public class Guests
    {
        public int adults { get; set; }
    }

    public class Price
    {
        public string currency { get; set; }
        public string @base { get; set; }
        public string total { get; set; }
        public Variations variations { get; set; }
    }

    public class Variations
    {
        public Average average { get; set; }
        public List<Change> changes { get; set; }
    }

    public class Average
    {
        public string @base { get; set; }
    }

    public class Change
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string total { get; set; }
    }

    public class Policies
    {
        public string paymentType { get; set; }
        public Cancellation cancellation { get; set; }
    }

    public class Cancellation
    {
        public string amount { get; set; }
        public DateTime deadline { get; set; }
    }

    public class Dictionaries
    {
        public CurrencyConversionLookupRates currencyConversionLookupRates { get; set; }
    }

    public class CurrencyConversionLookupRates
    {
        public GBP GBP { get; set; }
    }

    public class GBP
    {
        public string rate { get; set; }
        public string target { get; set; }
        public int targetDecimalPlaces { get; set; }
    }

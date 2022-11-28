// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AirlineDatum
    {
        public string type { get; set; }
        public string iataCode { get; set; }
        public string icaoCode { get; set; }
        public string businessName { get; set; }
        public string commonName { get; set; }
    }

    public class AirlineLinks
    {
        public string self { get; set; }
    }

    public class AirlineMeta
    {
        public int count { get; set; }
        public Links links { get; set; }
    }

    public class AirlineRoot
    {
        public Meta meta { get; set; }
        public List<AirlineDatum> data { get; set; }
    }


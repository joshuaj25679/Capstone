// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class AirportAddress
{
    public string cityName { get; set; }
    public string cityCode { get; set; }
    public string countryName { get; set; }
    public string countryCode { get; set; }
    public string stateCode { get; set; }
    public string regionCode { get; set; }
}

public class AirportAnalytics
{
    public AirportTravelers travelers { get; set; }
}

public class AirportDatum
{
    public string type { get; set; }
    public string subType { get; set; }
    public string name { get; set; }
    public string detailedName { get; set; }
    public string id { get; set; }
    public Self self { get; set; }
    public string timeZoneOffset { get; set; }
    public string iataCode { get; set; }
    public AirportGeoCode geoCode { get; set; }
    public AirportAddress address { get; set; }
    public AirportAnalytics analytics { get; set; }
}

public class AirportGeoCode
{
    public double latitude { get; set; }
    public double longitude { get; set; }
}

public class AirportLinks
{
    public string self { get; set; }
}

public class AirportMeta
{
    public int count { get; set; }
    public Links links { get; set; }
}

public class AirportRoot
{
    public AirportMeta meta { get; set; }
    public List<AirportDatum> data { get; set; }
}

public class AirportSelf
{
    public string href { get; set; }
    public List<string> methods { get; set; }
}

public class AirportTravelers
{
    public int score { get; set; }
}


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class CityRoot
{
    public CityMeta meta { get; set; }
    public List<CityDatum> data { get; set; }
}

public class CityDatum
{
    public string type { get; set; }
    public string subType { get; set; }
    public string name { get; set; }
    public string detailedName { get; set; }
    public string id { get; set; }
    public Self self { get; set; }
    public string iataCode { get; set; }
    public Address address { get; set; }
}
public class Address
{
    public string cityName { get; set; }
    public string countryName { get; set; }
}

public class CityMeta
{
    public int count { get; set; }
    public CityLinks links { get; set; }
}

public class CityLinks
{
    public string self { get; set; }
}
public class Self
{
    public string href { get; set; }
    public List<string> methods { get; set; }
}


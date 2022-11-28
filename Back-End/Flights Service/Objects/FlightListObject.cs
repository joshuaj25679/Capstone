
public class FlightListObject
{
    public int ID {get;set;}
    public int BookableSeats {get;set;}
    public string TotalFlightDuration {get;set;}
    public double TotalCost {get;set;}
    public List<LayoverObject> AllFlights {get;set;} = new List<LayoverObject>();

    public FlightListObject(int id, int seats, string flightDuration, double cost)
    {
        ID = id;
        BookableSeats = seats;
        TotalFlightDuration = flightDuration.Substring(2);
        TotalCost = cost;
    }

    public void addToList(LayoverObject layoverFlight){
        AllFlights.Add(layoverFlight);
    }
}
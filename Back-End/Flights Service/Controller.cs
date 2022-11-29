using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using RestSharp.Authenticators;
using System.Text.Json;

namespace Controllers
{
    [ApiController]
    [Route("flights")]

    public class Controller : ControllerBase
    {

        [HttpGet]
        [Route("test")]
        public ActionResult<String> TestEndPoint()
        {
            return "Test Succesful";
        }

        [HttpGet]
        [Route("getFlights/{startCity}/{endCity}/{departureDate}/{isNonStop}")]
        public ActionResult<List<FlightListObject>> GetFlights(string startCity, string endCity, string departureDate, string isNonStop)
        {
            //Endpoint URL
            var client = new RestClient("https://test.api.amadeus.com/v2/shopping/flight-offers");

            var token = getAPIToken();

            //Add OAuth 2 Authentication Token.
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            //Get individual cities from calls here
            startCity = GetOneCity(startCity);
            endCity = GetOneCity(endCity);


            //Add Parameters to the API Request
            var request = new RestRequest()
            .AddParameter("originLocationCode", startCity)
            .AddParameter("destinationLocationCode", endCity)
            .AddParameter("departureDate", departureDate)
            .AddParameter("adults", 1)
            .AddParameter("nonStop", isNonStop)
            .AddParameter("currencyCode", "USD")
            .AddParameter("max", 21);

            //Send API Request and Get Response
            var response = client.Get(request);

            //Check if Response Content is null
            if (response.Content != null)
            {
                //Build Frame for API return
                return sortFlightData(response.Content);
            }
            else
            {
                return null;
            }
        }

        public string GetOneAirport(string city)
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations");

            var token = getAPIToken();

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            var request = new RestRequest()
            .AddParameter("subType", "AIRPORT")
            .AddParameter("keyword", city)
            .AddParameter("view", "LIGHT");

            var response = client.Get(request);

            if (response.Content != null)
            {
                //Convert Response Content to Objects
                AirportRoot flightDataDeserialized = JsonSerializer.Deserialize<AirportRoot>(response.Content);
                string returnData = flightDataDeserialized.data[0].name;
                return returnData;
            }
            else
            {
                return "Custom Error Message";
            }
        }

        public string GetOneCity(string city)
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations");

            var token = getAPIToken();

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            var request = new RestRequest()
            .AddParameter("subType", "CITY")
            .AddParameter("keyword", city)
            .AddParameter("view", "LIGHT");

            var response = client.Get(request);

            if (response.Content != null)
            {
                //Convert Response Content to Objects
                CityRoot flightDataDeserialized = JsonSerializer.Deserialize<CityRoot>(response.Content);
                string returnData = flightDataDeserialized.data[0].iataCode;
                return returnData;
            }
            else
            {
                return "Custom Error Message";
            }
        }

        public string GetAirline(string airlineCode)
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/airlines");

            var token = getAPIToken();

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            var request = new RestRequest()
            .AddParameter("airlineCodes", airlineCode);

            var response = client.Get(request);

            if (response.Content != null)
            {
                //Convert Response Content to Objects
                AirlineRoot flightDataDeserialized = JsonSerializer.Deserialize<AirlineRoot>(response.Content);
                string returnData = flightDataDeserialized.data[0].commonName;
                return returnData;
            }
            else
            {
                return "Custom Error Message";
            }
        }

        private string getAPIToken()
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/security/oauth2/token");
            var request = new RestRequest();
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", "ojClaW43C1GBhtQkbleppVJ5tVcABUCr");
            request.AddParameter("client_secret", "2oPn3E5vMk31begC");

            var response = client.Post(request);

            if (response.Content != null)
            {
                TokenRoot token = JsonSerializer.Deserialize<TokenRoot>(response.Content);
                return token.access_token;
            }
            else
            {
                return null;
            }
        }

        private List<FlightListObject> sortFlightData(string data)
        {
            List<FlightListObject> returnData = new List<FlightListObject>();
            //Convert Response Content to Objects
            Root flightDataDeserialized = JsonSerializer.Deserialize<Root>(data);

            //Loop over data adding necessary fields to return
            for (int i = 0; i < flightDataDeserialized.data.Count; i++)
            {
                //Adding Data to return
                FlightListObject tempData = new FlightListObject(
                    Int32.Parse(flightDataDeserialized.data[i].id),
                    flightDataDeserialized.data[i].numberOfBookableSeats,
                    flightDataDeserialized.data[i].itineraries[0].duration.ToString(),
                    Double.Parse(flightDataDeserialized.data[i].price.total)
                    );

                //Check if there are multiple flights per trip
                if (flightDataDeserialized.data[i].itineraries[0].segments.Count() > 1)
                {

                    //Loop over each flight to add data
                    for (int j = 0; j < flightDataDeserialized.data[i].itineraries[0].segments.Count(); j++)
                    {
                        LayoverObject tempLayover = new LayoverObject();
                        //Adding Data to return
                        tempLayover.FlightDuration = flightDataDeserialized.data[i].itineraries[0].segments[j].duration.Substring(2);
                        tempLayover.DepartureLocation = GetOneAirport(flightDataDeserialized.data[i].itineraries[0].segments[j].departure.iataCode);
                        tempLayover.DepartureTime = flightDataDeserialized.data[i].itineraries[0].segments[j].departure.at.ToString();
                        tempLayover.ArrivalLocation = GetOneAirport(flightDataDeserialized.data[i].itineraries[0].segments[j].arrival.iataCode);
                        tempLayover.ArrivalTime = flightDataDeserialized.data[i].itineraries[0].segments[j].arrival.at.ToString();
                        tempLayover.Airline = GetAirline(flightDataDeserialized.data[i].itineraries[0].segments[j].carrierCode);
                        tempLayover.FlightCode = Int32.Parse(flightDataDeserialized.data[i].itineraries[0].segments[j].number);
                        tempData.addToList(tempLayover);
                    }
                }
                else
                {
                    LayoverObject tempLayover = new LayoverObject();
                    //Adding Data to return
                    tempLayover.FlightDuration = flightDataDeserialized.data[i].itineraries[0].segments[0].duration.Substring(2);
                    tempLayover.DepartureLocation = GetOneAirport(flightDataDeserialized.data[i].itineraries[0].segments[0].departure.iataCode);
                    tempLayover.DepartureTime = flightDataDeserialized.data[i].itineraries[0].segments[0].departure.at.ToString();
                    tempLayover.ArrivalLocation = GetOneAirport(flightDataDeserialized.data[i].itineraries[0].segments[0].arrival.iataCode);
                    tempLayover.ArrivalTime = flightDataDeserialized.data[i].itineraries[0].segments[0].arrival.at.ToString();
                    tempLayover.Airline = GetAirline(flightDataDeserialized.data[i].itineraries[0].segments[0].carrierCode);
                    tempLayover.FlightCode = Int32.Parse(flightDataDeserialized.data[i].itineraries[0].segments[0].number);
                    tempData.addToList(tempLayover);
                }
                //Set return Data
                returnData.Add(tempData);
            }

            //Return Data
            return returnData;
        }
    }
}
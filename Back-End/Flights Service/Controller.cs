using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Text.Json;

namespace Controllers
{
    [ApiController]
    [Route("flights")]

    public class Controller : ControllerBase
    {
        private string token = "4JqaWqGhBNxoS44ujpGuCfxapJ06";

        [HttpGet]
        [Route("test")]
        public ActionResult<String> TestEndPoint()
        {
            return "Test Succesful";
        }

        [HttpGet]
        [Route("getFlights")]
        public ActionResult<Dictionary<string, Dictionary<string, string>>> GetFlights([FromBody] Dictionary<string, string> parameters)
        {
            //Endpoint URL
            var client = new RestClient("https://test.api.amadeus.com/v2/shopping/flight-offers");

            //Add OAuth 2 Authentication Token.
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            //Add Parameters to the API Request
            var request = new RestRequest()
            .AddParameter("originLocationCode", parameters["startCity"])
            .AddParameter("destinationLocationCode", parameters["endCity"])
            .AddParameter("departureDate", parameters["date"])
            .AddParameter("adults", 1)
            .AddParameter("currencyCode", "USD");

            //Send API Request and Get Response
            var response = client.Get(request);

            //Check if Response Content is null
            if (response.Content != null)
            {
                //Build Frame for API return
                Dictionary<string, Dictionary<string, string>> returnData = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> tempData = new Dictionary<string, string>();

                //Convert Response Content to Objects
                Root flightDataDeserialized = JsonSerializer.Deserialize<Root>(response.Content);

                //Loop over data adding necessary fields to return
                for (int i = 0; i < flightDataDeserialized.data.Count; i++)
                {
                    //Adding Data to return
                    tempData.Add("bookableSeats", flightDataDeserialized.data[i].numberOfBookableSeats.ToString());
                    tempData.Add("totalFlightDuration", flightDataDeserialized.data[i].itineraries[0].duration.ToString());
                    tempData.Add("totalCost", flightDataDeserialized.data[i].price.total.ToString());

                    //Check if there are multiple flights per trip
                    if (flightDataDeserialized.data[i].itineraries[0].segments.Count() > 1)
                    {
                        //Loop over each flight to add data
                        for (int j = 0; j < flightDataDeserialized.data[i].itineraries[0].segments.Count(); j++)
                        {
                            //Adding Data to return
                            tempData.Add("flightDuration" + j, flightDataDeserialized.data[i].itineraries[0].segments[j].duration.ToString());
                            tempData.Add("departureLocation" + j, flightDataDeserialized.data[i].itineraries[0].segments[j].departure.iataCode.ToString());
                            tempData.Add("departureTime" + j, flightDataDeserialized.data[i].itineraries[0].segments[j].departure.at.ToString());
                            tempData.Add("arrivalLocation" + j, flightDataDeserialized.data[i].itineraries[0].segments[j].arrival.iataCode.ToString());
                            tempData.Add("arrivalTime" + j, flightDataDeserialized.data[i].itineraries[0].segments[j].arrival.at.ToString());
                            tempData.Add("airline" + j, flightDataDeserialized.data[i].itineraries[0].segments[j].carrierCode.ToString());
                            tempData.Add("flightCode" + j, flightDataDeserialized.data[i].itineraries[0].segments[j].number.ToString());
                        }
                    }
                    else
                    {
                        //Adding Data to return
                        tempData.Add("flightDuration", flightDataDeserialized.data[i].itineraries[0].segments[0].duration.ToString());
                        tempData.Add("departureLocation", flightDataDeserialized.data[i].itineraries[0].segments[0].departure.iataCode.ToString());
                        tempData.Add("departureTime", flightDataDeserialized.data[i].itineraries[0].segments[0].departure.at.ToString());
                        tempData.Add("arrivalLocation", flightDataDeserialized.data[i].itineraries[0].segments[0].arrival.iataCode.ToString());
                        tempData.Add("arrivalTime", flightDataDeserialized.data[i].itineraries[0].segments[0].arrival.at.ToString());
                        tempData.Add("airline", flightDataDeserialized.data[i].itineraries[0].segments[0].carrierCode.ToString());
                        tempData.Add("flightCode", flightDataDeserialized.data[i].itineraries[0].segments[0].number.ToString());
                    }
                    //Set return Data
                    returnData.Add(i.ToString(), tempData);

                    //Reset temporary dictionary of values
                    tempData = new Dictionary<string, string>();
                }

                //Return Data
                return returnData;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getOneCity")]
        public ActionResult<String> GetOneCity( [FromBody] string city)
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations");

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
    }
}
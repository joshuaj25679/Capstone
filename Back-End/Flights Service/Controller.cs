using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators.OAuth2;

namespace Controllers
{
    [ApiController]
    [Route("flights")]
    
    public class Controller : ControllerBase
    {
        private string token = "1ORVnc76t9Ps4OHfwgIcPs6fOv0C";

        [HttpGet]
        [Route("test")]
        public ActionResult<String> TestEndPoint(){
            return "Test Succesful";
        }

        [HttpGet]
        [Route("getFlights/{startCity}/{endCity}/{date}")]
        public ActionResult<String> GetFlights(string startCity, string endCity, string date){
            var client = new RestClient("https://test.api.amadeus.com/v2/shopping/flight-offers");

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );
            
            var request = new RestRequest()
            .AddParameter("originLocationCode", startCity)
            .AddParameter("destinationLocationCode", endCity)
            .AddParameter("departureDate", date)
            .AddParameter("adults", 1)
            .AddParameter("currencyCode","USD");

            var response = client.Get(request);

            if(response.Content != null){
                return response.Content;
            }
            else{
                return "Custom Error Message";
            }
        }

        [HttpGet]
        [Route("getOneCity/{city}")]
        public ActionResult<String> GetOneCity(string city){
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations");
            
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );
            
            var request = new RestRequest()
            .AddParameter("subType", "CITY")
            .AddParameter("keyword", city)
            .AddParameter("view", "LIGHT");

            var response = client.Get(request);
            
            if(response.Content != null){
                return response.Content;
            }
            else{
                return "Custom Error Message";
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators.OAuth2;

namespace Controllers
{
    [ApiController]
    [Route("hotels")]
    
    public class Controller : ControllerBase
    {
        private string token = "tYow6aeIC9hbHKOnAMQYDxtjxbJB";

        [HttpGet]
        [Route("test")]
        public ActionResult<String> TestEndPoint(){
            return "Test Succesful";
        }

        [HttpGet]
        [Route("getHotelByCity/{city}")]
        public ActionResult<String> GetHotelsByCity(string city){
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations/hotels/by-city");
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );
            
            var request = new RestRequest()
            .AddParameter("cityCode", city);

            var response = client.Get(request);

            if(response.Content != null){
                return response.Content;
            }
            else{
                return "Custom Error Message";
            }
        }

        [HttpGet]
        [Route("getHotelOffers/{city}")]
        public ActionResult<String> GetHotelOffers(string city){
            var client = new RestClient("https://test.api.amadeus.com/v3/shopping/hotel-offers");
            
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );
            
            var request = new RestRequest()
            .AddParameter("hotelIds", "[" + city + "]")
            .AddParameter("adults", "1");

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
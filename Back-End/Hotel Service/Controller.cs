using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Text.Json;

namespace Controllers
{
    [ApiController]
    [Route("hotels")]

    public class Controller : ControllerBase
    {
        private string token = "i22enmlIAhkOt3PlmZrQH3C88EMJ";

        [HttpGet]
        [Route("test")]
        public ActionResult<String> TestEndPoint()
        {
            return "Test Succesful";
        }

        [HttpGet]
        [Route("getHotelByCity/{city}")]
        public ActionResult<String> GetHotelsByCity(string city)
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations/hotels/by-city");
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            var request = new RestRequest()
            .AddParameter("cityCode", city);

            var response = client.Get(request);

            System.Console.WriteLine(response.Content);

            if (response.Content != null)
            {
                return response.Content;
            }
            else
            {
                return "Custom Error Message";
            }
        }

        [HttpGet]
        [Route("getHotelOffers/{city}")]
        public ActionResult<Dictionary<string, string>> GetHotelOffers(string city)
        {
            var client = new RestClient("https://test.api.amadeus.com/v3/shopping/hotel-offers");

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            var request = new RestRequest()
            .AddParameter("hotelIds", "[" + city + "]")
            .AddParameter("adults", "1")
            .AddParameter("currency", "USD");

            var response = client.Get(request);

            if (response.Content != null)
            {
                Dictionary<string, string> returnData = new Dictionary<string, string>();
                Root hotelDataDeserialized = JsonSerializer.Deserialize<Root>(response.Content);
                returnData.Add("hotelName", hotelDataDeserialized.data[0].hotel.name);
                returnData.Add("checkInDate", hotelDataDeserialized.data[0].offers[0].checkInDate);
                returnData.Add("checkOutDate", hotelDataDeserialized.data[0].offers[0].checkOutDate);
                returnData.Add("price", hotelDataDeserialized.data[0].offers[0].price.total);
                returnData.Add("guests", hotelDataDeserialized.data[0].offers[0].guests.adults.ToString());

                return returnData;
            }
            else
            {
                return null;
            }
        }
    }
}
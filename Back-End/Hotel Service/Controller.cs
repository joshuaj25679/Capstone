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
        [HttpGet]
        [Route("test")]
        public ActionResult<String> TestEndPoint()
        {
            return "Test Succesful";
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [Route("getHotelOffers/{city}/{startDate}/{endDate}")]
        public ActionResult<List<HotelObject>> GetHotelOffers(string city, string startDate, string endDate)
        {
            var client = new RestClient("https://test.api.amadeus.com/v3/shopping/hotel-offers");

            var token = getAPIToken();

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );
            
            
            List<string> hotelsInCity = GetHotelsByCity(city);


            //Build Request with Parameters
            var request = new RestRequest()
            .AddParameter("hotelIds", "[" + getHotelIds(hotelsInCity) + "]")
            .AddParameter("adults", "1")
            .AddParameter("checkInDate", startDate)
            .AddParameter("checkOutDate", endDate)
            .AddParameter("currency", "USD");

            //Get Response from API Call
            var response = client.Get(request);

            //Clean up data and return it IF the response exists
            if (response.Content != null)
            {
                System.Console.WriteLine("Response Received");
                //Setup List to return
                List<HotelObject> returnList = new List<HotelObject>();

                //Turn Response into C# Class Objects for easier accesability
                Root hotelDataDeserialized = JsonSerializer.Deserialize<Root>(response.Content);

                System.Console.WriteLine("Processing Hotel Data to Custom object");
                //Loop over list of data objects to make into a single custom objecct fo the list
                for (int i = 0; i < hotelDataDeserialized.data.Count(); i++)
                {
                    //Create custom object with needed data
                    returnList.Add(new HotelObject(
                                        i+1,
                                        hotelDataDeserialized.data[i].hotel.name,
                                        hotelDataDeserialized.data[i].offers[0].checkInDate,
                                        hotelDataDeserialized.data[i].offers[0].checkOutDate,
                                        hotelDataDeserialized.data[i].offers[0].price.total,
                                        hotelDataDeserialized.data[i].offers[0].room.description.text
                                        ));

                }
                return returnList;
            }
            else
            {
                return null;
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Get OAuth2.0 Token to access Amadeus API
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Helper Method to get all Hotels in a city 
        private List<string> GetHotelsByCity(string city)
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations/hotels/by-city");

            var token = getAPIToken();

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );
            city = GetOneCity(city);

            var request = new RestRequest()
            .AddParameter("cityCode", city);

            var response = client.Get(request);

            if (response.Content != null)
            {
                //System.Console.WriteLine(response.Content.ToString());
                List<string> hotelsInCity = new List<string>();
                CityHotelRoot tempData = JsonSerializer.Deserialize<CityHotelRoot>(response.Content);

                for (int i = 0; i < tempData.data.Count(); i++)
                {
                    hotelsInCity.Add(tempData.data[i].hotelId);
                }
                return hotelsInCity;
            }
            else
            {
                return null;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Get a general city name to a city code for use.
        public string GetOneCity(string city)
        {
            var client = new RestClient("https://test.api.amadeus.com/v1/reference-data/locations");

            var token = getAPIToken();

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                token, "Bearer"
            );

            var request = new RestRequest()
            .AddParameter("subType", "CITY,AIRPORT")
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Format List of Hotels to fit
        private string getHotelIds(List<string> idList)
        {

            string returnData = "";
            int counter = 0;
            foreach(string id in idList)
            {
                
                if(counter <= 100){
                    returnData = "" + returnData + id + ", ";
                    counter ++;
                }
                else{
                    break;
                }
                
            }
            return returnData.Remove(returnData.Length - 2);
        }
    }

}
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using System;
using RestSharp;
using RestSharp.Authenticators;

//"docker_db1": "Server=localhost,1433;database=apidb;User ID= SA;password=abc123!!@;"

namespace Controllers
{
    [ApiController]
    [Route("flights")]
    
    public class Controller : ControllerBase
    {

        [HttpGet]
        [Route("test")]
        public ActionResult<String> TestEndPoint(){
            return "Test Succesful";
        }

        [HttpGet]
        [Route("getFlights")]
        public ActionResult<String> GetFlights(){
            
            return "Test Succesful";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PokerServer.Models;
using ConsoleApplication2;
using Newtonsoft.Json;
using System.Web;
using System.Diagnostics;

namespace PokerServer.Controllers
{
    public class PokerController : ApiController
    {

        PokerModel pokerModel = myModel.getModel();
        // GET api/poker
        public HttpResponseMessage Get()
        {
            Newtonsoft.Json.Linq.JObject state = pokerModel.getState();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, state);
            return response;
        }

        // GET api/poker/5
        //TODO obfuscate player cards from the pay load
        public void Get(int id, int id1)
        {
            //This should return the JSON string representing the current state of the game
            //Newtonsoft.Json.Linq.JObject state = pokerModel.getState();
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, state);
            //return response;
            Debug.WriteLine(id + "  " + id1);
        }

        // POST api/poker
        public void Post(HttpRequestMessage m)
        {
            //TODO add verification for user id and password
            string username = HttpUtility.ParseQueryString(m.RequestUri.Query).Get("username");
            string password = HttpUtility.ParseQueryString(m.RequestUri.Query).Get("password");
            //TODO add verification for user id and password
            
            var content = Request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            ClientMessage message = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientMessage>(jsonContent);
            
            Debug.WriteLine(message.bet + " FUCK " + message.fold);
            pokerModel.ProcessMessage(message);
        }

        public static class myModel
        {
            public static PokerModel myPokerModel = new PokerModel();
            public static PokerModel getModel(){
                return myPokerModel;
            }
        }
    }
}

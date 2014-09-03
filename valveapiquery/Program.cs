using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Data;
//using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;


namespace valveapiquery
{
    class Program
    {
        public static List<int> playerids = new List<int>()
        {
            67601693, 86715129, 109778511, 115975133, 67760037, 40547474, 30237211, 86726887, 88719902, 5448108, 85805514, 21604967, 87276347, 86745912, 121847953, 118073569, 132291754

        };
        public static List<long> matchList = new List<long>();

        public static List<MatchPlayer> playerList = new List<MatchPlayer>();

        static void Main(string[] args)
        {
            
            Stopwatch runTime = Stopwatch.StartNew();

            RunAsync().Wait();

            MatchPlayer.buildItem(playerList);

            runTime.Stop();
            Console.WriteLine(string.Format("elapsed time : {0} ", (runTime.ElapsedTicks / (double)Stopwatch.Frequency)));

            foreach(var player in playerList){

                Console.WriteLine(player.itemhash);

            }

            var results = from p in playerList
                          group p.hero_id by p.itemhash into g
                          select new { ItemHash = g.Key, Heroes = g.ToList() };


            foreach (var res in results)
            {

                if (res.Heroes.Count > 1)
                {

                    Console.WriteLine("number of w/e : " + res.ItemHash + "  " + res.Heroes.Count);

                }
            }


            Console.ReadKey();





        }


        static async Task GetPlayerMatches(int playerid, HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync(string.Format("https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=65C5ADADF141DB0495C3FBBCA6D65689&matches_requested=1&account_id={0}", playerid));

            if (response.IsSuccessStatusCode)
            {

                //content = result.Content.ReadAsAsync<JObject>().Result;

                // JObject content = await response.Content.ReadAsAsync<JObject>();
                JObject content = response.Content.ReadAsAsync<JObject>().Result;


                JObject resultobject = content.Value<JObject>("result");


                if (resultobject.Value<int>("status") != 1)
                {
                    Debug.WriteLine("player id " + playerid + " does not have public api info available");

                }
                else
                {
                    var matchlist = resultobject.Value<JArray>("matches").ToList();

                    foreach (var match in matchlist)
                    {

                        var newmatch = match.Value<long>("match_id");
                        matchList.Add(newmatch);
                        Console.WriteLine(string.Format("playerid: {0}  matchid: {1} ", playerid, newmatch));

                    }




                }



            }

        }

        static async Task GetMatchDetails(long matchid, HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync(string.Format("https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?key=65C5ADADF141DB0495C3FBBCA6D65689&match_id={0}", matchid));

            if (response.IsSuccessStatusCode)
            {

                //content = result.Content.ReadAsAsync<JObject>().Result;

                // JObject content = await response.Content.ReadAsAsync<JObject>();
                JObject content = response.Content.ReadAsAsync<JObject>().Result;

                

                JObject resultobject = content.Value<JObject>("result");


                JArray playerlist = resultobject.Value<JArray>("players");

                //Console.WriteLine(playerlist.ToString());


                var trry = JsonConvert.DeserializeObject<List<MatchPlayer>>(playerlist.ToString()).ToList();

                //var playerlist = resultobject.Value<IEnumerable<MatchPlayer>>("players");


                playerList.AddRange(trry);
               

              




                



            }

        }


        static async Task RunAsync()
        {
            List<Task> Tasks = new List<Task>();

            using (var client = new HttpClient())
            {
              
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                foreach (var playerid in playerids)
                {
                    //Tasks.Add(Task.Run(() => GetPlayerMatches(playerid,client)));

                    Tasks.Add(GetPlayerMatches(playerid, client));

                    // HTTP GET
                    //HttpResponseMessage response = await client.GetAsync(string.Format("https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=65C5ADADF141DB0495C3FBBCA6D65689&matches_requested=2&account_id={0}", playerid));
                    //HttpResponseMessage response = await client.GetAsync(string.Format("https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=65C5ADADF141DB0495C3FBBCA6D65689&matches_requested=2&account_id={0}", playerid));
                    
                    


                }

                await Task.WhenAll(Tasks);

                Tasks = new List<Task>();


                foreach (var matchid in matchList)
                {

                    Tasks.Add(GetMatchDetails(matchid, client));

                }

                await Task.WhenAll(Tasks);




            }
        }

       



    }
}

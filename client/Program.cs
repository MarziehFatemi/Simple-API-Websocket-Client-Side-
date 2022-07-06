using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace client
{

    public class Response
    {
        string msg_type;
        public  tick Tick = new tick();
        echo_req echo_Req = new echo_req();
        subscription subscriptionId = new subscription(); 
       public class tick 
        {
            public int epoch;
            string ask;
            string bid;
            string id;
            int pip_size;
            string quote;
            string symbol;
        }

       public  class echo_req
        {
            string subscribe;
            string ticks;
        }
         
       public class subscription
        { string id; }
               

}

internal class Program
    {
        //  public static StreamWriter writer = new StreamWriter(@"E:\C#\Pojects\test api\testapi2\resut.txt");


        static int Cnt = 0; 
        static void Main(string[] args)
        {
            Cnt = 0; 
            using (WebSocket ws = new WebSocket("wss://ws.binaryws.com/websockets/v3?app_id=1089"))
            
            {
                
                Console.WriteLine(" The real-time tick stream includes"); 
                ws.OnMessage += Ws_OnMessage;

                ws.Connect();

                JObject Command = new JObject();
                Command.Add("ticks", "R_50");
                Command.Add("subscribe", 1);
                string jsonString = JsonConvert.SerializeObject(Command);
                ws.Send(jsonString);
                Console.ReadKey(); 
                
            }

        }


        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {

            Response response = JsonConvert.DeserializeObject<Response>(e.Data); 
            
            //Console.WriteLine("The current epoch: " + response.Tick.epoch);
            
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(response.Tick.epoch);
            DateTime dateTime = dateTimeOffset.DateTime;


            
           Console.WriteLine("The original time: " + dateTime.ToString());
           Console.WriteLine("Time after 1 minute delay: " + dateTime.AddMinutes(1).ToString());
           Console.WriteLine("");
           Console.WriteLine("");
            Cnt++; 

            
            using (StreamWriter writer = File.AppendText(Directory.GetCurrentDirectory() + @"TestResut.txt"))
            {
                if (Cnt == 1)
                {
                    writer.WriteLine("The Answer to your request at: " + DateTime.Now.ToString() + " is: ");
                    writer.WriteLine("*******************************");
                    writer.WriteLine("*******************************");
                    writer.WriteLine(""); 
                }



                    writer.WriteLine("The original time: " + dateTime.ToString());
                writer.WriteLine("Time after 1 minute delay: " + dateTime.AddMinutes(1).ToString());
                writer.WriteLine("");
                writer.WriteLine("");
            }
            


            }


    }
}

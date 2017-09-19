using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        static Socket sck;
        static void Main(string[] args)
        {

            Move move = new Move{
                src = 4,
                dest = 5, 
                UID = 0
            };
            
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            

            try
            {
                sck.Connect(localEndPoint);
            }
            catch
            {
                Console.Write("Unable to connect to remote endpoint\r\n");
                Main(args);
            }

            // Get UID
            byte[] UID = new byte[256];
            sck.Receive(UID);
            //Console.WriteLine("UID:" + Encoding.UTF8.GetString(UID));
            // Write Move class that hold UID. 
            Console.WriteLine(Encoding.UTF8.GetString(UID));

            // Parse Json
            dynamic MoveJson = JObject.Parse(Encoding.UTF8.GetString(UID));

            Console.WriteLine(MoveJson.UID);
            // Set UID in move. 
            move.UID = MoveJson.UID;

            // Serialize Move 
            String output = JsonConvert.SerializeObject(move);
            output = output + "\0";
            Console.WriteLine(output);

            byte[] buffer = new byte[256];
            
            while(true){
                 // Console.Write("Enter text: ");
                 // String txt = Console.ReadLine();
                //sck.Receive(buffer);
                //Console.WriteLine(Encoding.UTF8.GetString(buffer));

                Byte[] data = Encoding.ASCII.GetBytes(output);
            
                Console.Write("Press any key to Send Data");
                Console.Read();
                sck.Send(data);
                Console.Write("Data Sent\r\n");

                sck.Receive(buffer);


                Console.WriteLine(Encoding.UTF8.GetString(buffer));
                Console.Write("Press any key to continue");
                Console.Read();
            }
        }

    }

    class Move
    {
        public int src {get; set;}
        public int dest {get; set;}

        public int UID {get; set;}
    }

}


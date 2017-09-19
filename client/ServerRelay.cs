using  System.Text;
using System.Net.Sockets;
using System.Net; 
using System;

namespace ServerRelay{

    public class ServerRelay{
        
        // Properties
        static Socket socket; 
        static int UID; 
        static bool IsNorth;
        static byte[] buffer = new byte[1080];
     
        // Constructer 
        public ServerRelay(){}
    

        public static void ConnectToServer(){
            // Setting up socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);

            // Try to connect
            try {
                socket.Connect(localEndPoint);
            }
            catch {
                Console.Write("Unable to connect to remote endpoint\r\n");
                Main();
            }

            // UID will need to be send on every call, This makes it useful to have a global var. 
            SetUID(); 

            

            // The game is being played 
            ConnectionLoop(); 
            
        }

        private static void ConnectionLoop(){
            while(true){

                // TODO see if user is first or second player
                // This first player will be North. second player south. 
                // Second player will send OK command first and then wait to recieve
                // First player will send move data first. 
            }
        }

        private void SetIsNorth(){
           socket.Send(Encoding.ASCII.GetBytes("GameStart\0"));  
           socket.Receive(buffer);
           Console.WriteLine(Encoding.UTF8.GetString(buffer)); 
        } 
    
        // Get UID from the first recieve from the server and set golbal var. 
        private static void SetUID(){
            socket.Receive(buffer);
            UID = Convert.ToInt32(Encoding.UTF8.GetString(buffer));
            Console.WriteLine(UID);
        }
        
        public static int  Main(){
            ConnectToServer(); 
            return 0; 
        }     
    }
}

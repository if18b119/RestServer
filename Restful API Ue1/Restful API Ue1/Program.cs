using System;
using RestServerLib;
namespace Restful_API_Ue1
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.Write("Please enter a Port: ");
            int port_eingabe = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Starting Server on Port " + port_eingabe);

            MyServer server = new MyServer(port_eingabe);
            server.StartListening();
        }
    }
}

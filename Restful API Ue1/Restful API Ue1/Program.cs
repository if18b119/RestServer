using System;
using RestServerLib;
namespace Restful_API_Ue1
{
    class Program
    {   
       
        static void Main(string[] args)
        {
            int port = -1;
           
           while (true)
            {
                try
                {
                    Console.Write("Please enter a Port: ");
                    string input = Console.ReadLine();
                    if(!int.TryParse(input, out port))
                    {
                        throw new Exception("Error: Input not a number!");
                    }
                    else
                    {
                        port = Convert.ToInt32(input);
                        if(port<1024 || port> 49151)
                        {
                            throw new Exception("Error: Port must be between 1024 - 49151!");
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
            
            Console.WriteLine("Starting Server on Port " + port);

            MyServer server = new MyServer(port);
            server.StartListening();
        }
    }
}

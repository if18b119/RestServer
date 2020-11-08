using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace RestServerLib
{
    public class MyServer
    {
        public const String TYPE = "HTTP/1.1";
        private int port;
        public string name;
        private bool is_connected = false;
        private TcpListener listener;

        //Konstruktor, hier wird Der Port gesetzt und ein TcpListiner Objekt erstellt mit einer beliebigen IPAdresse
        //und einem Port
        public MyServer(int port_)
        {
            this.port = port_;
            this.name = "localhost:" + port_;
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void StartListening()
        {
            try
            {
                is_connected = true;
                listener.Start(); //Server starts listetning for requests
                while (is_connected)
                {
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = listener.AcceptTcpClient(); // Server akzeptiert einen Rewuest/Client und blockt
                    Console.WriteLine("Connected successfully!");

                    ClientHandler(client);

                    //Diconecting with the CLient
                    //client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                is_connected = false;
                listener.Stop();
            }

        }

        public void ClientHandler(TcpClient client)
        {
            StreamReader stream = new StreamReader(client.GetStream()); // Stream mit den Request/CLient infos
            String msg = ""; // Die Infos werden hier gespeichert
            //StreamWrite, Flush -> senden, despose -> schließen

            if(stream==null)
            {
                throw new NullReferenceException("NULL VALUE!");
            }

            while (stream.Peek() != -1) //Peek returned den nächsten Charakter oder -1 wenn keine mehr vorhanden sind
            {
                msg += stream.ReadLine() + "\n"; //Hier wird der Stream in dem string gespeichert
            }

            RequestKontext req = RequestKontext.GetRequest(msg);
            ServerReply reply = ServerReply.HandlingRequest(req);
            using (StreamWriter writer = new StreamWriter(client.GetStream())) //using für Idisposable
            {

                writer.Write($"{reply.Protocoll} {reply.Status}\r\n");
                writer.Write($"Content-Type: {reply.ContentType}\r\n");
                writer.Write($"Content-Length: {reply.Data.Length}\r\n");
                writer.Write("\r\n");
                writer.Write($"{reply.Data}\r\n");
                writer.Write("\r\n\r\n");


            }
        }
    }
}

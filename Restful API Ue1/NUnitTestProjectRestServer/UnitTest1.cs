using NUnit.Framework;
using RestServerLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Tests
{
    public class Tests
    {
        public RequestKontext req { get; set; }
        public String Pfad { get; set; } = "C:\\Users\\titto\\Desktop\\3.Semester\\Software Engineering\\RestServer\\Restful API Ue1\\Messages\\";

        [SetUp]
        public void Setup()
        {
            req = new RequestKontext("GET", "/messages", "HTTP/1.1", "");
            
        }

        public ServerReply SetServerReply()
        {
            return ServerReply.HandlingRequest(req);
        }
        
        public bool CheckValues(ServerReply sr,string status, string protocol, string data, string ctype)
        {
            Console.WriteLine("Data \n" + status + "\n" + protocol +"\n" + data + "\n" + ctype +"\n" + "Length: " + (status.Length+protocol.Length+data.Length+ctype.Length));
            Console.WriteLine("ServerReply \n" + sr.Status+"\n" + sr.Protocoll+"\n"+sr.Data+"\n"+sr.ContentType +"\n"+ "Length: " + (sr.Status.Length + sr.Protocoll.Length + sr.Data.Length + sr.ContentType.Length));
            if(sr.Status==status && sr.Protocoll==protocol && sr.Data == data && sr.ContentType==ctype)
            {
               return true;
            }
           else
            {
                return false;
            }
        }

        public void CreateMessage()
        {
            
            File.WriteAllText(this.Pfad + "Message.txt", "Hello\n");
          
        }

        public void DeleteMessage()
        {
            File.Delete(this.Pfad+"Message.txt");
        }

        public void DeleteMessage(string filename)
        {
            File.Delete(this.Pfad + filename);
        }

      
      /// //////////////////////////////
     
        //-------------------SEND-------------------//
        [Test]
        public void SendMessage()
        {
            req.Type = "POST";
            req.Body = "Hello\n";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "200 OK", "HTTP/1.1", "1", "text"), true);
            DeleteMessage("Message 1.txt");
        }

        [Test]
        public void SendMessageBrokenRequest()
        {
            req.Type = "POST";
            req.Options = "/messages";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "400 Bad Request", "HTTP/1.1", "", "text"), true);
            
        }

        [Test]
        public void SendMessageBrokenRequest2()
        {
            req.Type = "POST";
            req.Options = "/messages/2";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "400 Bad Request", "HTTP/1.1", "", "text"), true);
        }
        //-------------------LIST-------------------//
        [Test]
        public void GetAllMessagesSucc()
        {
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "200 OK", "HTTP/1.1", "", "text"), true);
        }

        [Test]
        public void ListOneMessage()
        {
            CreateMessage();
            req.Type = "GET";
            req.Options = "/messages/1";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "200 OK", "HTTP/1.1", "Name: Message.txt\nNachricht:\nHello\n", "text"), true);
            DeleteMessage();
        }

        [Test]
        public void ListOneMessageNotExisting()
        {
            req.Type = "GET";
            req.Options = "/messages/99";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "416 Range Not Satisfiable", "HTTP/1.1", "", "text"), true);
            DeleteMessage();
        }

        [Test]
        public void ListMessageBrokenRequest()
        {
            req.Type = "GET";
            req.Options = "/messages/";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "400 Bad Request", "HTTP/1.1", "", "text"), true);
            
        }
        //-------------------Delete-------------------//
        [Test]
        public void DeleteMsgSucc()
        {
            CreateMessage();
            req.Type = "DELETE";
            req.Options = "/messages/1";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "200 OK", "HTTP/1.1", "", "text"), true);
        }

        [Test]
        public void DeleteMsgNotExisiting()
        {
            req.Type = "DELETE";
            req.Options = "/messages/99";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "416 Range Not Satisfiable", "HTTP/1.1", "", "text"), true);
        }

        [Test]
        public void DeleteBrokenRequest()
        {
            req.Type = "DELETE";
            req.Options = "/messages";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "400 Bad Request", "HTTP/1.1", "", "text"), true);
        }
        //-------------------PUT-------------------//
        [Test]
        public void PUTMessage()
        {
            CreateMessage();
            req.Type = "PUT";
            req.Options = "/messages/1";
            req.Body = "Hey2";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "200 OK", "HTTP/1.1", "", "text"), true);
            DeleteMessage();
        }
        [Test]
        public void PUTBrokenRequest()
        {
            req.Type = "PUT";
            req.Options = "/messages";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "400 Bad Request", "HTTP/1.1", "", "text"), true);
        }

        [Test]
        public void PUTMessageNotExisting()
        {
            req.Type = "PUT";
            req.Options = "/messages/99";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "416 Range Not Satisfiable", "HTTP/1.1", "", "text"), true);
        }
        /////////////////////////
        [Test]
        public void NotSupportedRequest()
        {
            req.Type = "PATCH";
            req.Options = "/messages/99";
            req.Body = "";
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr, "405 Method Not Allowed", "HTTP/1.1", "", "text"), true);
        }


    }
}
using NUnit.Framework;
using RestServerLib;

namespace Tests
{
    public class Tests
    {
        public RequestKontext req { get; set; }
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
            if(sr.Status==status && sr.Protocoll==protocol && sr.Data == data && sr.ContentType==ctype)
            {
               return true;
            }
           else
            {
                return false;
            }
        }

        [Test]
        public void GetMessagesSucc()
        { 
            ServerReply sr = SetServerReply();
            Assert.AreEqual(CheckValues(sr,"200 OK","HTTP/1.1","","text"),true);
          
        }
       
    }
}
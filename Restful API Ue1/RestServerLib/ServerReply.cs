using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace RestServerLib
{
    public class ServerReply
    {
        public String Protocoll { get; set; }
        public String Status { get; set; }
        public String Data { get; set; }
        public String ContentType { get; set; }

        private static String pfad;
        




        public ServerReply(string protocoll, string status, string data, string content_type)
        {
            
            Protocoll = protocoll;
            Status = status;
            Data = data;
            ContentType = content_type;

        }

        public static ServerReply HandlingRequest(RequestKontext req)
        {
            pfad = "C:\\Users\\titto\\Desktop\\3.Semester\\Software Engineering\\RestServer\\Restful API Ue1\\Messages\\";
            if (req == null)
            {
                return BadRequest(req);
            }
            Console.WriteLine($"Type: {req.Type}");
            Console.WriteLine($"Request: {req.Options}");
            Console.WriteLine($"Protocoll: {req.Protocol}");

            foreach (HeaderInfo tmp in RequestKontext.HeaderInformation)
            {
                Console.WriteLine($"{tmp.key}:{tmp.value}");
            }

            if (req.Type == "GET")
            {
                return GET(req);
            }

            else if (req.Type == "POST")
            {
                return POST(req);

            }
            else if (req.Type == "DELETE")
            {
                return DELETE(req);
            }
            else if (req.Type == "PUT")
            {
                return PUT(req);
            }
            else
            {
                return new ServerReply(req.Protocol, "405 Method Not Allowed", "", "text"); ;
            }
        }

        public static ServerReply GET(RequestKontext req)
        {   
            if(req.Options.Contains("/")==false)
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }
            string[] frag = req.Options.Split('/');
            if (frag[1] == "messages" && frag.Length == 2)
            {
                string name;
                string[] messages = Directory.GetFiles(pfad); //geting all messages (with full path)
                //Converting String array into String
                StringBuilder tmp = new StringBuilder();
                foreach (string value in messages)
                {
                   
                    name = value.Replace(pfad,"");
                    tmp.Append("Name: ");
                    tmp.Append(name);
                    tmp.Append("\n");
                    tmp.Append("Nachricht:\n");
                    //getting the message out of the txt file
                    using (var streamReader = new StreamReader(value, Encoding.UTF8))
                    {
                        tmp.Append(streamReader.ReadToEnd());
                    }
                    tmp.Append("\n\n");

                }
                string _return = tmp.ToString();
                return new ServerReply(req.Protocol, "200 OK", _return, "text");
            }
            else if (frag[1] == "messages" && frag.Length == 3) // frag[0]="", frag[1]="messages", frag[3]=integer
            {
                if (Convert.ToInt32(frag[2]) <= Directory.GetFiles(pfad).Length && frag[2] != "" && Convert.ToInt32(frag[2]) > 0)//if the index doesn't pass the number of messages
                {
                    string file_on_index;
                    int index = Convert.ToInt32(frag[2]) - 1;
                    StringBuilder tmp = new StringBuilder();
                    file_on_index = Convert.ToString(Directory.GetFiles(pfad).GetValue(index));
                    tmp.Append("Name: ");
                    tmp.Append(file_on_index.Remove(0, pfad.Length));
                    tmp.Append("\n");
                    tmp.Append("Nachricht:\n");
                    using (var streamReader = new StreamReader(file_on_index, Encoding.UTF8))
                    {
                        tmp.Append(streamReader.ReadToEnd());
                    }
                    

                    string _return = Convert.ToString(tmp);

                    return new ServerReply(req.Protocol, "200 OK", _return, "text");
                }

                else
                {
                    return new ServerReply(req.Protocol, "416 Range Not Satisfiable", "", "text");
                }

            }
            else
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }
        }
        public static ServerReply POST(RequestKontext req)
        {
            if (req.Options.Contains("/") == false)
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }
            string[] frag = req.Options.Split('/');
            int num_of_files = Directory.GetFiles(pfad).Length;
            if (frag[1] == "messages" && frag.Length == 2)
            {
                if (req.Body == "")
                {
                    return new ServerReply(req.Protocol, "400 Bad Request", "", "text");
                }
                else
                {
                    string name = "Message " + Convert.ToString(num_of_files + 1) + ".txt";
                    using (File.Create(pfad + name))
                    {

                    }

                    File.WriteAllText(pfad + name, req.Body);
                    return new ServerReply(req.Protocol, "200 OK", Convert.ToString(num_of_files + 1), "text");
                }


            }
            else
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }
        }

        public static ServerReply PUT(RequestKontext req)
        {
            if (req.Options.Contains("/") == false)
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }
            string[] frag = req.Options.Split('/');
            if (frag[1] == "messages" && frag.Length == 3)
            {
                if (Convert.ToInt32(frag[2]) <= Directory.GetFiles(pfad).Length && Convert.ToInt32(frag[2]) > 0 && frag[2] != "")
                {
                    if (req.Body != "")
                    {
                        string file_on_index;
                        int index = Convert.ToInt32(frag[2]) - 1;
                        file_on_index = Convert.ToString(Directory.GetFiles(pfad).GetValue(index));
                        File.WriteAllText(file_on_index, req.Body);
                        return new ServerReply(req.Protocol, "200 OK", "", "text");
                    }

                    else
                    {
                        return new ServerReply(req.Protocol, "400 Bad Request", "", "text");
                    }

                }
                else
                {
                    return new ServerReply(req.Protocol, "416 Range Not Satisfiable", "", "text");
                }
            }
            else
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }
        }

        public static ServerReply DELETE(RequestKontext req)
        {
            if (req.Options.Contains("/") == false)
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }
            string[] frag = req.Options.Split('/');
            if (frag[1] == "messages" && frag.Length == 3)
            {
                if (Convert.ToInt32(frag[2]) <= Directory.GetFiles(pfad).Length && Convert.ToInt32(frag[2]) > 0 && frag[2] != "")//if the index doesn't pass the number of messages
                {
                    int index = Convert.ToInt32(frag[2]) - 1;
                    string file_on_index = Convert.ToString(Directory.GetFiles(pfad).GetValue(index));
                    File.Delete(file_on_index);
                    return new ServerReply(req.Protocol, "200 OK", "Deleted succesfully!", "text");
                }
                else
                {
                    return new ServerReply(req.Protocol, "416 Range Not Satisfiable", "", "text");
                }
            }
            else
            {
                return new ServerReply(req.Protocol, "404 Not Found", "", "text");
            }

        }

        public static ServerReply BadRequest(RequestKontext req)
        {
            return new ServerReply(req.Protocol, "400 Bad Request", "", "text");
        }
    }
}

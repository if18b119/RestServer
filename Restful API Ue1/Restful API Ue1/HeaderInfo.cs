﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Restful_API_Ue1
{
    public class HeaderInfo
    {
        public string key { get; private set; }
        public string value { get; private set; }
        public HeaderInfo(string x, string y)
        {
            key = x;
            value = y;
        }
    }
}

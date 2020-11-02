using System;
using System.Collections.Generic;
using System.Text;

namespace Restful_API_Ue1
{
    class HeaderInfo
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

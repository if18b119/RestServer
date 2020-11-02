using System;
using System.Collections.Generic;
using System.Text;

namespace Restful_API_Ue1
{
    class RequestKontext
    {
        public String Type { get; private set; }
        public String Options { get; private set; }
        public String Protocol { get; private set; }
        public String Body { get; private set; }

        public static List<HeaderInfo> HeaderInformation = new List<HeaderInfo>();

    }
}

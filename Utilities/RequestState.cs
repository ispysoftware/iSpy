using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace iSpyApplication.Utilities
{
    public class RequestState
    {
        // This class stores the request state of the request.
        public WebRequest Request;
        public WebResponse Response;
        public ConnectionOptions ConnectionOptions;

        public RequestState()
        {
            Request = null;
            Response = null;
            ConnectionOptions = null;
        }
    }
}

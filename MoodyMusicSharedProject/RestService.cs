using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MoodyMusic.Shared
{
    public class RestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DashScope
{
    internal class DefaultHttpClientProvider
    {
        internal static HttpClient CreateClient()
        {
            return new HttpClient();
        }
    }
}

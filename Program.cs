using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;


namespace ChatConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ChatRuns.RunAsync().GetAwaiter().GetResult();
        }
    }
}

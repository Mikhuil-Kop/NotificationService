using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCommonLibrary.Extensions
{
    public static class HttpExtension
    {
        public static string GetMessageString(this HttpClient client, HttpRequestMessage requestMessage)
        {
            var res = new StringBuilder();

            res.Append("Client.DefaultHeaders:\n");
            res.Append(client.DefaultRequestHeaders.Aggregate("", (x, y) => x + $"\t{y.Key} - {y.Value}\n"));

            res.Append("RequestMessage.Headers:\n");
            res.Append(requestMessage.Headers.Aggregate("", (x, y) => x + $"\t{y.Key} - {y.Value}\n"));

            res.Append("RequestMessage.Content:\n");
            res.Append($"\t{requestMessage.Content}");

            return res.ToString();
        }
    }
}

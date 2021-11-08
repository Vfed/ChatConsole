using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace ChatConsole
{
    partial class ChatRuns
    {
       
        static async Task<Uri> CreateMessageAsync(Guid chatId,string userName,string massege)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/massege/add", new { ChatId = chatId, UserName = userName, Massege = massege, CurrentTime = DateTime.Now });
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        static async Task<List<Message>> GetChatMassagesAsync(Guid ChatId,Guid userId,DateTime date)
        {
            List<Message> messages = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/massege/get?ChatId={ChatId}&userId={userId}&date={date}");
            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<List<Message>>();
            }
            return messages;
        }
    }
}

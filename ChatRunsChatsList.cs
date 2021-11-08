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
        
        static async Task<Uri> AddNewUserToChatAsync(Guid userId, Guid chatId)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chatslist/add",new {UserId = userId, ChatId = chatId });
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        static async Task<DateTime> GetCurrentDateTimeAsync(Guid userId, Guid chatId)
        {
            DateTime date = DateTime.MinValue;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chatslist/gettime?userId=" + userId+"&chatId="+chatId);
            if (response.IsSuccessStatusCode)
            {
                date = await response.Content.ReadAsAsync<DateTime>();
            }
            return date;
        }
    }
}

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
        static async Task<List<ChatUser>> GetChatUsersAsync( Guid chatId)
        {
            List<ChatUser> chatUsers = new List<ChatUser>();
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chat/getchatusers?chatId={chatId}");
            if (response.IsSuccessStatusCode)
            {
                chatUsers = await response.Content.ReadAsAsync<List<ChatUser>>();
            }
            return chatUsers;
        }
        static async Task<Uri> ChangeChatNameAsync(string chatName,Guid chatId)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/chatname", new { ChatName = chatName, ChatId = chatId});
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        static async Task<List<Chat>> GetUserChatsAsync(Guid UserId)
        {
            List<Chat> chats = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chat/get?userId={UserId}");
            if (response.IsSuccessStatusCode)
            {
                chats = await response.Content.ReadAsAsync<List<Chat>>();
            }
            return chats;
        }

        static async Task<Uri> CreateChatAsync(Guid Id1, Guid Id2)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/add", new { UserId1 = Id1, UserId2 = Id2 });
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
    }
}

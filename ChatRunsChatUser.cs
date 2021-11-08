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
        
        static async Task<ChatUser> GetChatUserAsync(ChatUserDto dto)
        {
            ChatUser chatUser = null;
            HttpResponseMessage response = await client.PostAsJsonAsync(client.BaseAddress + $"api/chatuser/find", dto);
            if (response.IsSuccessStatusCode)
            {
                chatUser = await response.Content.ReadAsAsync<ChatUser>();
            }
            return chatUser;
        }
        // Check if User Exist
        static async Task<bool> GetChatUserExistAsync(string name)
        {
            bool chatUserExist = false;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chatuser/checkuser?username=" + name);
            if (response.IsSuccessStatusCode)
            {
                chatUserExist = await response.Content.ReadAsAsync<bool>();
            }
            return chatUserExist;
        }
        static async Task<List<ChatUser>> GetAllUsersAsync()
        {
            List<ChatUser> chatUsers = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chatuser/");
            if (response.IsSuccessStatusCode)
            {
                chatUsers = await response.Content.ReadAsAsync<List<ChatUser>>();
            }
            return chatUsers;
        }
        static async Task<List<ChatUser>> GetChatUsersExeptMeAsync(string name)
        {
            List<ChatUser> chatUser = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chatuser/getallexeptme?Username=" + name);
            if (response.IsSuccessStatusCode)
            {
                chatUser = await response.Content.ReadAsAsync<List<ChatUser>>();
            }
            return chatUser;
        }

        // Create new ChatUser
        static async Task<AccessToken> LoginUserAsync(ChatUserLoginDto dto)
        {
            AccessToken accessToken = null;
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chatuser/login", dto);
            if (response.IsSuccessStatusCode)
            {
                accessToken = await response.Content.ReadAsAsync<AccessToken>();
            }
            return accessToken;
        }
        // Create new ChatUser
        static async Task<Uri> CreateUserAsync(ChatUserLoginDto dto)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chatuser/add", dto);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
    }
}

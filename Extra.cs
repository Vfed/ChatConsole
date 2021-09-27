using System;
using System.Collections.Generic;
using System.Text;

namespace ChatConsole
{
    class Extra
    {
        //static async Task<Uri> AddNewUserToChatAsync(Guid userId, Guid chatId)
        //{
        //    HttpResponseMessage response = await client.PostAsJsonAsync("api/chatslist/add", new { UserId = userId, ChatId = chatId });
        //    response.EnsureSuccessStatusCode();
        //    return URI of the created resource.;
        //    return response.Headers.Location;
        //}


        //case "4":
        //    string newUserError = "";
        //    string newUserName = "";
        //    bool chkNewUser = false;
        //    do
        //    {
        //        Console.Clear();
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.Write(newUserError);
        //        newUserError = "";
        //        Console.ResetColor();
        //        chatError = "";
        //        Console.WriteLine($"Enter a Name of User you want to Add or(\\Exit): ");
        //        newUserName = Console.ReadLine().Trim();
        //        if (newUserName.Length > 3 && newUserName != "\\Exit")
        //        {
        //            ChatUser newChatUser = await GetChatUserAsync(newUserName);
        //            if (newChatUser != null)
        //            {
        //                var uri = await AddNewUserToChatAsync(newChatUser.Id, currentChat.ChatId);
        //            }
        //            else
        //            {
        //                chatNameError = $"No User found with name {newUserName} Enter;\n";
        //            }
        //            chkChangeName = true;
        //        }
        //        else
        //        {
        //            chatNameError = "Wrong Enter;\n";
        //        }
        //    } while (!chkNewUser && newUserName != "\\Exit");
        //    break;
    }
}

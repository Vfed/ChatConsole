using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ChatConsole
{
    class ChatRuns
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task<Uri> ChangeChatNameAsync(string chatName,Guid chatId)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/chatname", new { ChatName = chatName, ChatId = chatId});
            response.EnsureSuccessStatusCode();
            // return URI of the created resource.;
            return response.Headers.Location;
        }
        static async Task<Uri> CreateMessageAsync(Guid chatId,string userName,string massege)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/massege/add", new { ChatId = chatId, UserName = userName, Massege = massege, CurrentTime = DateTime.Now });
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        static async Task<List<Message>> GetChatMassagesAsync(Guid ChatId)
        {
            List<Message> messages = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/massege/get?ChatId="+ChatId);
            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<List<Message>>();
            }
            return messages;
        }
        static async Task<List<Chat>> GetUserChatsAsync(Guid UserId)
        {
            List<Chat> chats = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chat/get?UserId=" + UserId);
            if (response.IsSuccessStatusCode)
            {
                chats = await response.Content.ReadAsAsync<List<Chat>>();
            }
            return chats;
        }
        static async Task<ChatUser> GetChatUserAsync(string name)
        {
            ChatUser chatUser = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"api/chatuser/find?Username="+name);
            if (response.IsSuccessStatusCode)
            {
                chatUser = await response.Content.ReadAsAsync<ChatUser>();
            }
            return chatUser;
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
        static async Task<Uri> CreateUserAsync(string user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chatuser/add", new { Username = user });
            response.EnsureSuccessStatusCode();
            // return URI of the created resource.;
            return response.Headers.Location;
        }
        static async Task<Uri> CreateChatAsync(Guid Id1, Guid Id2)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/add", new { UserId1 = Id1, UserId2 = Id2 });
            response.EnsureSuccessStatusCode();
            // return URI of the created resource.;
            return response.Headers.Location;
        }
        
        public static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Chat currentChat;
                string name = "";
                ChatUser chatUser = new ChatUser();
                bool chkName = false;
                string error = "";
                // User Init
                do
                {
                    Console.Clear();
                    if (error.Length > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(error);
                        Console.ResetColor();
                        error = "";
                    }
                    Console.Write("Enter Your Name :");
                    name = Console.ReadLine().Trim();
                    if (name.Length < 3)
                    {
                        error += "Name can`t be shorter then 3 symbols";
                    }
                    else
                    {
                        chatUser = await GetChatUserAsync(name);
                        if (chatUser != null)
                        {
                            //Check if user is exists
                            Console.WriteLine(chatUser.Id);
                            chkName = true;
                        }
                        else
                        {
                            string question = "";
                            do
                            {
                                Console.Clear();
                                if (question != "")
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Wrong Enter");
                                    Console.ResetColor();
                                }
                                Console.WriteLine("This User in not exist creat new user ? (Yes/No)");
                                question = Console.ReadLine();
                            } while (question != "Yes" && question != "No");
                            if (question == "Yes")
                            {
                                await CreateUserAsync(name);
                                chatUser = await GetChatUserAsync(name);
                                chkName = true;
                            }
                        }
                    }
                } while (!chkName);
                bool choiseChatChk = false;
                string choiseChat = "";
                do
                {
                    currentChat = null;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"User : ( {chatUser.Username} )\n");
                        Console.WriteLine("Chose an action (Write a number):");
                        Console.WriteLine("1. Enter Chat;");
                        Console.WriteLine("2. Start a new Chat;");
                        Console.WriteLine("3. Exit");
                        choiseChat = Console.ReadLine();
                        switch (choiseChat)
                        {
                            case "1":
                                choiseChatChk = true;
                                break;
                            case "2":
                                choiseChatChk = true;
                                break;
                            case "3":
                                choiseChatChk = true;
                                break;
                            default:
                                choiseChatChk = false;
                                error += "Wrong Enter !";
                                break;
                        }
                    }while (!choiseChatChk);
                    if (choiseChat == "1")
                    {
                        List<Chat> chats = await GetUserChatsAsync(chatUser.Id);
                        if (chats != null)
                        {
                            int getNum = 0;
                            bool getNumChk = false;
                            do
                            {
                                Console.Clear();
                                int i = 1;
                                Console.WriteLine($"User : ( {chatUser.Username} )\n");
                                Console.WriteLine("Chose a chat (Write a number to enter):");
                                foreach (Chat item in chats)
                                {
                                    Console.WriteLine($"{i}. {item.ChatName}");
                                    i++;
                                }
                                Console.WriteLine($"{i} to Exit");
                                getNumChk = Int32.TryParse(Console.ReadLine(), out getNum);
                                if (getNum > 0 && getNum < i && getNumChk)
                                {
                                    currentChat = chats[getNum-1];
                                }
                                else
                                {
                                    getNumChk = false;
                                }
                                if (getNum == i) break;
                            } while (!getNumChk);
                        }
                    }
                    if (choiseChat == "2")
                    {
                        ChatUser newChatUser = null;
                        string newChatUserName = "";
                        bool getChatUserChk = false;
                        do
                        {
                            newChatUser = null;
                            Console.Clear();
                            Console.WriteLine($"User : ( {chatUser.Username} )\n");
                            Console.WriteLine("Write a User Name to start a chat with or \\Exit :");
                            newChatUserName = Console.ReadLine();
                            if (newChatUserName != "\\Exit")
                            {
                                newChatUser = await GetChatUserAsync(newChatUserName);
                                if (newChatUserName != null)
                                {

                                    getChatUserChk = true;
                                }
                                else
                                {
                                    getChatUserChk = false;
                                }
                            }
                            else 
                            {
                                break;
                            }
                        } while (!getChatUserChk);
                        if (newChatUser != null) 
                        {
                            var uri = CreateChatAsync(chatUser.Id,newChatUser.Id);
                            Console.WriteLine("Chat is created check it in Select a chat menu.");
                            Console.ReadKey();
                        }
                    }

                    if (currentChat != null) 
                    {
                        string chatError = "";
                        string chatAction = "";
                        do
                        {
                            Console.Clear();
                            if (chatError.Length > 0) 
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(chatError);
                                Console.ResetColor();
                            }
                            Console.WriteLine($"User : ( {chatUser.Username} )\n");
                            Console.WriteLine($"Chat {currentChat.ChatName} (chose an action):");
                            Console.WriteLine("1. Show Messages");
                            Console.WriteLine("2. Write Messages");
                            Console.WriteLine("3. Change Chat Name");
                            //Console.WriteLine("4. Add new User to Chat");
                            Console.WriteLine("4. Exit Chat");
                            chatAction = Console.ReadLine();
                            Console.Clear();
                            switch (chatAction)
                            {   
                                
                                case "1":
                                    List<Message> messages = await GetChatMassagesAsync(currentChat.ChatId);
                                    foreach (Message item in messages)
                                    {
                                        if (item.UserName == chatUser.Username)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine(item.CurrentTime + " " + item.UserName);
                                            Console.ResetColor();
                                        }
                                        else 
                                        {
                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine(item.CurrentTime + " " + item.UserName);
                                            Console.ResetColor();
                                        }
                                        Console.WriteLine(item.Massege);
                                    }
                                    Console.ReadKey();
                                    break;
                                case "2":
                                    Console.WriteLine($"Enter massage : ({chatUser.Username})");
                                    string massage = Console.ReadLine().Trim();
                                    if (massage.Length > 0)
                                    {
                                        var uri = await CreateMessageAsync(currentChat.ChatId, chatUser.Username, massage);
                                    }
                                    break;
                                case "3":
                                    string chatNameError = "";
                                    string chatName = "";
                                    bool chkChangeName = false;
                                    do
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(chatNameError);
                                        chatNameError = "";
                                        Console.ResetColor();
                                        chatError = "";
                                        Console.WriteLine($"Enter New Chat Name or(\\Exit): ");
                                        chatName = Console.ReadLine().Trim();
                                        if (chatName.Length > 5 && chatName != "\\Exit")
                                        {
                                            var uri = await ChangeChatNameAsync(chatName, currentChat.ChatId);
                                            chkChangeName = true;
                                        }
                                        else
                                        {
                                            chatNameError = "Wrong Enter;\n";
                                        }
                                    } while (!chkChangeName && chatName !="\\Exit");
                                    break;
                                default:
                                    chatError = "Wrong Enter;";
                                    break;
                            }
                        } while (chatAction != "4");
                    }
                    //Chat Exit;
                } while (choiseChat != "3");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

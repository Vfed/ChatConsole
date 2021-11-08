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
        private static readonly HttpClient client = new HttpClient();
        private static string _token { get; set; }
        private static ChatUser chatUser { get; set; }

        public static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Chat currentChat;
                string name = "";
                string password = "";
                ChatUser chatUser = new ChatUser();
                bool chkLogin = false;
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
                    Console.Write("Enter Password :");
                    password = Console.ReadLine().Trim();
                    if (name.Length < 3)
                    {
                        error += "Name can`t be shorter then 3 symbols";
                    }
                    else
                    { 
                        bool chatUserExist = await GetChatUserExistAsync(name);
                        if (chatUserExist) //Login to existing user
                        {
                            AccessToken accessToken = await LoginUserAsync(new ChatUserLoginDto { Username = name, Password = password });
                            if (accessToken == null)
                            {
                                error += "Can`t login Error !!!\n";
                            }
                            else
                            {
                                _token = accessToken.Token;
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                                chatUser = await GetChatUserAsync(new ChatUserDto { Username = name });
                                chkLogin = true;
                            }
                        }
                        else {  //Add new User s Login to it
                            bool newUserCreateQnEnterd = false;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine($"User \"{name}\" don`t exist;");
                                Console.WriteLine($"Create new User \"{name}\"?(yes/no)");
                                string newUserCreateQn = Console.ReadLine();
                                switch (newUserCreateQn) 
                                {
                                    case "yes":
                                        var userCreated = await CreateUserAsync(new ChatUserLoginDto { Username = name, Password = password });
                                        AccessToken accessToken = await LoginUserAsync(new ChatUserLoginDto { Username = name, Password = password });
                                        if (accessToken == null) 
                                        {
                                            error += "Can`t login Error !!!\n";
                                        }
                                        else
                                        {
                                            _token = accessToken.Token;
                                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                                            chatUser = await GetChatUserAsync(new ChatUserDto { Username = name });

                                            chkLogin = true;
                                        }
                                        newUserCreateQnEnterd = true;
                                        break;
                                    case "no":
                                        newUserCreateQnEnterd = true;
                                        break;
                                }
                            } while (!newUserCreateQnEnterd);
                        }
                    }
                } while (!chkLogin);
                


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
                                    DateTime date = await GetCurrentDateTimeAsync(chatUser.Id, item.ChatId);
                                    if (date < item.LastData)
                                        Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"{i}. {item.ChatName}");
                                    Console.ResetColor();
                                    i++;
                                }
                                Console.WriteLine($"{i} to Exit");
                                getNumChk = Int32.TryParse(Console.ReadLine(), out getNum);
                                if (getNum > 0 && getNum < chats.Count + 1 && getNumChk)
                                {
                                    currentChat = chats[getNum - 1];
                                }
                                else
                                {
                                    getNumChk = false;
                                }
                                if (getNum == chats.Count + 1) break;
                            } while (!getNumChk);

                        }
                        else 
                        {
                            Console.WriteLine("There are no active chats");
                            Console.ReadLine();
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
                                newChatUser = await GetChatUserAsync(new ChatUserDto { Username = newChatUserName });
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
                            List<ChatUser> chatUsers = await GetChatUsersAsync(currentChat.ChatId);
                            Console.Write("Chat Users : ");
                            
                            for (int i = 0; i <= chatUsers.Count -2 ; i++)
                            {
                                Console.Write($"{chatUsers[i].Username},");
                            }
                            if (chatUsers.Count - 1 > 0)
                            {
                                Console.WriteLine(chatUsers[chatUsers.Count - 1].Username);
                            }
                            Console.WriteLine($"Chat {currentChat.ChatName} (chose an action):");
                            DateTime date = await GetCurrentDateTimeAsync(chatUser.Id, currentChat.ChatId);
                            if (date < currentChat.LastData)
                                Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("1. Show Messages");
                            Console.ResetColor();
                            Console.WriteLine("2. Write Messages");
                            Console.WriteLine("3. Change Chat Name");
                            Console.WriteLine("4. Add new User to Chat");
                            Console.WriteLine("5. Exit Chat");
                            chatAction = Console.ReadLine();
                            Console.Clear();
                            switch (chatAction)
                            {   
                                
                                case "1":
                                    List<Message> messages = await GetChatMassagesAsync(currentChat.ChatId,chatUser.Id,DateTime.Now);
                                    messages = messages.OrderBy(x => x.CurrentTime).ToList();
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
                                case "4":
                                    string newUserError = "";
                                    string newUserName = "";
                                    bool chkNewUser = false;
                                    do
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(newUserError);
                                        newUserError = "";
                                        Console.ResetColor();
                                        chatError = "";
                                        Console.WriteLine($"Enter a Name of User you want to Add or(\\Exit): ");
                                        newUserName = Console.ReadLine().Trim();
                                        if (newUserName.Length > 3 && newUserName != "\\Exit")
                                        {
                                            ChatUser newChatUser = await GetChatUserAsync(new ChatUserDto { Username = newUserName });
                                            if (newChatUser != null)
                                            {
                                                var uri = await AddNewUserToChatAsync(newChatUser.Id, currentChat.ChatId);
                                            }
                                            else
                                            {
                                                chatNameError = $"No User found with name {newUserName} Enter;\n";
                                            }
                                            chkNewUser = true;
                                        }
                                        else
                                        {
                                            chatNameError = "Wrong Enter;\n";
                                        }
                                    } while (!chkNewUser && newUserName != "\\Exit");
                                    break;
                                default:
                                    chatError = "Wrong Enter;";
                                    break;
                            }
                        } while (chatAction != "5");
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

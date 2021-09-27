using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ChatConsole
{
    class ChatRuns
    {
        private static readonly HttpClient client = new HttpClient();
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
                Console.WriteLine("Enter name");
                //string name = Console.ReadLine().Trim();
                ChatUser user1 = await GetChatUserAsync("Gor");
                if (user1 is ChatUser) { Console.WriteLine("True"); } else { Console.WriteLine("False"); }
                ChatUser user2 = await GetChatUserAsync("Kong");
                if (user2 is ChatUser) { Console.WriteLine("True"); } else { Console.WriteLine("False"); }
                var url = await CreateChatAsync(user1.Id, user2.Id);
                Console.WriteLine(url);
                Console.ReadLine();

                // ++++ Get User ++++

                //ChatUser user = await GetChatUserAsync(name);
                //if (user is ChatUser){Console.WriteLine(user.Id);}else{Console.WriteLine(user);}

                // ++++ Get the ExeptMeName ++++

                //List<ChatUser> users = await GetChatUsersExeptMeAsync(name);
                //if (users != null){foreach (var item in users){Console.WriteLine(item.Username);}}

                // ++++ Post CreateUser ++++

                //var url = await CreateUserAsync(name);
                //Console.WriteLine(url);

                //++++ Get the AllUsersName++++

                //List<ChatUser> users = await GetAllUsersAsync();
                //if (users != null) { foreach (var item in users) { Console.WriteLine(item.Username); } }

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
                    //1 Enter chat
                    //2 Start new chat
                    //3 Exit
                    //Chat Choise
                    do
                    {
                        Console.WriteLine("Chose an action (Write a number):");
                        Console.WriteLine("1. Enter Chat;");
                        Console.WriteLine("2. Start a new Chat;");
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
                    }

                    while (!choiseChatChk);
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
                                Console.WriteLine("Chose a chat (Write a number to enter):");
                                foreach (Chat item in chats)
                                {
                                    Console.WriteLine($"{i}. {item.ChatName}");
                                    i++;
                                }
                                Console.WriteLine($"{i} to Exit");
                                getNumChk = Int32.TryParse(Console.ReadLine(), out getNum);
                                if (getNum > 0 && getNum <= i && getNumChk)
                                {
                                    currentChat = chats[getNum];
                                }
                                else
                                {
                                    getNumChk = false;
                                }
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
                            Console.WriteLine($"Chat {currentChat.ChatName} (chose an action):");
                            Console.WriteLine("1. Show Messages");
                            Console.WriteLine("2. Write Messages");
                            Console.WriteLine("3. Exit Chat");
                            chatAction = Console.ReadLine();
                            Console.Clear();
                            switch (chatAction)
                            {   
                                case "2":
                                    Console.WriteLine($"Enter massage : ({chatUser.Username})");
                                    string massage = Console.ReadLine().Trim();
                                    if (massage.Length > 0)
                                    {
                                        var uri = await CreateMessageAsync(currentChat.ChatId, chatUser.Username, massage);
                                    }
                                    break;
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
                                    break;
                                default:
                                    chatError = "Wrong Enter;";
                                    break;
                            }
                        } while (chatAction != "3");
                    }

                    //Chat Exit;
                } while (choiseChat != "3");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //static async Task<ChatUser> GetUserAsync(string path)
        //{
        //    ChatUser user = null;
        //    HttpResponseMessage response = await client.GetAsync(path);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        user = await response.Content.ReadAsAsync<ChatUser>();
        //    }
        //    return user;
        //}
        //static void Main(string[] args)
        //        {
        //            //RunAsync().GetAwaiter().GetResult();

        //        }
        //static HttpClient client = new HttpClient();
        //static void ShowUsers(User user)
        //{
        //    Console.WriteLine($"\tId: {user.Id}\tName: {user.Username}\tAge: " +
        //        $"{user.Age}");
        //}
        //static async Task<Uri> CreateProductAsync(User user)
        //{
        //    HttpResponseMessage response = await client.PostAsJsonAsync("api/user/", user);
        //    response.EnsureSuccessStatusCode();

        //    // return URI of the created resource.
        //    return response.Headers.Location;
        //}
        //static async Task<User> GetUserAsync(string path)
        //{
        //    User user = null;
        //    HttpResponseMessage response = await client.GetAsync(path+);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        user = await response.Content.ReadAsAsync<User>();
        //    }
        //    return user;
        //}

        //static async Task<User> UpdateUserAsync(User user)
        //{
        //    HttpResponseMessage response = await client.PutAsJsonAsync(
        //        $"api/user/{user.Id}", user);
        //    response.EnsureSuccessStatusCode();

        //    // Deserialize the updated product from the response body.
        //    user = await response.Content.ReadAsAsync<User>();
        //    return user;
        //}

        //static async Task<HttpStatusCode> DeleteProductAsync(string id)
        //{
        //    HttpResponseMessage response = await client.DeleteAsync(
        //        $"api/user/{id}");
        //    return response.StatusCode;
        //}



        //static async Task RunAsync()
        //{
        //    // Update port # in the following line.
        //    client.BaseAddress = new Uri("http://localhost:5000/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));

        //    try
        //    {
        //        Console.WriteLine("Enter your name :");
        //        string name = Console.ReadLine();

        //        //var url = await CreateProductAsync(user);
        //        //Console.WriteLine($"Created at {url}");

        //        // Get the product
        //        User user = await GetUserAsync(url.PathAndQuery);
        //        ShowUser(user);

        //        //// Update the product
        //        //Console.WriteLine("Updating price...");
        //        //product.Price = 80;
        //        //await UpdateProductAsync(product);

        //        //// Get the updated product
        //        //product = await GetProductAsync(url.PathAndQuery);
        //        //ShowProduct(product);

        //        //// Delete the product
        //        //var statusCode = await DeleteProductAsync(product.Id);
        //        //Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    Console.ReadLine();
        //}
    }
}

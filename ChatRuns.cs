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
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/add", new{UserId1 = Id1, UserId2 =Id2});
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

                var url = await CreateChatAsync(user1.Id,user2.Id);
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


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //try
            //{
            //    string path1 = "http://localhost:5000/api/chatuser/";
            //    string path2 = "http://localhost:5000/api/chat/";
            //    string path3 = "http://localhost:5000/api/message/";
            //    ChatUser chatUser = new ChatUser();
            //    bool chkName = false;
            //    string error = "";
            //    string name = "";
            //    //Start
            //    do
            //    {

            //        Console.Clear();
            //        if (error.Length > 0)
            //        {
            //            Console.ForegroundColor = ConsoleColor.Red;
            //            Console.WriteLine(error);
            //            Console.ResetColor();
            //            error = "";
            //        }
            //        Console.Write("Enter Your Name :");
            //        name = Console.ReadLine().Trim();
            //        if (name.Length > 3) 
            //        {
            //            chatUser = await GetChatUserAsync(name);
            //            if (chatUser != null)
            //            {
            //                Console.WriteLine(chatUser.Id);
            //                chkName = true;
            //            }
            //            else
            //            {
            //                string question = "";
            //                do
            //                {
            //                    Console.Clear();
            //                    if (question != "")
            //                    {
            //                        Console.ForegroundColor = ConsoleColor.Red;
            //                        Console.WriteLine("");
            //                        Console.ResetColor();
            //                    }
            //                    Console.WriteLine("This User in not exist creat new user ? (Yes/No)");
            //                    question = Console.ReadLine();
            //                } while (question != "Yes" && question != "No");
            //                if (question == "Yes")
            //                {
            //                    var stringTask = client.GetStringAsync(path1 + "add?Username=" + name);
            //                    Console.WriteLine(stringTask);

            //                    //var streamTask1 = client.GetStreamAsync(path1 + "add?Username=" + name);
            //                    //var user1 = await JsonSerializer.DeserializeAsync<ChatUser>(await streamTask1);

            //                    chkName = true;
            //                }
            //            }
            //        }
            //    } while (true);
            //        // Create a new product
            //ChatUserDto dto = new ChatUserDto();
            //dto.Username = "Victorio";
            //var url = await CreateChatUserAsync("Victorio");
            //Console.WriteLine($"Created at {url}");

            // Get the product

            //ChatUser chatUser = new ChatUser();
            //string path1 = "http://localhost:5000/api/chatuser/";
            //string path2 = "http://localhost:5000/api/chat/";
            //string path3 = "http://localhost:5000/api/message/";

            //string name = "Tod";
            //string error ="";
            //bool chkName = true;

            //do
            //{
            //    Console.Clear();
            //    if (error.Length > 0)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Red;
            //        Console.WriteLine(error);
            //        Console.ResetColor();
            //        error = "";
            //    }
            //    Console.Write("Enter Your Name :");
            //    name = Console.ReadLine().Trim();
            //    if (name.Length > 3)
            //    {
            //        var streamTask = client.GetStreamAsync(path1 + "find?Username=" + name);
            //        var user = await JsonSerializer.DeserializeAsync<ChatUser>(await streamTask);

            //        if (user != null)
            //        {
            //            string question = "";
            //            do
            //            {
            //                Console.Clear();
            //                if (question != "")
            //                {
            //                    Console.ForegroundColor = ConsoleColor.Red;
            //                    Console.WriteLine("Wrong Enter !");
            //                    Console.ResetColor();
            //                }
            //                Console.WriteLine("This User is already exists, continue ? (Yes/No)");
            //                question = Console.ReadLine();
            //            } while (question != "Yes" && question != "No");
            //            switch (question)
            //            {
            //                case "Yes":
            //                    chatUser = user;
            //                    chkName = true;
            //                    break;
            //                default:
            //                    chkName = false;
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            string question = "";
            //            do
            //            {
            //                Console.Clear();
            //                if (question != "")
            //                {
            //                    Console.ForegroundColor = ConsoleColor.Red;
            //                    Console.WriteLine("");
            //                    Console.ResetColor();
            //                }
            //                Console.WriteLine("This User in not exist creat new user ? (Yes/No)");
            //                question = Console.ReadLine();
            //            } while (question != "Yes" && question != "No");
            //            if (question == "Yes")
            //            {
            //                var stringTask = client.GetStringAsync(path1 + "add?Username=" + name);
            //                Console.WriteLine(stringTask);

            //                //var streamTask1 = client.GetStreamAsync(path1 + "add?Username=" + name);
            //                //var user1 = await JsonSerializer.DeserializeAsync<ChatUser>(await streamTask1);

            //                chkName = true;
            //            }
            //            else { chkName = false; }
            //        }
            //    }
            //    else
            //    {
            //        chkName = false;
            //        error = " Wrong Enter !";
            //    }
            //} while (!chkName);
            ////var stringTask = client.GetStringAsync("http://localhost:5000/api/chatuser/");
            ////var streamTask = client.GetStreamAsync("http://localhost:5000/api/chatuser/serial");
            ////var users = await JsonSerializer.DeserializeAsync<List<ChatUser>>(await streamTask);
            ////var msg = await stringTask;

            ////var msg = await streamTask;
            ////foreach (var item in users)
            ////{
            ////    if (item is ChatUser)
            ////    {
            ////        Console.WriteLine(item.Id);
            ////        Console.WriteLine(item.Username+"\n");
            ////    }
            ////    //Console.WriteLine(item);
            ////}
            ////Console.Write(msg);

            //if (chatUser != null)
            //{
            //    Console.WriteLine(chatUser.Id + " " + chatUser.Username + " " + chatUser.Chats);
            //}
            //Console.ReadKey();
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

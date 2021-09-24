using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatConsole
{
    class ChatUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public List<ChatsList> Chats { get; set; }
    }
    class ChatsList
    {
        public Guid Id { get; set; }
        public ChatUser ChatUser { get; set; }
        public Chat Chat { get; set; }
    }

    class Chat 
    {
        public Guid ChatId { get; set; }
        public string ChatName { get; set; }
        public DateTime LastData { get; set; }
        public List<ChatsList> ChatsList { get; set; }
        public List<Message> Messages { get; set; }
    }

    class Message
    {
        public Guid Id { get; set; }
        public Chat Chat { get; set; }
        public string UserName { get; set; }
        public string Massege { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}

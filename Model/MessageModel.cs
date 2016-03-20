using System;

namespace Model
{
    public class MessageModel
    {
        public int Id { get; set; }
        public int? MessageId { get; set; }
        public string TextMessage { get; set; }
        public DateTime DatePosted { get; set; }
        public string UserName { get; set; }
        public int Comments { get; set; }
    }
}
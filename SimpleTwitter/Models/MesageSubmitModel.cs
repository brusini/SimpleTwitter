using System;

namespace SimpleTwitter.Models
{
    public class MesageSubmitModel
    {
        public int? MessageId { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}
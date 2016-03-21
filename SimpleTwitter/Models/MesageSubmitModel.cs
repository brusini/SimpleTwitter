using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleTwitter.Models
{
    public class MesageSubmitModel
    {
        public int? MessageId { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Message { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
    }
}
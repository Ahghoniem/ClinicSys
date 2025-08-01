using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.ChatDTOs
{
    public class MessageDto
    {
        public int ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
    }
}

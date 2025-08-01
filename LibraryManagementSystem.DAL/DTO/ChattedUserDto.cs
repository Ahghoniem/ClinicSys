using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.DAL.Entities.Chat;

namespace LibraryManagementSystem.DAL.DTO
{
    public class ChattedUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }
        public int ConversationId { get; set; }
        public MessageStatus? state { get; set; }

        public string? senderId { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }
}

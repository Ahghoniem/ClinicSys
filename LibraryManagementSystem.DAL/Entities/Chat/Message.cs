using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities.Chat
{
    public enum MessageStatus
    {
        NotSent = 0,
        Sent = 1,
        Delivered = 2,
        Read = 3
    }
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public MessageStatus Status { get; set; } = MessageStatus.NotSent;
    }
}

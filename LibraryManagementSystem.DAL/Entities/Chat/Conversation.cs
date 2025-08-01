using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities.Chat
{
    public class Conversation
    {
        public int Id { get; set; }
        public ICollection<ConversationUser> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}

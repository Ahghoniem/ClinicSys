using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities.Chat
{
    public class ConversationUser
    {
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

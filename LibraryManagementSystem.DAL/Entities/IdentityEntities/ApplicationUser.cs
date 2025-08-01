using LibraryManagementSystem.DAL.Entities.Chat;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities.IdentityEntities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public int UserType { get; set; }
        public ICollection<ConversationUser> Conversations { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.ChatDTOs
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public List<string> UserIds { get; set; }
    }
}

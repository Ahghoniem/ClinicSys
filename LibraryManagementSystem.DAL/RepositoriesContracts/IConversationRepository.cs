using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities.Chat;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.RepositoriesContracts
{
    public interface IConversationRepository
    {
        Task<Conversation> CreateConversationAsync(string user1Id, string user2Id);
        Task<Conversation> GetConversationByIdAsync(int id);
        Task<List<Message>> GetMessagesAsync(int conversationId);
        Task<Message> AddMessageAsync(Message message);
        Task<List<ChattedUserDto>> GetChattedUsersSortedByLastMessageAsync(string currentUserId);
        Task<Message> UpdateMessageStatusAsync(int messageId, MessageStatus status);
        Task<Message> GetMessageByIdAsync(int messageId);
        public Task<List<ApplicationUser>> GetUsersInConversationAsync(string currentUserId);
        public  Task MarkAllMessagesAsReadAsync(int conversationId, string userId);


    }
}

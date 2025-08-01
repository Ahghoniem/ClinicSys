using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities.Chat;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IChatService
    {
        Task<Conversation> CreateConversationAsync(string user1Id, string user2Id);
        Task<IEnumerable<Message>> UpdateAllSentMessagesToDeliveredAsync(int conversationId, string receiverId);

        Task<List<Message>> GetMessagesAsync(int conversationId);
        Task<Message> SendMessageAsync(int conversationId, string senderId, string content);
        public Task<List<ChattedUserDto>> GetChattedUsersSortedByLastMessageAsync(string userId);
        Task<Message> UpdateMessageStatusAsync(int messageId, MessageStatus status);
        Task<Message> GetMessageByIdAsync(int messageId);
        Task<List<int>> GetConversationIdsForUserAsync(string userId);
        public  Task MarkAllMessagesAsReadAsync(int conversationId, string userId);

    }
}

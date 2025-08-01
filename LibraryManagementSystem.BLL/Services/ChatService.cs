using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities.Chat;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.UOW;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Services
{

    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LibraryDbContext _context;

        public ChatService(IUnitOfWork unitOfWork, LibraryDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _context = dbContext;
        }
        public async Task<IEnumerable<Message>> UpdateAllSentMessagesToDeliveredAsync(int conversationId, string receiverId)
        {
            var messages = await _context.Message
                .Where(m => m.ConversationId == conversationId &&
                            m.SenderId != receiverId &&
                            m.Status == MessageStatus.Sent)
                .ToListAsync();

            foreach (var msg in messages)
            {
                msg.Status = MessageStatus.Delivered;
            }

            await _context.SaveChangesAsync();
            return messages;
        }


        public async Task<Conversation> CreateConversationAsync(string user1Id, string user2Id)
        {
            var user1 = await _context.Users.FindAsync(user1Id);
            var user2 = await _context.Users.FindAsync(user2Id);

            if (user1 == null || user2 == null)
                throw new ArgumentException("One or both users do not exist.");

            // Must be one doctor and one patient
            bool validPair = (user1.UserType == 3 && user2.UserType == 4) ||
                             (user1.UserType == 4 && user2.UserType == 3);

            if (!validPair)
                throw new InvalidOperationException("Only Doctor and Patient can start a conversation.");

            var existing = await _context.ConversationUser
                .Where(cu => cu.UserId == user1Id)
            .Select(cu => cu.ConversationId)
                .Intersect(_context.ConversationUser
                    .Where(cu => cu.UserId == user2Id)
                    .Select(cu => cu.ConversationId))
                .FirstOrDefaultAsync();

            if (existing != 0)
                throw new InvalidOperationException("Conversation already exists.");

            var conversation = new Conversation
            {
                Users = new List<ConversationUser>
        {
            new ConversationUser { UserId = user1Id },
            new ConversationUser { UserId = user2Id }
                }
            };

            _context.Conversation.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }


        public async Task<List<Message>> GetMessagesAsync(int conversationId)
        {
            return await _unitOfWork.Conversations.GetMessagesAsync(conversationId);
        }

        public async Task<Message> SendMessageAsync(int conversationId, string senderId, string content)
        {
            var message = new Message
            {
                ConversationId = conversationId,
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.UtcNow,
                Status = MessageStatus.Sent
            };

            return await _unitOfWork.Conversations.AddMessageAsync(message);
        }

        public async Task<List<ChattedUserDto>> GetChattedUsersSortedByLastMessageAsync(string userId)
        {
            return await _unitOfWork.Conversations.GetChattedUsersSortedByLastMessageAsync(userId);
        }

        public async Task<Message> UpdateMessageStatusAsync(int messageId, MessageStatus status)
        {
            return await _unitOfWork.Conversations.UpdateMessageStatusAsync(messageId, status);
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _unitOfWork.Conversations.GetMessageByIdAsync(messageId);
        }

        public async Task<List<int>> GetConversationIdsForUserAsync(string userId)
        {
            var conversations = await _unitOfWork.Conversations.GetUsersInConversationAsync(userId);
            return conversations.SelectMany(u => u.Conversations)
                                .Select(cu => cu.ConversationId)
                                .Distinct()
                                .ToList();
        }
public async Task MarkAllMessagesAsReadAsync(int conversationId, string userId)
        {
            await _unitOfWork.Conversations.MarkAllMessagesAsReadAsync(conversationId, userId);
        }
    }

}


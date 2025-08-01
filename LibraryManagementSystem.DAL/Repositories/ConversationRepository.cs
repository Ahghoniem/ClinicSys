using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities.Chat;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Repositories
{

    public class ConversationRepository : IConversationRepository
    {
        private readonly LibraryDbContext _context;

        public ConversationRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> CreateConversationAsync(string user1Id, string user2Id)
        {
            var user1Exists = await _context.Users.AnyAsync(u => u.Id == user1Id);
            var user2Exists = await _context.Users.AnyAsync(u => u.Id == user2Id);

            if (!user1Exists || !user2Exists)
                throw new ArgumentException("One or both users do not exist.");
            var conversation = new Conversation
            {
                Users = new List<ConversationUser>
            {
                new ConversationUser { UserId = user1Id },
                new ConversationUser { UserId = user2Id }
            }
            };

            _context.Conversation.Add(conversation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log or return ex.InnerException?.Message to help debugging
                throw new Exception("Error while saving conversation: " + ex.InnerException?.Message, ex);
            }
            return conversation;
        }

        public async Task<Conversation> GetConversationByIdAsync(int id)
        {
            return await _context.Conversation
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Message>> GetMessagesAsync(int conversationId)
        {
            return await _context.Message
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<List<ChattedUserDto>> GetChattedUsersSortedByLastMessageAsync(string currentUserId)
        {
            var currentUserType = await _context.Users
                .Where(u => u.Id == currentUserId)
                .Select(u => u.UserType)
                .FirstOrDefaultAsync();

            var allowedTargetType = currentUserType == 3 ? 4 : currentUserType == 4 ? 3 : -1;

            var conversations = await _context.Conversation
                .Include(c => c.Users)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Messages)
                .Where(c => c.Users.Any(u => u.UserId == currentUserId))
                .ToListAsync();

            var result = conversations
                .SelectMany(conv => conv.Users
                    .Where(u => u.UserId != currentUserId && u.User.UserType == allowedTargetType)
                    .Select(u => new
                    {
                        User = u.User,

                        ConversationId = conv.Id,
                        LastMessage = conv.Messages
                            .OrderByDescending(m => m.SentAt)
                            .FirstOrDefault()
                    }))
                .GroupBy(x => x.User.Id)
                .Select(g => g.OrderByDescending(x => x.LastMessage?.SentAt).First())
                .OrderByDescending(x => x.LastMessage?.SentAt)
                .Select(x => new ChattedUserDto
                {
                    Id = x.User.Id,
                    Email = x.User.Email,
                    FullName = x.User.FullName,
                    ImageUrl = x.User is Doctor doc ? doc.ImageUrl : (x.User is Patient pat ? pat.ImageUrl : null),
                    ConversationId = x.ConversationId,
                    LastMessage = x.LastMessage?.Content,
                    LastMessageTime = x.LastMessage?.SentAt,
                    state=x.LastMessage?.Status,
                    senderId=x.LastMessage?.SenderId
                })
                .ToList();

            return result;
        }

        public async Task<List<ApplicationUser>> GetUsersInConversationAsync(string currentUserId)
        {
            var currentUser = await _context.Users.FindAsync(currentUserId);
            if (currentUser == null) throw new Exception("User not found");

            int currentUserType = currentUser.UserType;

            // Only allow Doctor-Patient chats
            int allowedTargetType = currentUserType == 3 ? 4 : currentUserType == 4 ? 3 : -1;

            if (allowedTargetType == -1)
                return new List<ApplicationUser>();

            var users = await _context.ConversationUser
                .Where(cu => cu.UserId == currentUserId)
                .SelectMany(cu => cu.Conversation.Users)
                .Where(cu => cu.UserId != currentUserId && cu.User.UserType == allowedTargetType)
                .Select(cu => cu.User)
                .Distinct()
                .ToListAsync();

            return users;
        }


        public async Task<Message> AddMessageAsync(Message message)
        {
            message.Status = MessageStatus.Sent;
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> UpdateMessageStatusAsync(int messageId, MessageStatus status)
        {
            var message = await _context.Message.FindAsync(messageId);
            if (message == null) throw new Exception("Message not found");

            message.Status = status;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.Message
                .Include(m => m.Sender)
                .Include(m => m.Conversation)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }
        public async Task MarkAllMessagesAsReadAsync(int conversationId, string userId)
        {
            Console.WriteLine(conversationId);
            Console.WriteLine(userId);
            var messagesToUpdate = await _context.Message
                .Where(m => m.ConversationId == conversationId && m.SenderId != userId && m.Status != MessageStatus.Read)
                .ToListAsync();

            foreach (var message in messagesToUpdate)
            {
                Console.WriteLine("Ana kdhioahfwilnflknlnl");
                message.Status = MessageStatus.Read;
            }

            await _context.SaveChangesAsync();
        }

    }

}

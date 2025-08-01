using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.Entities.Chat;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly UserManager<ApplicationUser> _userManager;
        private static HashSet<string> ConnectedUsers = new();
        private static ConcurrentDictionary<string, string> ConnectionMap = new();
        private static ConcurrentDictionary<int, HashSet<string>> ChatViewers = new();
        public ChatHub(IChatService chatService, UserManager<ApplicationUser> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        public async Task SendMessage(int conversationId, string senderId, string message)
        {


            var sentMessage = await _chatService.SendMessageAsync(conversationId, senderId, message);

            await Clients.All.SendAsync("ReceiveMessage", sentMessage);
        }

        public async Task JoinConversation(int conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }

        public async Task MarkAsDelivered(int messageId)
        {
            var message = await _chatService.UpdateMessageStatusAsync(messageId, MessageStatus.Delivered);
            await Clients.All.SendAsync("MessageDelivered", message);
        }
        public async Task MarkAllAsDelivered(int conversationId, string receiverId)
        {
            // Call the service to update all "sent" messages for this conversation & receiver
            var updatedMessages = await _chatService.UpdateAllSentMessagesToDeliveredAsync(conversationId, receiverId);

            // Broadcast updated messages (you can refine this to a specific group if needed)
            foreach (var message in updatedMessages)
            {
                await Clients.All.SendAsync("MessageAllDelivered", message);
            }
        }

        public Task RegisterUser(string userId)
        {
            ConnectedUsers.Add(userId);
            ConnectionMap[Context.ConnectionId] = userId;

            // Notify others that this user is online
            Clients.All.SendAsync("UserStatusChanged", userId, "Online");

            return Task.CompletedTask;
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;

            if (ConnectionMap.TryRemove(connectionId, out var userId))
            {
                if (!ConnectionMap.Values.Contains(userId))
                {
                    ConnectedUsers.Remove(userId);

                    // Notify others that this user is offline
                    await Clients.All.SendAsync("UserStatusChanged", userId, "Offline");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        public Task<bool> IsUserOnline(string userId)
        {
            return Task.FromResult(ConnectedUsers.Contains(userId));
        }
        public async Task MarkAsRead(int messageId)
        {
            var message = await _chatService.UpdateMessageStatusAsync(messageId, MessageStatus.Read);
            await Clients.All.SendAsync("MessageRead", message);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                var conversationIds = await _chatService.GetConversationIdsForUserAsync(userId);
                foreach (var id in conversationIds)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
                }
            }

            await base.OnConnectedAsync();
        }
        public async Task JoinConversationView(int conversationId, string UserId)
        {
            var userId = UserId;
            if (userId == null) return;

            // Add user to the group and internal tracking
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat-view-{conversationId}");

            lock (ChatViewers)
            {
                if (!ChatViewers.ContainsKey(conversationId))
                    ChatViewers[conversationId] = new HashSet<string>();

                ChatViewers[conversationId].Add(userId);
            }

            // Notify others in the conversation
            await Clients.All.SendAsync("UserViewingChat", userId, conversationId, true);
        }
        public async Task LeaveConversationView(int conversationId, string UserId)
        {
            var userId = UserId;
            if (userId == null) return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat-view-{conversationId}");

            lock (ChatViewers)
            {
                if (ChatViewers.ContainsKey(conversationId))
                {
                    ChatViewers[conversationId].Remove(userId);
                }
            }

            await Clients.All.SendAsync("UserViewingChat", userId, conversationId, false);
        }
    }
}

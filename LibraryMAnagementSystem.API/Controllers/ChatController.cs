using LibraryManagementSystem.BLL.DTOs.ChatDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryMAnagementSystem.API.ResponseHandler;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMAnagementSystem.API.Controllers
{
    public class ChatController : BaseAPIController
    {
        private readonly IChatService _chatService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(IChatService chatService, UserManager<ApplicationUser> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        [HttpPost("conversation")]
        public async Task<IActionResult> CreateConversation([FromQuery] string user1Id, [FromQuery] string user2Id)
        {
            var conversation = await _chatService.CreateConversationAsync(user1Id, user2Id);
            return Ok(conversation);
        }

        [HttpGet("messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var messages = await _chatService.GetMessagesAsync(conversationId);
            return Ok(messages);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string userId)
        {
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser == null) return NotFound("User not found.");

            int currentUserType = currentUser.UserType;

            int allowedTargetType = currentUserType == 3 ? 4 : currentUserType == 4 ? 3 : -1;

            if (allowedTargetType == -1)
                return Ok(new List<object>());

            var users = await _userManager.Users
    .Where(u => u.UserType == allowedTargetType)
    .ToListAsync(); // execute query first

            var result = users.Select(u => new
            {
                u.Id,
                u.Email,
                u.FullName,
                ImageUrl = u is Doctor doc ? doc.ImageUrl : (u is Patient pat ? pat.ImageUrl : null)
            });

            return Ok(new ApiResponse<object>(result, "REtrived", 200));
        }



        [HttpGet("chatted-users")]
        public async Task<IActionResult> GetChattedUsers([FromQuery] string user1Id)
        {
            var users = await _chatService.GetChattedUsersSortedByLastMessageAsync(user1Id);
            return Ok(users);
        }



        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            var message = await _chatService.SendMessageAsync(messageDto.ConversationId, messageDto.SenderId, messageDto.Content);

            return Ok(message);
        }

        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllMessagesAsRead([FromBody] ReadAllDto dto)
        {
            await _chatService.MarkAllMessagesAsReadAsync( dto.ConversationId, dto.userId);
            return Ok(new ApiResponse<string>("All messages marked as read.", "Success", 200));
        }

    }
}

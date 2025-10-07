using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Talksy.Api.Dtos.Chat;
using Talksy.Api.Dtos.Message;
using Talksy.Api.Services;
using UserModel = Talksy.Api.Models.User;

namespace Talksy.Api.Controllers;

/// <summary>
/// Управление чатами и сообщениями.
/// </summary>
[ApiController]
[Route("api/chat")]
public sealed class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    private Guid GetUserIdFromJwt()
    {
        var userIdRaw = User.FindFirst(UserModel.JwtUserIdClaimName)!.Value;
        var userId = Guid.Parse(userIdRaw);
        return userId;
    }

    /// <summary>
    /// Получить список чатов текущего пользователя.
    /// </summary>
    /// <returns>Список чатов.</returns>
    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Получить чаты пользователя",
        Description = "Возвращает список чатов для текущего авторизованного пользователя.")]
    [ProducesResponseType(typeof(IEnumerable<ChatDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ChatDto>>> GetChatsAsync()
    {
        var userId = GetUserIdFromJwt();
        var (_, isFailure, chatDtos) = await _chatService.FindChatsForUserAsync(userId);

        if (isFailure)
        {
            return NotFound();
        }

        return Ok(chatDtos);
    }


    /// <summary>
    /// Создать новый чат между текущим пользователем и другим пользователем.
    /// </summary>
    /// <param name="receiverId">Id пользователя, с которым создаётся чат.</param>
    /// <returns>Информация о созданном чате.</returns>
    [HttpPost("{receiverId:guid}")]
    [Authorize]
    [SwaggerOperation(Summary = "Создать чат",
        Description = "Создает чат между текущим пользователем и указанным получателем.")]
    [ProducesResponseType(typeof(ChatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ChatDto>> CreateChatAsync(Guid receiverId)
    {
        var senderId = GetUserIdFromJwt();
        var (_, isFailure, chatDto, error) = await _chatService.CreateChatAsync(senderId, receiverId);

        if (isFailure)
        {
            return BadRequest(error);
        }

        return Ok(chatDto);
    }

    /// <summary>
    /// Получить все сообщения из указанного чата.
    /// </summary>
    /// <param name="chatId">Id чата.</param>
    /// <returns>Список сообщений.</returns>
    [HttpGet("message/{chatId:guid}")]
    [Authorize]
    [SwaggerOperation(Summary = "Получить сообщения чата",
        Description = "Возвращает список сообщений из указанного чата.")]
    [ProducesResponseType(typeof(IEnumerable<MessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesFromChatAsync(Guid chatId)
    {
        var (_, isFailure, messageDtos, error) = await _chatService.GetMessagesFromChatAsync(chatId);

        if (isFailure)
        {
            return BadRequest(error);
        }

        return Ok(messageDtos);
    }
    
    /// <summary>
    /// Получить последнее из указанного чата.
    /// </summary>
    /// <param name="chatId">Id чата.</param>
    /// <returns>Последние сообщение.</returns>
    [HttpGet("message/{chatId:guid}/last")]
    [Authorize]
    [SwaggerOperation(Summary = "Получить последние сообщение в чате",
        Description = "Возвращает последнее сообщение из указанного чата.")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MessageDto>> GetLastMessageFromChatAsync(Guid chatId)
    {
        var (_, isFailure, messageDto, error) = await _chatService.GetLastMessageFromChatAsync(chatId);

        if (isFailure)
        {
            return NotFound(error);
        }

        return Ok(messageDto);
    }

    /// <summary>
    /// Отправить сообщение в чат.
    /// </summary>
    /// <param name="dto">Данные для создания сообщения.</param>
    /// <returns>Отправленное сообщение.</returns>
    [HttpPost("message")]
    [Authorize]
    [SwaggerOperation(Summary = "Отправить сообщение", Description = "Отправляет новое сообщение в чат.")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MessageDto>> SendMessageAsync(MessageCreateDto dto)
    {
        var (_, isFailure, messageDto, error) = await _chatService.SendMessageAsync(dto);

        if (isFailure)
        {
            return BadRequest(error);
        }

        return Ok(messageDto);
    }
}
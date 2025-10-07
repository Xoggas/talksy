using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Talksy.Api.Data;
using Talksy.Api.Dtos.Chat;
using Talksy.Api.Dtos.Message;
using Talksy.Api.Hubs;
using Talksy.Api.Models;

namespace Talksy.Api.Services;

public interface IChatService
{
    Task<Result<IEnumerable<ChatDto>>> FindChatsForUserAsync(Guid userId);
    Task<Result<ChatDto>> CreateChatAsync(Guid youId, Guid themId);
    Task<Result<IEnumerable<MessageDto>>> GetMessagesFromChatAsync(Guid chatId);
    Task<Result<MessageDto>> GetLastMessageFromChatAsync(Guid chatId);
    Task<Result<MessageDto>> SendMessageAsync(MessageCreateDto dto);
}

public sealed class ChatService : IChatService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatEventHub, IChatEventHub> _eventHub;

    public ChatService(AppDbContext dbContext, IMapper mapper, IHubContext<ChatEventHub, IChatEventHub> eventHub)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _eventHub = eventHub;
    }

    public async Task<Result<IEnumerable<ChatDto>>> FindChatsForUserAsync(Guid userId)
    {
        var chatModels = await _dbContext.Chats
            .Where(x => x.ChatMember1.Id == userId || x.ChatMember2.Id == userId)
            .Include(x => x.ChatMember1)
            .Include(x => x.ChatMember2)
            .ToListAsync();

        if (chatModels.Count == 0)
        {
            return Result.Failure<IEnumerable<ChatDto>>("There are no chats");
        }

        var chatDtos = _mapper.Map<IEnumerable<ChatDto>>(chatModels);

        return Result.Success(chatDtos);
    }

    public async Task<Result<ChatDto>> CreateChatAsync(Guid youId, Guid themId)
    {
        var you = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == youId);
        var them = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == themId);

        if (you is null || them is null)
        {
            return Result.Failure<ChatDto>("One of the users doesn't exist");
        }

        var chatExists = _dbContext.Chats.Any(x =>
            x.ChatMember1.Id == youId && x.ChatMember2.Id == themId ||
            x.ChatMember1.Id == themId && x.ChatMember2.Id == youId);

        if (chatExists)
        {
            return Result.Failure<ChatDto>("There is already a chat");
        }

        var chatModel = new Chat
        {
            Id = Guid.CreateVersion7(),
            ChatMember1 = you,
            ChatMember2 = them,
        };

        await _dbContext.Chats.AddAsync(chatModel);
        await _dbContext.SaveChangesAsync();

        var chatDtoForYou = _mapper.Map<ChatDto>(chatModel);
        
        await _eventHub.Clients.All.ChatCreated(chatDtoForYou);
        
        return Result.Success(chatDtoForYou);
    }

    public async Task<Result<IEnumerable<MessageDto>>> GetMessagesFromChatAsync(Guid chatId)
    {
        var messages = await _dbContext.Messages
            .Where(x => x.Chat.Id == chatId)
            .OrderBy(x => x.SentAt)
            .Include(x => x.Chat)
            .Include(x => x.Chat.ChatMember1)
            .Include(x => x.Chat.ChatMember2)
            .Include(x => x.Sender)
            .ToListAsync();

        if (messages.Count == 0)
        {
            return Result.Failure<IEnumerable<MessageDto>>("There are no messages");
        }

        var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);

        return Result.Success(messagesDto);
    }

    public async Task<Result<MessageDto>> GetLastMessageFromChatAsync(Guid chatId)
    {
        var message = await _dbContext.Messages
            .Where(x => x.Chat.Id == chatId)
            .OrderBy(x => x.SentAt)
            .Include(x => x.Chat)
            .Include(x => x.Chat.ChatMember1)
            .Include(x => x.Chat.ChatMember2)
            .Include(x => x.Sender)
            .FirstOrDefaultAsync();

        if (message is null)
        {
            return Result.Failure<MessageDto>("There are no messages");
        }

        var messageDto = _mapper.Map<MessageDto>(message);

        return Result.Success(messageDto);
    }

    public async Task<Result<MessageDto>> SendMessageAsync(MessageCreateDto dto)
    {
        var sender = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == dto.SenderId);
        if (sender is null)
        {
            return Result.Failure<MessageDto>("Sender doesn't exist");
        }

        var chat = await _dbContext.Chats
            .Include(x => x.ChatMember1)
            .Include(x => x.ChatMember2)
            .FirstOrDefaultAsync(x => x.Id == dto.ChatId);
        
        if (chat is null)
        {
            return Result.Failure<MessageDto>("Chat doesn't exit");
        }

        var messageModel = new Message
        {
            Id = Guid.CreateVersion7(),
            SentAt = DateTime.Now,
            Chat = chat,
            Sender = sender,
            Content = dto.Content
        };

        await _dbContext.Messages.AddAsync(messageModel);
        await _dbContext.SaveChangesAsync();

        var messageDto = _mapper.Map<MessageDto>(messageModel);

        await _eventHub.Clients.All.GotNewMessage(messageDto);

        return Result.Success(messageDto);
    }
}
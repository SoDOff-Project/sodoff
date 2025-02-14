using System;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services;

public class MessagingService
{
    private readonly DBContext ctx;
    
    public MessagingService(DBContext ctx)
    {
        this.ctx = ctx;
    }

    public Model.Message AddMessageToViking(Viking viking, Viking toViking, string apiToken, MessageType type, MessageTypeID typeID, MessageLevel level, string content, string memberMessage = "", string nonMemberMessage = "", bool isReply = false, int replyMessageID = 0) // previous implementation had seperate functions for text and data messages, lets combine them
    {
        Random rnd = new Random(); // 'ConversationId' and 'QueueId'

        // create a new message
        Model.Message message = new Model.Message
        {
            ConversationID = rnd.Next(1000, 9999),
            QueueID = rnd.Next(1000, 9999),
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            MessageType = type,
            MessageLevel = level,
            MessageTypeID = typeID,
            Data = content,
            IsNew = true // immediately mark as new
        };

        if (isReply == true && replyMessageID != 0)
        {
            // find the message this message is in reply to
            Model.Message? messageToReplyTo = ctx.Messages.FirstOrDefault(e => e.Id == replyMessageID);

            if(messageToReplyTo != null) 
            {
                // set conversationid and queueid
                message.ConversationID = messageToReplyTo.ConversationID; 
                message.QueueID = messageToReplyTo.QueueID;

                // add to parent messages replies, create a collection of replies if message has none
                if(messageToReplyTo.Replies == null) messageToReplyTo.Replies = new List<Model.Message>();

                // add to list of replies
                messageToReplyTo.Replies.Add(messageToReplyTo); // EF Core should do the rest (hopefully)

                // return message here, do not add another parent message to message board
                return message;
            }
        }

        // add it to message lists
        toViking.MessageBoard.Add(message);
        viking.MessagesMade.Add(message);
        ctx.SaveChanges();

        return message;
    }

    public bool RemoveMessage(int id)
    {
        Model.Message? messageToRemove = ctx.Messages.FirstOrDefault(e => e.Id == id);
        if (messageToRemove != null)
        {
            ctx.Messages.Remove(messageToRemove);

            ctx.SaveChanges();
            return true;
        }

        return false;
    }

    public CombinedListMessage[] ConstructCombinedMessageArray(Viking viking)
    {
        // get all viking messages
        List<Model.Message> messages = viking.MessageBoard.ToList();

        // create a new list of CombinedListMessage for the response
        List<CombinedListMessage> response = new List<CombinedListMessage>();

        // get current DateTime
        DateTime now = DateTime.UtcNow;

        foreach(Model.Message message in messages)
        {
            // if the message is older than 7 days or the 'IsDeleted' flag is set true, remove the message from the board
            if(message.IsDeleted || message.CreatedAt > now.AddDays(7))
            {
                RemoveMessage(message.Id);
            }

            // construct a CombinedListMessage based on database model

            CombinedListMessage msgRes = new CombinedListMessage
            {
                MessageType = (int)message.MessageType,
                MessageBody = XmlUtil.SerializeXml(new Schema.Message 
                {
                    MessageID = message.Id,
                    ConversationID = message.ConversationID ?? 0,
                    ReplyToMessageID = message.ParentMessageId,
                    Creator = message.FromViking!.Uid.ToString() ?? "NotFound",
                    CreateTime = message.CreatedAt,
                    UpdateDate = message.LastUpdatedAt,
                    MessageType = message.MessageType,
                    MessageLevel = message.MessageLevel,
                    Content = message.Data ?? "There's nothing in this message! Please report this error to the server admins or devs.",
                    DisplayAttribute = "C=White" // hardcoding white for now
                }),
                MessageDate = message.CreatedAt
            };

            // if the message has a replies list, add those to response

            if(message.Replies != null)
            {
                foreach(Model.Message replyMessage in message.Replies)
                {
                    if (replyMessage.Viking == viking) continue; // this is in the original implementation, i forgot what it was for, might remove

                    CombinedListMessage replyRes = new CombinedListMessage
                    {
                        MessageType = (int)replyMessage.MessageType,
                        MessageBody = XmlUtil.SerializeXml(new Schema.Message 
                        {
                            MessageID = replyMessage.Id,
                            ConversationID = replyMessage.ConversationID ?? 0,
                            ReplyToMessageID = replyMessage.ParentMessageId,
                            Creator = replyMessage.FromViking!.Uid.ToString() ?? "NotFound",
                            CreateTime = replyMessage.CreatedAt,
                            UpdateDate = replyMessage.LastUpdatedAt,
                            MessageType = replyMessage.MessageType,
                            MessageLevel = replyMessage.MessageLevel,
                            Content = replyMessage.Data ?? "There's nothing in this message! Please report this error to the server admins or devs.",
                            DisplayAttribute = "C=White" // hardcoding white for now
                        }),
                        MessageDate = replyMessage.CreatedAt
                    };

                    response.Add(replyRes);
                }
            }
            
            response.Add(msgRes);
        }

        // TODO - announcements, data messages (if above code doesn't add them)

        // sort response by newest first
        List<CombinedListMessage> sortedList = response.OrderBy(e => e.MessageDate).ToList();
        sortedList.Reverse();

        // return response as an array
        return response.ToArray();
    }

    public MessageInfo[] ConstructUserMessageInfoArray(Viking viking, bool showOldMessages, bool showDeletedMessages)
    {
        // get all recent messages from viking message board
        List<Model.Message> recentMessages = viking.MessageBoard.OrderBy(e => e.CreatedAt)
            .OrderBy(e => e.QueueID)
            .ToList();

        // make a new MessageInfo list for the response
        List<MessageInfo> response = new List<MessageInfo>();

        // get current DateTime
        DateTime now = DateTime.UtcNow;

        foreach(Model.Message message in recentMessages)
        {
            if(!showOldMessages && message.IsDeleted) continue;
            if(!message.IsNew) continue;
            if(message.MessageType == MessageType.Announcement) continue; // no return viking
            if(message.CreatedAt > now.AddMinutes(30) && message.IsNew) continue; // do not add messages that are older than 30 minutes even if IsNew was marked
            response.Add(new MessageInfo 
            {
                MessageID = message.Id,
                UserMessageQueueID = message.QueueID,
                FromUserID = message.FromViking!.Uid.ToString() ?? "NotFound",
                MessageTypeID = (int)message.MessageTypeID,
                Data = message.Data ?? "NoData",
                MemberMessage = message.MemberMessage ?? "NoData",
                NonMemberMessage = message.NonMemberMessage ?? "NoData"
            });
        }

        return response.ToArray();
    }
}

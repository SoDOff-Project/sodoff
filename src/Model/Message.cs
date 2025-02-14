using System;
using System.ComponentModel.DataAnnotations;
using sodoff.Model;
using sodoff.Schema;

namespace sodoff.Model;

public class Message
{
    [Key]
    public int Id { get; set; }
    public int VikingId { get; set; }
    public int ToVikingId { get; set; }

    public int? ParentMessageId { get; set; }

    public int QueueID { get; set; }
    public int? ConversationID { get; set; }

    public MessageType MessageType { get; set; }
    public MessageTypeID MessageTypeID { get; set; }
    public MessageLevel MessageLevel { get; set; }

    public string? Data { get; set; }
    public string? MemberMessage { get; set; }
    public string? NonMemberMessage { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
    public bool IsNew { get; set; }

    public virtual Viking? Viking { get; set; }
    public virtual Viking? ToViking { get; set; }
    public virtual Message? ParentMessage { get; set; }
    public virtual ICollection<Message>? Replies { get; set; }
}

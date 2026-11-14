using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.Notification;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string RecipientUserId { get; set; } = null!;

        public NotificationType Type { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; } = null!;

        public int? OrderId { get; set; }

        public int? SupportTicketId { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAtUtc { get; set; }

        [ForeignKey(nameof(RecipientUserId))]
        public ApplicationUser RecipientUser { get; set; } = null!;

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }

        [ForeignKey(nameof(SupportTicketId))]
        public SupportTicket? SupportTicket { get; set; }
    }
}

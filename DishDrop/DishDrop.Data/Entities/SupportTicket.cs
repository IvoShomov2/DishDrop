using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.SupportTicket;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("SupportTicket")]
    public class SupportTicket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string CreatedByUserId { get; set; } = null!;

        public int? OrderId { get; set; }

        public SupportCategory Category { get; set; }

        [Required]
        [MaxLength(SubjectMaxLength)]
        public string Subject { get; set; } = null!;

        public SupportStatus Status { get; set; }

        [MaxLength(UserIdMaxLength)]
        public string? AssignedAdminId { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(CreatedByUserId))]
        public ApplicationUser CreatedByUser { get; set; } = null!;

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }

        [ForeignKey(nameof(AssignedAdminId))]
        public ApplicationUser? AssignedAdmin { get; set; }

        public ICollection<SupportTicketMessage> Messages { get; set; } = new List<SupportTicketMessage>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

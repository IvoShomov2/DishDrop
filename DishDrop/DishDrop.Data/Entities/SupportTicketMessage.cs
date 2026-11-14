using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DishDrop.Data.DataConstants.SupportTicketMessage;
using static DishDrop.Data.DataConstants.Identity;

namespace DishDrop.Data.Entities
{
    [Table("SupportTicketMessage")]
    public class SupportTicketMessage
    {
        [Key]
        public int Id { get; set; }

        public int SupportTicketId { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string AuthorUserId { get; set; } = null!;

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(SupportTicketId))]
        public SupportTicket SupportTicket { get; set; } = null!;

        [ForeignKey(nameof(AuthorUserId))]
        public ApplicationUser AuthorUser { get; set; } = null!;
    }
}

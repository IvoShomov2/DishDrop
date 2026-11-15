using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using static DishDrop.Data.DataConstants.ApplicationUser;

namespace DishDrop.Data.Entities
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

        public ICollection<RestaurantStaff> RestaurantMemberships { get; set; } = new List<RestaurantStaff>();

        public CourierProfile? CourierProfile { get; set; }

        public ICollection<RestaurantApprovalHistory> RestaurantApprovalChanges { get; set; } = new List<RestaurantApprovalHistory>();

        public ICollection<CourierApprovalHistory> CourierApprovalChanges { get; set; } = new List<CourierApprovalHistory>();

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<OrderStatusHistory> OrderStatusChanges { get; set; } = new List<OrderStatusHistory>();

        public ICollection<Payment> CollectedPayments { get; set; } = new List<Payment>();

        public ICollection<DeliveryStatusHistory> DeliveryStatusChanges { get; set; } = new List<DeliveryStatusHistory>();

        public ICollection<ReviewReply> ReviewReplies { get; set; } = new List<ReviewReply>();

        public ICollection<SupportTicket> CreatedSupportTickets { get; set; } = new List<SupportTicket>();

        public ICollection<SupportTicket> AssignedSupportTickets { get; set; } = new List<SupportTicket>();

        public ICollection<SupportTicketMessage> SupportMessages { get; set; } = new List<SupportTicketMessage>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

        public ICollection<IdentityUserClaim<string>> UserClaims { get; set; } = new List<IdentityUserClaim<string>>();

        public ICollection<IdentityUserLogin<string>> UserLogins { get; set; } = new List<IdentityUserLogin<string>>();

        public ICollection<IdentityUserToken<string>> UserTokens { get; set; } = new List<IdentityUserToken<string>>();

        public ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DishDrop.Data.Enums;
using static DishDrop.Data.DataConstants.Restaurant;

namespace DishDrop.Data.Entities
{
    [Table("Restaurant")]
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        public int CityId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(ImagePathMaxLength)]
        public string? ImagePath { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsAcceptingOrders { get; set; }

        [Column(TypeName = DataConstants.Money.SqlType)]
        [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
        public decimal MinimumOrderAmount { get; set; }

        public int EstimatedPreparationMinutes { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [ForeignKey(nameof(CityId))]
        public City City { get; set; } = null!;

        public ICollection<RestaurantStaff> Staff { get; set; } = new List<RestaurantStaff>();

        public ICollection<RestaurantWorkingPeriod> WorkingPeriods { get; set; } = new List<RestaurantWorkingPeriod>();

        public ICollection<RestaurantScheduleException> ScheduleExceptions { get; set; } = new List<RestaurantScheduleException>();

        public ICollection<RestaurantDeliveryArea> RestaurantDeliveryAreas { get; set; } = new List<RestaurantDeliveryArea>();

        public ICollection<RestaurantCuisineType> RestaurantCuisineTypes { get; set; } = new List<RestaurantCuisineType>();

        public ICollection<RestaurantApprovalHistory> ApprovalHistory { get; set; } = new List<RestaurantApprovalHistory>();

        public ICollection<Menu> Menus { get; set; } = new List<Menu>();

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

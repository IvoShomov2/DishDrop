using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DishDrop.Data.Entities
{
    [Table("RestaurantWorkingPeriod")]
    public class RestaurantWorkingPeriod
    {
        [Key]
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly OpensAt { get; set; }

        public TimeOnly ClosesAt { get; set; }

        public bool ClosesNextDay { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DishDrop.Data.Entities
{
    [Table("RestaurantScheduleExceptionPeriod")]
    public class RestaurantScheduleExceptionPeriod
    {
        [Key]
        public int Id { get; set; }

        public int RestaurantScheduleExceptionId { get; set; }

        public TimeOnly OpensAt { get; set; }

        public TimeOnly ClosesAt { get; set; }

        public bool ClosesNextDay { get; set; }

        [ForeignKey(nameof(RestaurantScheduleExceptionId))]
        public RestaurantScheduleException RestaurantScheduleException { get; set; } = null!;
    }
}

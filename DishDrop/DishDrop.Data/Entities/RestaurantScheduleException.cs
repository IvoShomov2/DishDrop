using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DishDrop.Data.Entities
{
    [Table("RestaurantScheduleException")]
    public class RestaurantScheduleException
    {
        [Key]
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public DateOnly Date { get; set; }

        public bool IsClosed { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<RestaurantScheduleExceptionPeriod> Periods { get; set; } = new List<RestaurantScheduleExceptionPeriod>();
    }
}

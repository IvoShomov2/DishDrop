using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DishDrop.Data.Entities
{
    [Table("RestaurantDeliveryArea")]
    public class RestaurantDeliveryArea
    {
        public int RestaurantId { get; set; }

        public int DeliveryAreaId { get; set; }

        [Column(TypeName = DataConstants.Money.SqlType)]
        [Precision(DataConstants.Money.Precision, DataConstants.Money.Scale)]
        public decimal DeliveryFee { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        [ForeignKey(nameof(DeliveryAreaId))]
        public DeliveryArea DeliveryArea { get; set; } = null!;
    }
}

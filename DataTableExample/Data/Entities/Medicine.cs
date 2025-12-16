using System.ComponentModel.DataAnnotations;

namespace DataTableExample.Data.Entities
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string ActiveIngredient { get; set; }

        [MaxLength(100)]
        public string Manufacturer { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}

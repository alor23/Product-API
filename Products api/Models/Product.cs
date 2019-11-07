using System;
using System.ComponentModel.DataAnnotations;

namespace Products_api.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        [Required]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        public decimal Price { get; set; }

    }
}

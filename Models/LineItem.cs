using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Models
{
    public class LineItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        [MaxLength(30)]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double LineTotal { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string SupplierName { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime DateReceived { get; set; }
        public double Total { get; set; }
        [MaxLength(20)]
        public string InvoiceNumber { get; set; }
        public List<LineItem> LineItems { get; set; }
    }
}

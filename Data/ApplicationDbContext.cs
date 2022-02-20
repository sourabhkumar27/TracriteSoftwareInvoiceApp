using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using InvoiceApp.Models;

namespace InvoiceApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<InvoiceApp.Models.Invoice> Invoice { get; set; }
        public DbSet<InvoiceApp.Models.LineItem> LineItem { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ContactUs.Models
{

    public class ContactContext : DbContext
    {

        public ContactContext(DbContextOptions<ContactContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }

    }

}
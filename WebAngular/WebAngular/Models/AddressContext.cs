using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebAngular.Models
{
    public class AddressContext : DbContext
    {
        public AddressContext()
            :base("DbConnection")
        { }

        public DbSet<Addres> Address { get; set; }
    }
}
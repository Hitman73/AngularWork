﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAngular
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DBAddressEntities : DbContext
    {
        public DBAddressEntities()
            : base("name=DBAddressEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Addres> Addres { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Street> Street { get; set; }           
    
        public virtual ObjectResult<genRecord_Result> genRecord()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<genRecord_Result>("genRecord");
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerManagement.Model.Entity
{
    using ServerManagement.VML;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ServerManagementEntities : DbContext
    {
        public ServerManagementEntities()
            : base(Utils.GetEntityConnectionString())
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MacAddress> MacAddresses { get; set; }
        public virtual DbSet<IP> IPs { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
    }
}

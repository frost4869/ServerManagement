//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerManagement.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class IP
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public int ServerId { get; set; }
        public Nullable<int> MacAddressId { get; set; }
        public bool Active { get; set; }
    
        public virtual MacAddress MacAddress { get; set; }
        public virtual Server Server { get; set; }
    }
}

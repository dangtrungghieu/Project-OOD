﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_OOD.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QL_ThueXeEntities : DbContext
    {
        public QL_ThueXeEntities()
            : base("name=QL_ThueXeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GOPY> GOPY { get; set; }
        public virtual DbSet<LOAIXE> LOAIXE { get; set; }
        public virtual DbSet<MUONXE> MUONXE { get; set; }
        public virtual DbSet<NGUOIDUNG> NGUOIDUNG { get; set; }
        public virtual DbSet<QUYENTRUYCAP> QUYENTRUYCAP { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TRAM> TRAM { get; set; }
        public virtual DbSet<XE> XE { get; set; }
    }
}

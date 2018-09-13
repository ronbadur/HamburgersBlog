﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using HamburgersBlog.Models;

namespace HamburgersBlog.DAL
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() : base("ProjectContext") { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Princess> Princesses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Resturant> Resturants { get; set; }
        public DbSet<Hamburger> Hamburgers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
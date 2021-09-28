using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Text;

namespace Flex.Tests
{
    class TheContext : DbContext
    {
        public TheContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public virtual DbSet<MovieRecord> Movies
        {
            get;
            set;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

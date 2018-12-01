using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sqlite.Model;
using System;

namespace Sqlite.EF
{
    public class DatabaseContext : DbContext
    {
        private readonly string pathToDb;

        public DatabaseContext(string pathToDb)
        {
            this.pathToDb = pathToDb;
        }

        public DbSet<TableEntry> Entries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(String.Format("Data source={0}", pathToDb));
            optionsBuilder.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
        }
    }
}
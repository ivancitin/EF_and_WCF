using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Sqlite.EF;

namespace Sqlite.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20181130090000_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("Sqlite.Model.TableEntry", b =>
            {
                b.ToTable("TableEntry");

                b.Property<string>("Json");
                b.Property<long>("Id").ValueGeneratedOnAdd();

                b.HasKey("Json");
                b.HasKey("Id");
            });
        }
    }
}
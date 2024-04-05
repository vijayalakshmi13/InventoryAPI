using azure.Model;
using Microsoft.EntityFrameworkCore;

namespace azure.Data
{
    public class InventoryDbContext : DbContext
    {

        public InventoryDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<InventoryItems> InventoryItems { get; set; }
        public DbSet<ItemsSoldInfo> ItemsSoldInfo { get; set; }
        public DbSet<Person> Person { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=10.126.27.240;Database=PRACTISE_DB;User ID=PRACTISE;Password=Password@2023;MultipleActiveResultSets=true");
        //}


    }
}

using AdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AdminSystem.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ): base(options){}

    public DbSet<Customer> customer { get; set; }
    public DbSet<Order> order { get; set; }

}

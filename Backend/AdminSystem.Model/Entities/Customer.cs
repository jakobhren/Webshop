using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminSystem.Model.Entities;


public class Customer
{
    public Customer() {}
    public Customer(int id){CustomerId = id;}
    
    [Column("customerid")] 
    public int CustomerId { get; set; }
    [Column("email")] 
    public string Email { get; set; }
     [Column("password")] 
    public string Password { get; set; }
     [Column("name")] 
    public string Name { get; set; }

    [Column("creation")]
    public DateTime Creation { get; set; }

}



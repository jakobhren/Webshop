namespace AdminSystem.Model.Entities;

public class Order
{
    public Order(int id){OrderId = id;}
    public Order() {}
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public double OrderTotal { get; set; }
    public string CustomerEmail { get; set; }
     // Navigation property to OrderItems
    public List<OrderItem> OrderItems { get; set; }
}

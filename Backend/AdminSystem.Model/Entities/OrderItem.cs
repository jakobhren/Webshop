namespace AdminSystem.Model.Entities;

public class OrderItem
{
public OrderItem(int id){OrderItemId = id;}
public OrderItem() { }
public int OrderItemId { get; set; }
public int OrderId { get; set; }
public int EbookId { get; set; }


// Navigation property to Book
    public Ebook Ebook { get; set; }

}

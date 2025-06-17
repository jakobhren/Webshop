namespace AdminSystem.Model.Entities;

public class Category
{
    public Category(int id){CategoryId = id;}
public int CategoryId { get; set; }
public string CategoryName{ get; set; }
}

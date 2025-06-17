namespace AdminSystem.Model.Entities;

public class Ebook
{
public Ebook(int id){EbookId = id;}
public Ebook() { }
public int EbookId { get; set; }
public string Title { get; set; }
public string Author { get; set; }
public int PublicationYear { get; set; }
public decimal Price { get; set; }
public string File_url { get; set; }
public string Image_url { get; set; }
public int CategoryId { get; set; }

}
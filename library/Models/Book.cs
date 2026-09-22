namespace library.Models;

public class Book : Product
{
    public string Author { get; set; } = "";

    public string ISBN { get; set; } = "";

    public string Category { get; set; } = "";

    public override string GetDetails()
    {
        return
            $"Type     : Book\n" +
            $"ID       : {Id}\n" +
            $"Title    : {Title}\n" +
            $"Author   : {Author}\n" +
            $"ISBN     : {ISBN}\n" +
            $"Category : {Category}\n" +
            $"Price    : RM {Price:F2}\n" +
            $"Stock    : {Stock}";
    }
}
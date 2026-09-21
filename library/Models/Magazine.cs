namespace library.Models;

public class Magazine : Product
{
    public string Publisher { get; set; } = "";

    public int IssueNumber { get; set; }

    public DateOnly PublishedDate { get; set; }

    public override string GetDetails()
    {
        return
            $"Type           : Magazine\n" +
            $"ID             : {Id}\n" +
            $"Title          : {Title}\n" +
            $"Publisher      : {Publisher}\n" +
            $"Issue Number   : {IssueNumber}\n" +
            $"Published Date : {PublishedDate:yyyy-MM-dd}\n" +
            $"Price          : RM {Price:F2}\n" +
            $"Stock          : {Stock}";
    }
}
namespace ServerV12.Types;

public class Query
{
    public Book GetBook()
        => new Book("C# in depth.", "Jon Skeet");
}
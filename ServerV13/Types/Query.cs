namespace ServerV13.Types;

[QueryType]
public class Query
{
    public Book GetBook()
        => new Book("C# in depth", "Jon Skeet");
}
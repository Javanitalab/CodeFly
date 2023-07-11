namespace DataAccess;

public class PagingModel
{
    public int? Id { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string Sort { get; set; }

    public SortDirection SortDirection { get; set; } = SortDirection.ASC;
    
    
}

public enum SortDirection
{
    ASC = 1,
    DESC = 2,
}

namespace BearerAuthDemo;

/// <summary>
/// Supports page-based pagination
/// </summary>
public class Pagination
{
    public const string HeaderKey = "X-Query-Pagination";

    /// <summary>
    /// The page number to retrieve (page numbers start at 1)
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The requested number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The actual number of items returned in the response
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// The total number of items in the data set
    /// </summary>
    public int ItemTotal { get; set; }

    /// <summary>
    /// The total number of pages in the data set
    /// </summary>
    public int PageCount => (int)System.Math.Ceiling((double)ItemTotal / PageSize);

    /// <summary>
    /// True if this is not the last page
    /// </summary>
    public bool More => Page < PageCount;
}
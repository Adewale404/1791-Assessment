namespace LoadExpressApi.Application.Common.Models;

public class PaginationFilter : BaseFilter
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; } = int.MaxValue;

    public string[]? OrderBy { get; set; }

    public string? Search { get; set; }
    public string? Filter { get; set; }
}

public static class PaginationFilterExtensions
{
    public static bool HasOrderBy(this PaginationFilter filter) =>
        filter.OrderBy?.Any() is true;
}
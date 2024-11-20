namespace Domain.Models.Base;

public class PaginatedResult<T>
{
    public List<T> Data { get; set; } = [];
    public T? Current;
    
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}
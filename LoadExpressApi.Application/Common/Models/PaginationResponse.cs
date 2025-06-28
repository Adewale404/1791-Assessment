using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LoadExpressApi.Application.Common.Models;

public class PaginationResponse<T> : Result
{
    public PaginationResponse(List<T> data, int count, int page, int pageSize)
    {
        Data = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public PaginationResponse()
    {
    }

    public PaginationResponse(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
    {
        Data = data;
        CurrentPage = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Messages = messages;
    }

    public List<T> Data { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public static PaginationResponse<T> Failure(List<string> messages)
    {
        return new(false, default, messages);
    }

    public static PaginationResponse<T> Success(List<T> data, int count, int page, int pageSize)
    {
        return new(true, data, null, count, page, pageSize);
    }

    public new static PaginationResponse<T> Fail()
    {
        return new() { Succeeded = false };
    }

    public new static PaginationResponse<T> Fail(string message)
    {
        return new() { Succeeded = false, Messages = new List<string> { message } };
    }

    public static ErrorResult<T> ReturnError(string message)
    {
        return new() { Succeeded = false, Messages = new List<string> { message }, StatusCode = 500 };
    }

    public new static PaginationResponse<T> Fail(List<string> messages)
    {
        return new() { Succeeded = false, Messages = messages };
    }

    public static ErrorResult<T> ReturnError(List<string> messages)
    {
        return new() { Succeeded = false, Messages = messages, StatusCode = 500 };
    }

    public new static Task<PaginationResponse<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public new static Task<PaginationResponse<T>> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Task<ErrorResult<T>> ReturnErrorAsync(string message)
    {
        return Task.FromResult(ReturnError(message));
    }

    public new static Task<PaginationResponse<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public static Task<ErrorResult<T>> ReturnErrorAsync(List<string> messages)
    {
        return Task.FromResult(ReturnError(messages));
    }

    public new static PaginationResponse<T> Success()
    {
        return new() { Succeeded = true };
    }

    public new static PaginationResponse<T> Success(string message)
    {
        return new() { Succeeded = true, Messages = new List<string> { message } };
    }

    public new static PaginationResponse<T> Success(List<string> messages)
    {
        return new() { Succeeded = true, Messages = messages };
    }

    public static PaginationResponse<T> Success(List<T> data)
    {
        return new() { Succeeded = true, Data = data };
    }

    public static PaginationResponse<T> Success(List<T> data, string message)
    {
        return new() { Succeeded = true, Data = data, Messages = new List<string> { message } };
    }

    public static PaginationResponse<T> Success(List<T> data, List<string> messages)
    {
        return new() { Succeeded = true, Data = data, Messages = messages };
    }

    public new static Task<PaginationResponse<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public new static Task<PaginationResponse<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public new static Task<PaginationResponse<T>> SuccessAsync(List<string> messages)
    {
        return Task.FromResult(Success(messages));
    }

    public static Task<PaginationResponse<T>> SuccessAsync(List<T> data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<PaginationResponse<T>> SuccessAsync(List<T> data, string message)
    {
        return Task.FromResult(Success(data, message));
    }

    public static Task<PaginationResponse<T>> SuccessAsync(List<T> data, List<string> messages)
    {
        return Task.FromResult(Success(data, messages));
    }
}

public static class MapperExtensions
{

    public static async Task<PaginationResponse<T>> ToPaginatedResultListAsync<T>(this IQueryable<T> query, int pageIndex, int limit, string? sortColumn = null) where T : class
    {
        pageIndex = pageIndex == 0 ? 1 : pageIndex;
        limit = limit == 0 ? 10 : limit;
        int totalCount;
        try
        {
            totalCount = await query.AsNoTracking().CountAsync();
        }
        catch (InvalidOperationException)
        {
            var list = query.ToList();
            query = list.AsQueryable();
            totalCount = list.Count;
        }

        var collection = query;
        collection = collection.Skip((pageIndex - 1) * limit).Take(limit);
        ICollection<T> rows;
        try
        {
            rows = await collection.ToListAsync();
        }
        catch (InvalidOperationException)
        {
            rows = collection.ToList();
        }

        return new PaginationResponse<T>(true, rows.ToList(), null, totalCount, pageIndex, limit);
    }
    public static async Task<PaginationResponse<TDto>> ToMappedPaginatedResultAsync<T, TDto>(
        this IQueryable<T> query, int pageNumber, int pageSize)
        where T : class
    {
        var converter = new MappedPaginatedResultConverter<T, TDto>(pageNumber, pageSize);
        return await converter.ConvertBackAsync(query);
    }

    public static async Task<PaginationResponse<TDto>> ToMappedPaginatedResultAsync<T, TDto>(
       this IList<T> query, int pageNumber, int pageSize)
       where T : class
    {
        var converter = new MappedPaginatedResultConverter<T, TDto>(pageNumber, pageSize);
        return await converter.ConvertBackAsync(query);
    }

    public class MappedPaginatedResultConverter<T, TDto> : IMapsterConverterAsync<PaginationResponse<TDto>, IQueryable<T>>
        where T : class
    {
        private int _pageNumber;
        private int _pageSize;

        public MappedPaginatedResultConverter(int pageNumber, int pageSize)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
        }

        public Task<IQueryable<T>> ConvertAsync(PaginationResponse<TDto> item)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<TDto>> ConvertBackAsync(IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            _pageNumber = _pageNumber == 0 ? 1 : _pageNumber;
            _pageSize = _pageSize == 0 ? 10 : _pageSize;
            List<string> columnValues = new();

            int count = await query.AsNoTracking().CountAsync();
            _pageNumber = _pageNumber <= 0 ? 1 : _pageNumber;
            var items = await query.Skip((_pageNumber - 1) * _pageSize).Take(_pageSize).AsNoTracking().ToListAsync();
            var mappedItems = items.Adapt<List<TDto>>();
            return await Task.FromResult(PaginationResponse<TDto>.Success(mappedItems, count, _pageNumber, _pageSize));
        }
        public async Task<PaginationResponse<TDto>> ConvertBackAsync(IList<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            _pageNumber = _pageNumber == 0 ? 1 : _pageNumber;
            _pageSize = _pageSize == 0 ? 10 : _pageSize;
            int count = query.Count;
            _pageNumber = _pageNumber <= 0 ? 1 : _pageNumber;
            var items = query.Skip((_pageNumber - 1) * _pageSize).Take(_pageSize).ToList();
            var mappedItems = items.Adapt<List<TDto>>();
            return await Task.FromResult(PaginationResponse<TDto>.Success(mappedItems, count, _pageNumber, _pageSize));
        }



    }
}
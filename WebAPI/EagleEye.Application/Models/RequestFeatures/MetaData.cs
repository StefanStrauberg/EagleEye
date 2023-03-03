using System.Text.Json;

namespace WebAPI.EagleEye.Application.Models.RequestFeatures
{
    public class MetaData
    {
        private int _currentPage;
        private int _totalPages;
        private int _pageSize;
        private int _totalCount;
        private long _countOfItemsByFilter;
        public MetaData(int CurrentPage,
                        int TotalPages,
                        int PageSize,
                        int TotalCount,
                        long CountOfItemsByFilter)
        {
            _currentPage = CurrentPage;
            _totalPages = TotalPages;
            _pageSize = PageSize;
            _totalCount = TotalCount;
            _countOfItemsByFilter = CountOfItemsByFilter;
        }

        public int CurrentPage { get => _currentPage; }
        public int TotalPages { get => _totalPages; }
        public int PageSize { get => _pageSize; }
        public int TotalCount { get => _totalCount; }
        public long CountOfItemsByFilter { get => _countOfItemsByFilter; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public override string ToString()
            => JsonSerializer.Serialize(this);
    }
}
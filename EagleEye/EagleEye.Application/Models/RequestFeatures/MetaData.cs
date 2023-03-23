using System.Text.Json;

namespace WebAPI.EagleEye.Application.Models.RequestFeatures
{
    public class MetaData
    {
        private int _currentPage;
        private int _totalPages;
        private int _pageSize;
        private long _totalCount;

        public MetaData(int CurrentPage,
                        int TotalPages,
                        int PageSize,
                        long TotalCount)
        {
            _currentPage = CurrentPage;
            _totalPages = TotalPages;
            _pageSize = PageSize;
            _totalCount = TotalCount;
        }

        public int CurrentPage { get => _currentPage; }
        public int TotalPages { get => _totalPages; }
        public int PageSize { get => _pageSize; }
        public long TotalCount { get => _totalCount; }
        public bool HasPrevious => _currentPage > 1;
        public bool HasNext => _currentPage < _totalPages;

        public override string ToString()
            => JsonSerializer.Serialize(this);
    }
}
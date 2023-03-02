using System;
namespace EagleEye.Application.RequestFeatures
{
    public class QueryParameters
    {
        const int maxPageSize = 100; 
        public int PageNumber { get; set; } = 1; 
        private int _pageSize = 50; 
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value; 
        }
        private static DateTime _defaultSearchDate = DateTime.Now;
        private DateTime _minSearchDate = _defaultSearchDate;
        public DateTime MinSearchDate 
        { 
            get => _minSearchDate;
            set => _minSearchDate = (value > _maxSearchDate) ? _minSearchDate : value;
        }
        private DateTime _maxSearchDate = _defaultSearchDate;
        public DateTime MaxSearchDate 
        { 
            get => _maxSearchDate; 
            set => _maxSearchDate = (value > _maxSearchDate && value <= _minSearchDate) ? _maxSearchDate : value;
        }
    }
}
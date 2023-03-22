using System;

namespace WebAPI.EagleEye.Application.Models.RequestFeatures
{
    public class QueryParameters
    {
        private const int maxPageSize = 100;
        private const int defPageNumber = 1;
        private static DateTime _defDate = DateTime.Now;

        private int _pageNumber;
        public int PageNumber 
        { 
            get => _pageNumber; 
            set => _pageNumber = (value < 0) ? defPageNumber : value; 
        } 
        
        private int _pageSize = 50; 
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value; 
        }
        
        private DateTime _minSearchDate = _defDate;
        public DateTime MinSearchDate 
        { 
            get => _minSearchDate;
            set => _minSearchDate = (value > _maxSearchDate) ? new DateTime(_defDate.Year,
                                                                            _defDate.Month,
                                                                            _defDate.Day,
                                                                            0,
                                                                            0,
                                                                            0) : value;
        }
        
        private DateTime _maxSearchDate = _defDate;
        public DateTime MaxSearchDate 
        { 
            get => _maxSearchDate; 
            set => _maxSearchDate = (value > _defDate && value < _minSearchDate) ? _defDate : value;
        }
    }
}
using EagleEye.Application.RequestFeatures;
using System;
using System.Linq;

namespace EagleEye.Application.Paging
{
    public class PagedList
    {
        public MetaData MetaData { get; set; }
        public string data { get; set; }
        public PagedList(string items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            data = items;
        }
    }
}
using EagleEye.Application.RequestFeatures;
using System;

namespace EagleEye.Application.Paging
{
    public class PagedList
    {
        public MetaData MetaData { get; }
        public string data { get; }
        public PagedList(string jsonString, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData(TotalCount: count,
                                    PageSize: pageSize,
                                    CurrentPage: pageNumber,
                                    TotalPages: (int)Math.Ceiling(count / (double)pageSize));
            data = jsonString;
        }
    }
}
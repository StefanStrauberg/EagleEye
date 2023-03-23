using System;
using WebAPI.EagleEye.Application.Models.RequestFeatures;

namespace WebAPI.EagleEye.Application.Models.Paging
{
    public class PagedList
    {
        public MetaData MetaData { get; }
        public string data { get; }
        public PagedList(string jsonString, long countGetItems, int pageNumber, int pageSize)
        {
            MetaData = new MetaData(TotalCount: countGetItems,
                                    PageSize: pageSize,
                                    CurrentPage: pageNumber,
                                    TotalPages: (int)Math.Ceiling(countGetItems / (double)pageSize));
            data = jsonString;
        }
    }
}
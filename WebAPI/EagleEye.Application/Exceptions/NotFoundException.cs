using System;

namespace WebAPI.EagleEye.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string collectionName, string key) 
            : base($"Object from Collection:{collectionName} with ID:{key} was not found.")
        {
        }
    }
}
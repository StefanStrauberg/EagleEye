using System;

namespace WebAPI.EagleEye.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, string collectionName, object key) 
            : base($"{DateTime.Now} Object:{name} from collection:{collectionName} with ID:{key} was not found.")
        {
            
        }
    }
}
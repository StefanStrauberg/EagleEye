using System;

namespace EagleEye.Domain
{
    public class DomainBaseEntity
    {
        public Guid Id { get; set; }
        public dynamic Data { get; set; }
    }
}
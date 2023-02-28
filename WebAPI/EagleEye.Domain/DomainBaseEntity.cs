using System;
using System.Text.Json.Nodes;

namespace EagleEye.Domain
{
    public class DomainBaseEntity
    {
        public Guid Id { get; set; }
        public JsonObject Data { get; set; }
    }
}
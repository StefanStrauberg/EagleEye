using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public abstract class ReciverParams
    {
        public ParserDelegate parse { get; set; }
    }
}
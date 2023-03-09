using System.Threading.Tasks;

namespace FGLogDog.Application.Contracts
{
    public interface ISender
    {
        Task StartSender(string message);
    }
}
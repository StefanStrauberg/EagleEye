using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Models.Email;

namespace WebAPI.EagleEye.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(EmailMessage email);
    }
}
using Arasan.Models;
using System.Data;
namespace Arasan.Interface
{
    public interface IMailService
    {
        void SendEmailAsync(MailRequest mailRequest);

    }
}

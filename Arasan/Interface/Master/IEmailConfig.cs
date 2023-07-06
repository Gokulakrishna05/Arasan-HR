using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IEmailConfig
    {
        IEnumerable<EmailConfig> GetAllEmailConfig(string status);
        string EmailConfigCRUD(EmailConfig ss);
        DataTable GetEmailConfig(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
    }
}

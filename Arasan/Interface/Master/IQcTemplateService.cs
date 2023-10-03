using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;
using MimeKit.Cryptography;

namespace Arasan.Interface.Master
{
    public interface IQcTemplateService
    {
        IEnumerable<QcTemplate> GetAllQcTemplate();
        DataTable GetDesc();
        DataTable GetQCTestDetails(string itemId);
        string QcTemplateCRUD(QcTemplate cy);
    }
}

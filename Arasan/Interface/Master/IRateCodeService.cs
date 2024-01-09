using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
namespace Arasan.Interface.Master
{
    public interface IRateCodeService
    {
        DataTable GetAllListRateCodeItem(string strStatus);
        DataTable GetEditRateCode(string id);
        string RateCodeCRUD(RateCode cy);
        string StatusChange(string tag, string id);
    }
}

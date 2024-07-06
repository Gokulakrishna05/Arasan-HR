using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Master
{
    public interface IRateService
    {
        DataTable GetAllListRateItem(string strStatus);
        DataTable GetRateCode();
        string RateCRUD(Rate cy);
        DataTable GetItemDetails(string itemId);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetEditRate(string id);
        DataTable GetEditRateDeatil(string id);
        DataTable GetEditRateDeatils(string id);
        string RateCodeCRUD(string Ratecode, string RateDsc);
        string RateDetailCRUD(Rate cy);
        string RateRevisionCRUD(Rate cy);
    }
}

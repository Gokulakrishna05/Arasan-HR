using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Interface
{
    public interface IPayPeriodService
    {
        public string PayPeriodCRUD(PayPeriod pp);
        DataTable GetEditPayPeriod(string id);
        DataTable GetPayPeriod(string id);
        DataTable GetAlLPayPeriod(string strStatus);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
    }
}

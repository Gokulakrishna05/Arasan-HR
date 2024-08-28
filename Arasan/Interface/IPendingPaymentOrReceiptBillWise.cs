 
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface
{
    public interface IPendingPaymentOrReceiptBillWise
    {
        DataTable GetAllPendingPaymentOrReceiptBillWise(string dtFrom, string Branch);
    }
}

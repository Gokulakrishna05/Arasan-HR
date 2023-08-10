using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Sales

{
    public interface IDebitNoteBillService
    {
        IEnumerable<DebitNoteBill> GetAllDebitNoteBill();
        string DebitNoteBillCRUD(DebitNoteBill cy);
        DataTable GetParty();
        DataTable GetGrn();
        DataTable GetItemDetails(string itemId);
        //DataTable GetItemDetails(string itemId);
    }
}

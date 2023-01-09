using Arasan.Interface;
using System.Collections.Generic;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Arasan.Interface
{
    public interface IQCTestingService
    {
        
        DataTable GetGRN(string id);
        DataTable GetPO(string id);
        DataTable GetGRNDetails(string id);
        DataTable GetPODetails(string id);
        DataTable GetItembyId(string id);
        DataTable GetPOItembyId(string id);
        DataTable GetQCDetail(string id);
        DataTable GetParty(string id);
        DataTable GetPOParty(string id);
        string QCTestingCRUD(QCTesting cy);
        //DataTable GetPO(string id);
        DataTable GetQCTesting(string id);
        IEnumerable<QCTesting> GetAllQCTesting();

    }
}

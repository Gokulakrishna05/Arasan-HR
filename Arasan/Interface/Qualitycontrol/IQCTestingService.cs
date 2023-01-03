using Arasan.Interface;
using System.Collections.Generic;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Arasan.Interface
{
    public interface IQCTestingService
    {
        DataTable GetGRN();
        //DataTable GetGRN(string id);
        DataTable GetGRNDetails(string id);
        DataTable GetItembyId(string id);
        DataTable GetQCDetail(string id);
        DataTable GetParty(string id);
        string QCTestingCRUD(QCTesting cy);

        DataTable GetQCTesting(string id);
        IEnumerable<QCTesting> GetAllQCTesting();

    }
}

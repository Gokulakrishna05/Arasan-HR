using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{ 

public interface ISubContractingReport
    {
    DataTable GetAllSubContReport(string dtFrom, string dtTo);

}

}
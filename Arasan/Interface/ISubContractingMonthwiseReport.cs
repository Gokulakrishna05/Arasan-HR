using System.Data;
using Arasan.Models;
namespace Arasan.Interface { 
public interface ISubContractingMonthwiseReport
{
    DataTable GetAllSubContMonWisReport(string dtFrom, string dtTo);

}

}
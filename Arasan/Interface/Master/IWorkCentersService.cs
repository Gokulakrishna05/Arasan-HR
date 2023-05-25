using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master;

public interface IWorkCentersService
{
    DataTable GetSupplier();
    IEnumerable<WorkCenters> GetAllWorkCenters();
    DataTable GetWorkCenters(string id);
    DataTable GetWorkCentersDetail(string id);
    string WorkCentersCRUD(WorkCenters cy);
    string StatusChange(string tag, int id);
}

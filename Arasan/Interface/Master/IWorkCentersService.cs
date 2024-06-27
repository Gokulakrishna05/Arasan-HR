using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master;

public interface IWorkCentersService
{
    DataTable GetSupplier();

    DataTable GetWorkCenters(string id);

    DataTable GetWorkCentersDetail(string id);

    string WorkCentersCRUD(WorkCenters cy);
    string ProductionRateCRUD(WorkCenters cy);
    string RejdetCRUD(WorkCenters cy);
    string ProdCapCRUD(WorkCenters cy);
    string ProdCapPerCRUD(WorkCenters cy);
    string ApSiveCRUD(WorkCenters cy);
    string PasteRunCRUD(WorkCenters cy);

    string StatusChange(string tag, string id);
    string RemoveChange(string tag, string id);


    DataTable GetAllWorkCenters(string strStatus);
    DataTable GetContType();
}

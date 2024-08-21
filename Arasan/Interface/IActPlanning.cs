namespace Arasan.Interface;
using System.Data;
using Arasan.Models;

public interface IActPlanning
{
    DataTable GetMachine();
    DataTable GetAlloted();
    DataTable GetItem();
    string ActPlanningCRUD(ActPlanning cy);

    DataTable GetAllActPlanning(string strStatus);

    DataTable GetEditActPlan(string id);
    DataTable GetEditTool(string id);
    DataTable GetEditReason(string id);


    string StatusChange(string tag, string id);
    string RemoveChange(string tag, string id);


}


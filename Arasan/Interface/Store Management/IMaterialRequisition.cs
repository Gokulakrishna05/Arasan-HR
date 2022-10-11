using System.Data;
namespace Arasan.Interface.Store_Management
{
    public interface IMaterialRequisition
    {
        DataTable GetLocation();
        DataTable GetWorkCenter();
        DataTable GetItem(string value);
        DataTable GetItemGrp();
    }
}

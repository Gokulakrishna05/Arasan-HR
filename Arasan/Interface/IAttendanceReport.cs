using System.Data;

namespace Arasan.Interface
{
    public interface IAttendanceReport
    {
        DataTable GetAllAttendanceReportGrid();
    }
}

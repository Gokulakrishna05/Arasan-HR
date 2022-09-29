using System.Data;

namespace Arasan.Interface
{
    public interface IPurchaseEnqService
    {
        DataTable GetBranch();
        DataTable GetSupplier();
        DataTable GetCurency();
        DataTable GetItem();
    }
}

namespace Arasan.Interface;
using System.Data;
using Arasan.Models;

public interface IPayTransaction
    {
    string PayTransactionCRUD(PayTransaction cy);

    string StatusChange(string tag, string id);
    string RemoveChange(string tag, string id);
    DataTable GetPayCategory();
    DataTable GetPayCode();
    DataTable GetBranch();
    DataTable getPayTraId(string id);
    DataTable getPayTra(string id);

    DataTable GetAllPayTransaction(string strStatus);

    DataTable GetEditPayTra(string id);
    DataTable GetEditEPayTra(string id);


}


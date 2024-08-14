namespace Arasan.Interface;
using System.Data;
using System.Diagnostics.Contracts;
using Arasan.Models;


    public interface IResignation
    {

        DataTable GetEmpId();
    string ResignationCRUD(ResignationModel R);
    DataTable GetAllResignation(string strStatus);
    string StatusDelete(string tag, string id);
    DataTable GetEditResignation(string id);


}


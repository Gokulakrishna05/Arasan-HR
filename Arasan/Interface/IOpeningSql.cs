//namespace Arasan.Interface
//{
//    public interface IOpeningSql
//    {
//    }
//}
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface
{
    public interface IOpeningSql
    {
        DataTable GetAllOpeningSql(string dtFrom, string Branch);
     }
}

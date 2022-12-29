using Arasan.Interface;
using System.Collections.Generic;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Arasan.Interface
{
    public interface IQCTestingService
    {
        DataTable GetGRN();
        //DataTable GetGRN(string id);
        DataTable GetGRNDetails(string id);
    }
}

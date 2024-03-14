using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Services;
namespace Arasan.Interface 
{
    public interface IMaterialSplit
    {

        DataTable GetItem(string id);
    }
}

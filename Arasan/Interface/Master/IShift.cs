using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface IShift
    {
        DataTable GetAlLshift(string strStatus);

        public string ShiftCRUD(Shift pp);


    }
}

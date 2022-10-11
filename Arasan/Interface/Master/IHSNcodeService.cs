using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;

namespace Arasan.Interface.Master
{
    public interface IHSNcodeService
    {
        string HSNcodeCRUD(HSNcode by);
        IEnumerable<HSNcode> GetAllHSNcode();
        HSNcode GetHSNcodeById(string id);
    }
}

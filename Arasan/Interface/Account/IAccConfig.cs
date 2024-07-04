using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;                                                    

namespace Arasan.Interface
{
    public interface IAccConfig
    {
        
        string  ConfigCRUD(AccConfig Cy);
        //DataTable GetConfig();
        DataTable Getledger();

        DataTable GetAccConfigItem(string id);
        DataTable GetAccConfig(string id);

        //DataTable Getschemebyid(string id);
     

        //IEnumerable<ConfigItem> GetAllConfigItem(string id);

        //DataTable GetSchemeDetails(string itemId);

        string StatusChange(string tag, string id);

        string RemoveChange(string tag, string id);

        DataTable GetConfigItem(string id);

        DataTable GetConfig(string id);

        DataTable GetAllConfig(string strStatus);
    }
}


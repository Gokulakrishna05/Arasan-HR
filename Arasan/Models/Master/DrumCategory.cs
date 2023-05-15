using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class DrumCategory
    {

        public string ID { get; set; }

        //public string CateType { get; set; }
        //public string Descrip { get; set; }
        //public string Stat { get; set; }

        public string CategoryType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

    }


}

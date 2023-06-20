using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class DrumCategory
    {
        public string ID { get; set; }
        public string CategoryType { get; set; }
        public string Description { get; set; }
        public string status { get; set; }
        public string CateType { get; set; }

    }


}

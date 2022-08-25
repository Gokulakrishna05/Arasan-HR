using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Branch
    {
            public int Id { get; set; }
            public string CompanyName { get; set; }
            public string BranchName { get; set; }
        public string Addr { get; set; }
        public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public int PinCode { get; set; }
            public int GSTNo { get; set; }
            public string GSTDate { get; set; }

       

    }
    }

